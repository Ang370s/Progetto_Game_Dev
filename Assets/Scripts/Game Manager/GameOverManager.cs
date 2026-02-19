using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;

    void Start()
    {
        if (PlayerStats.Instance != null)
        {
            int totale = PlayerStats.Instance.GetFinalScore();
            scoreDisplay.text = totale.ToString();

            Debug.Log("Game Over. Score: " + totale);
        }
        else
        {
            scoreDisplay.text = "0";
        }
    }

    public void Retry()
    {
        if (PlayerStats.Instance != null)
        {
            Destroy(PlayerStats.Instance.gameObject);
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void BackToMenu()
    {
        if (PlayerStats.Instance != null)
        {
            Destroy(PlayerStats.Instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
