using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject pauseButton; // il quadratino

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Metodo principale per cambiare stato pausa
    public void TogglePause()
    {
        isPaused = !isPaused;

        // Pannelli
        pausePanel.SetActive(isPaused);
        optionsPanel.SetActive(false); // chiudi options se aperto

        // Quadratino
        if (pauseButton != null)
            pauseButton.SetActive(!isPaused);

        // Blocca o sblocca il gioco
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Metodo chiamato dal pulsante Resume nel menu
    public void Resume()
    {
        if (!isPaused) return; // sicurezza

        isPaused = false;

        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }

    // Restart del livello
    public void Restart()
    {
        Time.timeScale = 1f;


        PlayerStats.Instance.ResetStats();
        Chest.ResetChestStats();
        SceneManager.LoadScene("SampleScene");

        isPaused = false; // resetta lo stato
    }

    // Apri pannello opzioni
    public void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // Chiudi pannello opzioni e ritorna pausa
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // Torna al main menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene("MainMenu");

        isPaused = false;
    }
}
