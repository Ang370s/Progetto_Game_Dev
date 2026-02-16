using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance; // PROVA

    public float finalTimeSaved = 0f; // Variabile "cassaforte"

    public bool bossDefeated = false; // Nuova variabile
    public bool hasKey = false;
    public int killCount = 0;
    public bool playerDied = false;

    [Header("Player Health")]
    public float savedHealth;

    [Header("Inventory")]
    public int savedPotions = 0;

    [Header("Gems")]
    public int diamondCount = 0;
    public int emeraldCount = 0;
    public int goldCount = 0;

    [Header("Gem UI")]
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI emeraldText;
    public TextMeshProUGUI goldText;

    [Header("Potion Drop")]
    public int killsForPotion = 3;
    public GameObject potionPrefab;

    void Update()
    {
        // Se premi il tasto P sulla tastiera, stampa il punteggio finale
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("--- TEST PUNTEGGIO ---");
            Debug.Log("Totale calcolato: " + GetFinalScore());
        }
    }

    void Awake()
    {

        Debug.Log("Awake chiamato su: " + gameObject.name);

        if (Instance == null)
        {
            Instance = this;
            // Rende l'oggetto immortale tra le scene
            DontDestroyOnLoad(gameObject);
            Debug.Log("Istanza creata con successo. Ora sono in DontDestroyOnLoad.");
        }
        else
        {
            Debug.LogWarning("Esiste già un'altra istanza di PlayerStats! Distruggo questo duplicato: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        diamondText = GameObject.Find("DiamondText")?.GetComponent<TextMeshProUGUI>();
        emeraldText = GameObject.Find("EmeraldText")?.GetComponent<TextMeshProUGUI>();
        goldText = GameObject.Find("GoldText")?.GetComponent<TextMeshProUGUI>();

        UpdateUI();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // --- CALCOLO PUNTEGGIO FINALE ---
    public int GetFinalScore()
    {
        int score = 0;
        score += goldCount * 10;
        score += emeraldCount * 50;
        score += diamondCount * 200;
        score += killCount * 20; // Solo nemici comuni

        if (hasKey) score += 500;

        // BONUS BOSS: Un bel premio per la vittoria
        if (bossDefeated) score += 2000;

        // BONUS TEMPO: Più sei veloce, più punti fai

        float timeToCalculate = finalTimeSaved;

        // Se siamo ancora nel dungeon e premiamo P, finalTimeSaved è 0, 
        // quindi proviamo a leggere il timer live per il debug
        if (timeToCalculate <= 0)
        {
            GameTimer liveTimer = FindObjectOfType<GameTimer>();
            if (liveTimer != null) timeToCalculate = liveTimer.GetTime();
        }

        int timeBonus = Mathf.Max(0, 5000 - (int)(timeToCalculate * 10));
        score += timeBonus;

        if (playerDied) score = Mathf.Max(0, score - 5000); // Penalità per la morte

        if (score < 0) score = 0; // Non permettiamo punteggi negativi

        return score;
    }

    // Aggiungiamo un metodo per resettare le statistiche se ricominci il gioco
 public void ResetStats()
    {
        killCount = 0;
        diamondCount = 0;
        emeraldCount = 0;
        goldCount = 0;
        hasKey = false;
        bossDefeated = false;
        playerDied = false;

        finalTimeSaved = 0f;
        savedHealth = 0;
        savedPotions = 0;

        UpdateUI();
    }

    public void AddKill(Vector3 enemyPosition,  bool isBoss = false)
    {
        if (isBoss)
        {
            bossDefeated = true;
            Debug.Log("BossDefeated impostato a TRUE");
        }
        else
        {
            killCount++;
            if (killCount % killsForPotion == 0) DropPotion(enemyPosition);
        }
        
        UpdateUI();
    }

    void DropPotion(Vector3 position)
    {
        Instantiate(potionPrefab, position, Quaternion.identity);
    }

    public void AddGem(GemType type, int amount = 1)
    {
        switch (type)
        {
            case GemType.Diamond:
                diamondCount += amount;
                if (diamondText != null) diamondText.text = diamondCount.ToString();
                break;

            case GemType.Emerald:
                emeraldCount += amount;
                if (emeraldText != null) emeraldText.text = emeraldCount.ToString();
                break;

            case GemType.Gold:
                goldCount += amount;
                if (goldText != null) goldText.text = goldCount.ToString();
                break;
        }
        
        UpdateUI();

        // TEST
        Debug.Log($"Gemma raccolta! Punteggio attuale calcolato: {GetFinalScore()}");
    }

    public void UpdateUI()
    {
        // Usiamo il controllo null perché nel Menu o nella Vittoria 
        // questi testi potrebbero non esistere
        if (diamondText != null) diamondText.text = diamondCount.ToString();
        if (emeraldText != null) emeraldText.text = emeraldCount.ToString();
        if (goldText != null) goldText.text = goldCount.ToString();
    }

    public void GetKey()
    {
        hasKey = true;
    }

    // Nuovo metodo per salvare il tempo prima del cambio scena
    public void SaveTimeBeforeExit(float time)
    {
        finalTimeSaved = time;
    }

    public void SaveTimeFromTimer()
    {
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
        {
            timer.StopTimer();
            finalTimeSaved = timer.GetTime();
        }
    }

}