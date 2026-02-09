using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    private Image keyImage;

    void Awake()
    {
        keyImage = GetComponent<Image>();
        keyImage.enabled = false;
    }

    void Update()
    {
        if (PlayerStats.Instance == null)
            return;

        keyImage.enabled = PlayerStats.Instance.hasKey;
    }
}