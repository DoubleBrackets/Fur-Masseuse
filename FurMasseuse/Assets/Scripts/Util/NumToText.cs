using TMPro;
using UnityEngine;

namespace Util
{
    public class NumToText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        public void SetText(int number)
        {
            text.text = number.ToString();
        }

        public void SetText(float number)
        {
            text.text = number.ToString("F2");
        }
    }
}