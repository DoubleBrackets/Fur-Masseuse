using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Util
{
    public class SceneLoadMono : MonoBehaviour
    {
        [SerializeField]
        [Scene]
        private string sceneName;

        public void LoadScene()
        {
            SceneLoader.Instance.LoadScene(sceneName);
        }
    }
}