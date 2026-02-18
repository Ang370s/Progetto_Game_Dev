using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;
    public AudioClip gameOverMusic;
    public AudioClip victoryMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.playOnAwake = false;

            // ✅ QUI carichiamo subito il volume salvato
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            audioSource.volume = savedVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            // 🎵 MENU GROUP
            case "MainMenu":
            case "OptionsScene":
            case "RecordsScene":
                PlayMusic(menuMusic);
                break;

            // 🎮 GAME
            case "SampleScene":
                PlayMusic(gameMusic);
                break;

            // 👹 BOSS
            case "BossFightScene":
                PlayMusic(bossMusic);
                break;

            // 💀 GAME OVER
            case "GameOverScene":
                PlayMusic(gameOverMusic);
                break;

            // 🏆 VICTORY
            case "VictoryScene":
                PlayMusic(victoryMusic);
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip)
            return;

        audioSource.clip = clip;

        // SOLO GameOver non in loop
        if (clip == gameOverMusic)
            audioSource.loop = false;
        else
            audioSource.loop = true;

        audioSource.Play();
    }


    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
