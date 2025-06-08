using UnityEngine;

public class AudioOneshot : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    public void Play()
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
    }
}