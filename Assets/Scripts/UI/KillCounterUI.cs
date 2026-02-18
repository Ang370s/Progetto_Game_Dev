using TMPro;
using UnityEngine;

public class KillCounterUI : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI killText;

    /* void Start()
     {
         UpdateDisplay();
     }*/

    void Start()
    {
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.ResetStats();
        if (playerStats == null)
            playerStats = PlayerStats.Instance;
    }

    void Update()
    {
        if (PlayerStats.Instance == null) return;

        killText.text = PlayerStats.Instance.killCount.ToString();
    }

    /*void Start()
    {
        if (playerStats == null)
            playerStats = PlayerStats.Instance;

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (playerStats == null) return;

        killText.text = playerStats.killCount.ToString();
    }*/

}