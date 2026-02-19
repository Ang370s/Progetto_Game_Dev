using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("SFX Clips")]
    public AudioClip sword;
    public AudioClip bow;
    public AudioClip damage;
    public AudioClip click;
    public AudioClip chestBreak;
    public AudioClip heal;
    public AudioClip gem;
    public AudioClip door;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip, volume);
    }

    public void playClick()
    {
        PlaySFX(click);
    }
}
