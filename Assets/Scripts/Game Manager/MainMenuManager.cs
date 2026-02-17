using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
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