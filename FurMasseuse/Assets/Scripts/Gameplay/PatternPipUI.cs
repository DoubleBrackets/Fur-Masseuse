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

        [Header("Events")]

        public UnityEvent OnFinished;

        public UnityEvent OnUnfinished;

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
                    pipImage.color = Color.green;
                    break;
                case MainGameplay.MassageStrength.Medium:
                    pipImage.color = Color.yellow;
                    break;
                case MainGameplay.MassageStrength.High:
                    pipImage.color = Color.red;
                    break;
            }
        }
    }
}