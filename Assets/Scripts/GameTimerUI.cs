using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    public GameTimer gameTimer;
    public TextMeshProUGUI timerText;

    void Update()
    {
        float time = gameTimer.elapsedTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}