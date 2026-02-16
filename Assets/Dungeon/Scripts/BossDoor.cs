using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoor : MonoBehaviour
{
    [SerializeField] private string bossSceneName = "BossScene";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(bossSceneName);
        }
    }
}
