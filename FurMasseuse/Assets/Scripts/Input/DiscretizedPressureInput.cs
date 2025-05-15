using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Input
{
    public class DiscretizedPressureInput : MonoBehaviour
    {
        private enum InputState
        {
            WaitingForInput,
            Sampling,
            WaitingForReset
        }

        [SerializeField]
        private List<float> signalRanges;

        [SerializeField]
        private int samplingSize;

        [SerializeField]
        private bool log;

        [FormerlySerializedAs("OnKnifeCut")]
        public UnityEvent<int> OnDiscreteTriggered;

        [FormerlySerializedAs("OnKnifeCutInterval")]
        public List<UnityEvent> OnDiscreteTriggeredInterval;

        [SerializeField]
        private InputState state;

        [SerializeField]
        private List<KeyCode> intervalKeyboardInputs;

        public static DiscretizedPressureInput Instance { get; private set; }

        private int samples;
        private float maxSignalInSamples;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            state = InputState.WaitingForInput;
        }

        private void Update()
        {
            for (var i = 0; i < intervalKeyboardInputs.Count; i++)
            {
                if (UnityEngine.Input.GetKeyDown(intervalKeyboardInputs[i]))
                {
                    OnDiscreteTriggeredInterval[i].Invoke();
                    OnDiscreteTriggered.Invoke(i);
                }
            }
        }

        private void OnValidate()
        {
            if (OnDiscreteTriggeredInterval.Count < signalRanges.Count - 1)
            {
                for (int i = OnDiscreteTriggeredInterval.Count; i < signalRanges.Count - 1; i++)
                {
                    OnDiscreteTriggeredInterval.Add(new UnityEvent());
                }
            }
        }

        public void HandleSignal(float normalizedSignal)
        {
            if (state == InputState.WaitingForInput)
            {
                HandleWaitForInput(normalizedSignal);
            }

            if (state == InputState.Sampling)
            {
                HandleSampling(normalizedSignal);
            }

            if (state == InputState.WaitingForReset)
            {
                HandleWaitForReset(normalizedSignal);
            }
        }

        private void HandleWaitForInput(float normalizedSignal)
        {
            if (normalizedSignal > signalRanges[0])
            {
                state = InputState.Sampling;
                samples = 0;
                maxSignalInSamples = normalizedSignal;
            }
        }

        private void HandleWaitForReset(float normalizedSignal)
        {
            if (normalizedSignal < signalRanges[0])
            {
                state = InputState.WaitingForInput;
            }
        }

        private void HandleSampling(float normalizedSignal)
        {
            samples++;
            if (samples > samplingSize)
            {
                state = InputState.WaitingForInput;

                int index = GetIntervalIndex(normalizedSignal);

                if (index != -1)
                {
                    OnDiscreteTriggered.Invoke(index);
                    OnDiscreteTriggeredInterval[index].Invoke();
                    Debug.Log($"Knife cut detected in interval {index}");

                    state = InputState.WaitingForReset;
                }
            }
            else
            {
                if (normalizedSignal > maxSignalInSamples)
                {
                    maxSignalInSamples = normalizedSignal;
                }
            }
        }

        public int GetIntervalIndex(float signal)
        {
            for (var i = 1; i < signalRanges.Count; i++)
            {
                float intervalUpperBound = signalRanges[i];
                if (signal < intervalUpperBound)
                {
                    return i - 1;
                }
            }

            return -1;
        }
    }
}