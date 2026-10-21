using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    
    [Header("Clips")]
    public AudioClip music;
    public AudioClip flapSound;
    public AudioClip metalHitSound;
    public AudioClip brickHitSound;
    public AudioClip collectSound;

    [Header("Flap Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.4f;
    [Range(0f, 1f)] public float flapVolume = 0.3f;
    [Range(0f, 1f)] public float hitVolume = 0.8f;
    [Range(0f, 1f)] public float collectVolume = 0.6f;

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
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayFlap()
    {
        if (flapSound != null)
        {
            AudioSource.PlayClipAtPoint(flapSound, Camera.main.transform.position, flapVolume);
        }
    }

        public void PlayTopHit()
    {
        if (metalHitSound != null)
        {
            AudioSource.PlayClipAtPoint(metalHitSound, Camera.main.transform.position, hitVolume);
        }
    }

    public void PlayBottomHit()
    {
        if (brickHitSound != null)
        {
            AudioSource.PlayClipAtPoint(brickHitSound, Camera.main.transform.position, hitVolume);
        }
    }

    public void PlayCollect()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, collectVolume);
        }
    }
}