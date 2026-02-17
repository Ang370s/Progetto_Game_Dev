using UnityEngine;
using UnityEngine.SceneManagement;

public class RecordsMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
