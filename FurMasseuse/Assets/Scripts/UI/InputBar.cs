using System.Collections.Generic;
using Input;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

namespace UI
{
    public class InputBar : MonoBehaviour
    {
        [SerializeField]
        private Image inputBarFill;

        [SerializeField]
        private Slider inputBar;

        [SerializeField]
        private List<Color> fillColors;

        private void Start()
        {
            ArduinoInput.Instance.OnIntensityChange.AddListener(HandleSignalChanged);
        }

        private void OnDestroy()
        {
            ArduinoInput.Instance.OnIntensityChange.RemoveListener(HandleSignalChanged);
        }

        private void HandleSignalChanged(float signal)
        {
            inputBar.value = signal;

            int index = DiscretizedPressureInput.Instance.GetIntervalIndex(signal);

            inputBarFill.color = fillColors[index + 1];
        }
    }
}