using System.Collections.Generic;
using Input;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class MainGameplay : MonoBehaviour
    {
        public enum MassageStrength
        {
            Low,
            Medium,
            High
        }

        public static int ScoreStatic;

        [Header("Config")]

        [SerializeField]
        private int patternLength;

        [SerializeField]
        private float gameplayTime;

        [SerializeField]
        private float wrongPatternPenalty;

        [Header("Events")]

        public UnityEvent<List<MassageStrength>> OnPatternCreated;

        public UnityEvent<int> OnStepCompleted;

        public UnityEvent<float> OnGameplayTimerUpdated;

        public UnityEvent OnGameplayTimerExpired;

        public UnityEvent<int> OnScoreUpdated;

        private List<MassageStrength> currentPattern;
        private int currentPatternIndex;

        private float gameplayTimer;
        private int score;

        private void Start()
        {
            currentPatternIndex = 0;
            gameplayTimer = gameplayTime;
            SetScore(0);

            CreateNewPattern();
            DiscretizedPressureInput.Instance.OnDiscreteTriggered.AddListener(HandleDiscreteInput);
        }

        private void Update()
        {
            if (gameplayTimer > 0)
            {
                gameplayTimer -= Time.deltaTime;
                OnGameplayTimerUpdated?.Invoke(gameplayTimer);
            }
            else
            {
                Debug.Log("Time Over!");

                gameplayTimer = 0;
                OnGameplayTimerUpdated?.Invoke(gameplayTimer);
                OnGameplayTimerExpired?.Invoke();
            }
        }

        private void OnDestroy()
        {
            DiscretizedPressureInput.Instance.OnDiscreteTriggered.RemoveListener(HandleDiscreteInput);
        }

        private void HandleDiscreteInput(int intervalIndex)
        {
            var input = (MassageStrength)intervalIndex;
            HandlePatternInput(input);
        }

        public List<MassageStrength> CreateNewPattern()
        {
            currentPattern = new List<MassageStrength>();
            for (var i = 0; i < patternLength; i++)
            {
                currentPattern.Add((MassageStrength)Random.Range(0, 3));
            }

            currentPatternIndex = 0;

            OnPatternCreated?.Invoke(currentPattern);

            return currentPattern;
        }

        public void HandlePatternInput(MassageStrength input)
        {
            if (input == currentPattern[currentPatternIndex])
            {
                Debug.Log($"Input accepted! Expected: {currentPattern[currentPatternIndex]}, Got: {input}");
                currentPatternIndex++;
                OnStepCompleted?.Invoke(currentPatternIndex);
                if (currentPatternIndex >= currentPattern.Count)
                {
                    // Pattern completed
                    SetScore(score + 1);
                    OnScoreUpdated?.Invoke(score);
                    CreateNewPattern();
                }
            }
            else
            {
                Debug.Log($"Input failed! Expected: {currentPattern[currentPatternIndex]}, Got: {input}");
                gameplayTimer -= wrongPatternPenalty;
            }
        }

        private void SetScore(int newScore)
        {
            score = newScore;
            ScoreStatic = newScore;
            OnScoreUpdated?.Invoke(score);
        }
    }
}