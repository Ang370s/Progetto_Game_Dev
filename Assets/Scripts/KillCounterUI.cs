using TMPro;
using UnityEngine;

public class KillCounterUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI killText;

    void Update()
    {
        killText.text = playerStats.killCount.ToString();
    }
}