using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public UnityEvent<int> OnKnifeCut;
        public List<UnityEvent> OnKnifeCutInterval;

        [SerializeField]
        private InputState state;

        private int samples;
        private float maxSignalInSamples;

        private void Awake()
        {
            state = InputState.WaitingForInput;
        }

        private void OnValidate()
        {
            if (OnKnifeCutInterval.Count < signalRanges.Count - 1)
            {
                for (int i = OnKnifeCutInterval.Count; i < signalRanges.Count - 1; i++)
                {
                    OnKnifeCutInterval.Add(new UnityEvent());
                }
            }
        }

        public void Input(float normalizedSignal)
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

                for (var i = 1; i < signalRanges.Count; i++)
                {
                    float intervalUpperBound = signalRanges[i];
                    if (maxSignalInSamples < intervalUpperBound)
                    {
                        int intervalIndex = i - 1;
                        OnKnifeCut.Invoke(intervalIndex);
                        OnKnifeCutInterval[intervalIndex].Invoke();
                        Debug.Log($"Knife cut detected in interval {intervalIndex}");

                        state = InputState.WaitingForReset;
                        break;
                    }
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
    }
}