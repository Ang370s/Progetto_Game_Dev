using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject pauseButton; // il quadratino

    [Header("Gamepad Selection")]
    public GameObject firstPauseButton;
    public GameObject firstOptionsButton;

    void Update()
    {
        bool startPressed = Input.GetKeyDown(KeyCode.JoystickButton7) ||
                        Input.GetKeyDown(KeyCode.JoystickButton6) ||
                        Input.GetKeyDown(KeyCode.JoystickButton10) ||
                        Input.GetKeyDown(KeyCode.Escape); // Mantieni Escape per i test su PC

        if (startPressed)
        {
            if (optionsPanel.activeSelf)
            {
                CloseOptions(); // Se sei nelle opzioni, torna al menu pausa
            }
            else if (pausePanel.activeSelf)
            {
                Resume(); // Se sei in pausa, torna al gioco
            }
            else
            {
                TogglePause(); // Se sei nel gioco, apri la pausa
            }
        }

        // TASTO B (JoystickButton1) per tornare indietro quando sei nelle OPZIONI
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (pausePanel.activeSelf)
                Resume(); // Se sei in pausa, torna al gioco
        
            else if (optionsPanel.activeSelf)
                CloseOptions(); // Se sei nelle opzioni, torna al menu pausa
        }
    }

    // Metodo principale per cambiare stato pausa
    public void TogglePause()
    {
        isPaused = !isPaused;

        // Pannelli
        pausePanel.SetActive(isPaused);

        // Quadratino
        if (pauseButton != null)
            pauseButton.SetActive(!isPaused);

        // Blocca o sblocca il gioco
        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            // FORZIAMO la selezione del primo tasto, altrimenti le freccette non sanno dove iniziare
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
        }
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

        // Seleziona il primo pulsante delle opzioni per il gamepad
        SetFirstSelection(firstOptionsButton);

        EventSystem.current.SetSelectedGameObject(firstOptionsButton);
    }


    // Chiudi pannello opzioni e ritorna pausa
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);

        // Torna a selezionare un bottone nel menu pausa
        SetFirstSelection(firstPauseButton);
    }

    // Funzione di supporto per pulire e settare la selezione
    private void SetFirstSelection(GameObject target)
    {
        if (target == null) return;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }

    // Torna al main menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene("MainMenu");

        isPaused = false;
    }
}
