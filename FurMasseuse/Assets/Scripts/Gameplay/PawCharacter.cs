using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class PawCharacter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public UnityEvent OnPawHurt;
        public UnityEvent OnSuccessMassage;

        public UnityEvent OnSpawnIn;

        public UnityEvent OnLeave;

        public async UniTaskVoid FailMassage()
        {
            OnPawHurt?.Invoke();
        }

        public async UniTaskVoid SpawnIn()
        {
            OnSpawnIn?.Invoke();
        }

        public async UniTaskVoid SuccessMassage()
        {
            OnSuccessMassage?.Invoke();
        }

        public async UniTaskVoid Leave()
        {
            OnLeave?.Invoke();
            await UniTask.Delay(5);
            Destroy(gameObject);
        }
    }
}