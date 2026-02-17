using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;

    void Start()
    {
        // Controlliamo se l'oggetto immortale esiste
        if (PlayerStats.Instance != null)
        {
            // 1. Recupera il punteggio
            int totale = PlayerStats.Instance.GetFinalScore();

            // 2. Scrive il testo a schermo
            scoreDisplay.text = totale.ToString();

            // 3. Nascondi la vecchia UI del dungeon (i contatori piccoli)
            Canvas oldUI = PlayerStats.Instance.GetComponentInChildren<Canvas>();
            if (oldUI != null) oldUI.enabled = false;

            Debug.Log("Punteggio caricato correttamente: " + totale);
            Debug.Log("Vittoria! Kill: " + PlayerStats.Instance.killCount + " Boss: " + PlayerStats.Instance.bossDefeated);

            // 4. Salva il punteggio
            ScoreManager.Instance.SaveNewScore(totale);
        }
        else
        {
            // Se arrivi qui senza passare dal gioco (es. lanci la scena da sola)
            scoreDisplay.text = "0";
            Debug.LogWarning("PlayerStats non trovato. Hai lanciato la scena direttamente?");
        }
    }

    public void BackToMenu()
    {
        // Distruggiamo l'oggetto immortale per resettare i punti per la prossima partita
        if (PlayerStats.Instance != null)
        {
            Destroy(PlayerStats.Instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
