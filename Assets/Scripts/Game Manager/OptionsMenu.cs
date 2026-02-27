using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    void Update()
    {
        // Se premi B (Button 1) nel menu delle opzioni, torni al menu principale
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            BackToMenu();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}