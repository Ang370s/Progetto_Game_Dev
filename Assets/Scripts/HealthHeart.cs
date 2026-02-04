using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart;
    Image heartImage;

    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStaus status)
    {
        switch(status)
        {
            case HeartStaus.Empty:
                heartImage.sprite = emptyHeart;
                break;
            case HeartStaus.Half:
                heartImage.sprite = halfHeart;
                break;
            case HeartStaus.Full:
                heartImage.sprite = fullHeart;
                break;
        }
    }

}

public enum HeartStaus
{
    Empty = 0,
    Half = 1,
    Full = 2,
}   