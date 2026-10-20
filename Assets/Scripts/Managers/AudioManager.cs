using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [Header("Clips")]
    public AudioClip music;

    void Awake()
    {
        Instance = this;
    }

    public void PlayMusic()
    {
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.volume = 0.4f;
            musicSource.Play();
        // }
    }
}