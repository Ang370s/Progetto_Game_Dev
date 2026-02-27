using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Gamepad Selection")]
    public GameObject firstButton;

    void Update()
    {
        // Se premi B (Button 1) nel menu principale, esci dal gioco
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            QuitGame();
        }
    }

    private void Start()
    {
        // Appena parte la scena, diciamo al sistema di evidenziare il primo tasto
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }

    public void PlayGame()
    {
        Chest.ResetChestStats();

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ResetStats();
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void OpenRecords()
    {
        SceneManager.LoadScene("RecordsScene");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void QuitGame()
    {
        Debug.Log("Exit pressed");

        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}