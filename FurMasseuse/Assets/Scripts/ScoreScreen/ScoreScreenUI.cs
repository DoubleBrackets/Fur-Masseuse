using Gameplay;
using TMPro;
using UnityEngine;

namespace ScoreScreen
{
    public class ScoreScreenUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        private void Start()
        {
            scoreText.text = MainGameplay.ScoreStatic.ToString();
        }
    }
}