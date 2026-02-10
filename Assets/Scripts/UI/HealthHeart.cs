using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite fullHeart, halfHeart, emptyHeart; // Sprites for different heart states
    Image heartImage; // Reference to the Image component

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        heartImage = GetComponent<Image>(); // Get the Image component attached to this GameObject
    }

    // Method to set the heart image based on the heart status
    public void SetHeartImage(HeartStaus status)
    {
        // Change the sprite based on the heart status
        switch (status)
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

// Enum to represent the status of a heart
public enum HeartStaus
{
    Empty = 0,
    Half = 1,
    Full = 2,
}   