using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class ArduinoInputMono : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<float> onSignalChanged;

        [SerializeField]
        private List<UnityEvent> onDiscreteTriggeredInterval;

        private void Start()
        {
            ArduinoInput.Instance.OnIntensityChange.AddListener(HandleSignalChanged);
            DiscretizedPressureInput.Instance.OnDiscreteTriggered.AddListener(HandleDiscreteTriggered);
        }

        private void OnDestroy()
        {
            ArduinoInput.Instance.OnIntensityChange.RemoveListener(HandleSignalChanged);
            DiscretizedPressureInput.Instance.OnDiscreteTriggered.RemoveListener(HandleDiscreteTriggered);
        }

        private void HandleSignalChanged(float signal)
        {
            onSignalChanged?.Invoke(signal);
        }

        private void HandleDiscreteTriggered(int index)
        {
            if (index < onDiscreteTriggeredInterval.Count)
            {
                onDiscreteTriggeredInterval[index]?.Invoke();
            }
        }
    }
}