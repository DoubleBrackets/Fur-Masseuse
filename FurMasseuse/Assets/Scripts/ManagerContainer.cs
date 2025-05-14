using UnityEngine;

public class ManagerContainer : MonoBehaviour
{
    private static ManagerContainer instance;

    [SerializeField]
    private GameObject _managerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Instantiate(_managerPrefab, transform);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}