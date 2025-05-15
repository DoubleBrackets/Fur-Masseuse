using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PawCharacterManager : MonoBehaviour
    {
        [SerializeField]
        private List<PawCharacter> pawCharacterPrefabs;

        [SerializeField]
        private Vector2 spawnAngleRange;

        [SerializeField]
        private Transform spawnPoint;

        private PawCharacter currentPawCharacter;

        private int previousCharIndex;

        public void BringInNewCharacter()
        {
            if (currentPawCharacter != null)
            {
                currentPawCharacter.Leave().Forget();
            }

            CreateNewChar();
        }

        private void CreateNewChar()
        {
            if (currentPawCharacter != null)
            {
                currentPawCharacter.Leave().Forget();
            }

            int randomIndex = Random.Range(0, pawCharacterPrefabs.Count);

            while (randomIndex == previousCharIndex)
            {
                randomIndex = Random.Range(0, pawCharacterPrefabs.Count);
            }

            previousCharIndex = randomIndex;

            currentPawCharacter =
                Instantiate(pawCharacterPrefabs[randomIndex], spawnPoint);

            currentPawCharacter.transform.rotation =
                Quaternion.Euler(0, 0, Random.Range(spawnAngleRange.x, spawnAngleRange.y));
        }

        public void Success()
        {
            if (currentPawCharacter != null)
            {
                currentPawCharacter.SuccessMassage().Forget();
            }
        }

        public void Fail()
        {
            if (currentPawCharacter != null)
            {
                currentPawCharacter.FailMassage().Forget();
            }
        }
    }
}