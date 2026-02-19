using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordsMenu : MonoBehaviour
{
    public TextMeshProUGUI recordsText;

    void Start()
    {
        ShowRecords();
    }

    void ShowRecords()
    {
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager è NULL!");
            return;
        }

        if (recordsText == null)
        {
            Debug.LogError("recordsText NON assegnato!");
            return;
        }

        if (ScoreManager.Instance.scoreList == null || ScoreManager.Instance.scoreList.scores == null || ScoreManager.Instance.scoreList.scores.Count == 0)
        {
            recordsText.text = "Nessun record ancora registrato.\nGioca per essere il primo!";
            return;
        }
        var scores = ScoreManager.Instance.scoreList.scores;

        int maxToShow = Mathf.Min(5, scores.Count);

        for (int i = 0; i < maxToShow; i++)
        {
            recordsText.text += $"\n{i + 1}. {scores[i].score} - {scores[i].date}\n";
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
