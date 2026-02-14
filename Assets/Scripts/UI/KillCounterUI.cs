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
        if (playerStats == null)
            playerStats = PlayerStats.Instance;
    }

    void Update()
    {
        killText.text = playerStats.killCount.ToString();
    }
}