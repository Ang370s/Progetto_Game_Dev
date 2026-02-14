using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    public GameTimer gameTimer;
    public TextMeshProUGUI timerText;

    void Start()
    {
        gameTimer = FindObjectOfType<GameTimer>();
    }

    void Update()
    {
        if (gameTimer == null) return;

        float time = gameTimer.elapsedTime;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

}