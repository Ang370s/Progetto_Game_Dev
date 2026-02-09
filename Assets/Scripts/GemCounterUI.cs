using TMPro;
using UnityEngine;

public class GemCounterUI : MonoBehaviour
{
    public PlayerStats stats;
    public TextMeshProUGUI gemText;

    void Update()
    {
        gemText.text = stats.gemCount.ToString();
    }
}