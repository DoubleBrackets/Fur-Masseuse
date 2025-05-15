using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay
{
    public class PatternPipUI : MonoBehaviour
    {
        public enum State
        {
            Unititialized,
            Unfinished,
            Finished
        }

        [Header("Depends")]

        [SerializeField]
        private Image pipImage;

        [SerializeField]
        private List<Sprite> typeSprites;

        [Header("Events")]

        public UnityEvent OnFinished;

        public UnityEvent OnUnfinished;

        public UnityEvent OnHide;

        public UnityEvent OnShow;

        [Header("Config")]

        [SerializeField]
        private Vector2 rotationRange;

        private State currentState = State.Unititialized;

        public void Initialize()
        {
            SetState(State.Unfinished);
        }

        public void SetState(State state)
        {
            if (currentState == state)
            {
                return;
            }

            currentState = state;

            switch (state)
            {
                case State.Unfinished:
                    OnUnfinished?.Invoke();
                    break;
                case State.Finished:
                    OnFinished?.Invoke();
                    break;
            }
        }

        public void SetType(MainGameplay.MassageStrength strength)
        {
            switch (strength)
            {
                case MainGameplay.MassageStrength.Low:
                    pipImage.sprite = typeSprites[0];
                    break;
                case MainGameplay.MassageStrength.Medium:
                    pipImage.sprite = typeSprites[1];
                    break;
                case MainGameplay.MassageStrength.High:
                    pipImage.sprite = typeSprites[2];
                    break;
            }
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }

        public void Show()
        {
            // Randomize rotation
            float randomRotation = Random.Range(-rotationRange.x, rotationRange.x);
            transform.rotation = Quaternion.Euler(0, 0, randomRotation);
            OnShow?.Invoke();
        }
    }
}