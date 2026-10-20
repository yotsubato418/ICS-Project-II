using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    
    [Header("Clips")]
    public AudioClip music;
    public AudioClip flapSound;

    [Header("Flap Settings")]
    [Range(0f, 1f)] public float flapVolume = 0.3f;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic()
    {
        if (musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = 0.4f;
        musicSource.Play();
    }

    public void PlayFlap()
    {
        if (flapSound != null)
        {
            AudioSource.PlayClipAtPoint(flapSound, Camera.main.transform.position, flapVolume);
        }
    }
}