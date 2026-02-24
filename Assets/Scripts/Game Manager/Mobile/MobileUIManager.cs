using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance;

    public GameObject joystick;
    public GameObject attackButton;
    public GameObject doorButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bool isMobile;// = Application.isMobilePlatform;

#if UNITY_ANDROID || UNITY_IOS
        isMobile = true;
#else
        isMobile = false;
#endif

        Debug.Log("Is Mobile Platform: " + isMobile);


        if (joystick != null)
            joystick.SetActive(isMobile);

        if (attackButton != null)
            attackButton.SetActive(isMobile);

        if (doorButton != null)
            doorButton.SetActive(false);
    }
}