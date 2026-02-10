using TMPro;
using UnityEngine;



/* Da togliere */
public class GemCounterUI : MonoBehaviour
{
    public PlayerStats stats;
    public GemType gemType;
    public TextMeshProUGUI gemText;

    void Update()
    {
        switch (gemType)
        {
            case GemType.Diamond:
                gemText.text = stats.diamondCount.ToString();
                break;

            case GemType.Emerald:
                gemText.text = stats.emeraldCount.ToString();
                break;

            case GemType.Gold:
                gemText.text = stats.goldCount.ToString();
                break;
        }
    }
}