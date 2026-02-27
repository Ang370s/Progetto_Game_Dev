using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIInputSwitcher : MonoBehaviour
{
    public static UIInputSwitcher Instance; // Singleton per trovarlo ovunque

    private GameObject lastSelected;
    private bool isUsingGamepad = true;

    [Header("Input Prompts UI (Automatic Search)")]
    private GameObject pcPanel;
    private GameObject consolePanel;
    private GameObject mobilePanel;

    private void Start()
    {
        // Rileviamo se siamo su Android per mostrare i comandi R36S di default
#if UNITY_ANDROID
        SwitchVisuals(false); // Parte in modalità Gamepad
#else
        SwitchVisuals(true);  // Parte in modalità PC
#endif
    }

    private void Awake()
    {
        // LOGICA PER NON FARLO MORIRE TRA LE SCENE
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita che se torni al Menu se ne crei un secondo
            return;
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Al cambio scena, resettiamo i riferimenti (verranno ricollegati dal Linker)
        pcPanel = null;
        consolePanel = null;
        mobilePanel = null;

        StartCoroutine(SetInitialSelection());
    }
    private IEnumerator SetInitialSelection()
    {
        yield return new WaitForEndOfFrame();
        if (EventSystem.current != null && EventSystem.current.firstSelectedGameObject != null)
        {
            lastSelected = EventSystem.current.firstSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(lastSelected);
            isUsingGamepad = true;
            SwitchVisuals(false);
        }
    }

    void Update()
    {
        if (EventSystem.current == null) return;

        // 1. RILEVA MOUSE
        float mouseMove = Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y");
        if (Mathf.Abs(mouseMove) > 0.1f || Input.GetMouseButtonDown(0))
        {
            if (isUsingGamepad)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                    lastSelected = EventSystem.current.currentSelectedGameObject;

                EventSystem.current.SetSelectedGameObject(null);
                isUsingGamepad = false;
                SwitchVisuals(true);
            }
        }

        // 2. RILEVA GAMEPAD/TASTI
        float gamepadMove = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(gamepadMove) > 0.1f || Input.anyKeyDown)
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
            {
                if (!isUsingGamepad || EventSystem.current.currentSelectedGameObject == null)
                {
                    isUsingGamepad = true;
                    SwitchVisuals(false);
                    // LOGICA DI RECUPERO: Se non c'è nulla di selezionato, forza l'ultimo o il primo
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        GameObject toSelect = (lastSelected != null) ? lastSelected : EventSystem.current.firstSelectedGameObject;

                        // Se ancora nullo, cerchiamo il primo bottone attivo nel pannello corrente
                        if (toSelect == null || !toSelect.activeInHierarchy)
                        {
                            toSelect = FindFirstActiveButton();
                        }

                        EventSystem.current.SetSelectedGameObject(toSelect);
                    }
                }
            }
        }
    }

    // Metodo chiamato dal Linker per collegare i pannelli della scena
    public void RegisterPanels(GameObject pc, GameObject console, GameObject mobile)
    {
        pcPanel = pc;
        consolePanel = console;
        mobilePanel = mobile;

        // Appena registrati, aggiorna la visuale in base alla modalità attuale
        SwitchVisuals(!isUsingGamepad);
    }

    // Funzione per cambiare i riquadri visivi tra PC, Console e Mobile
    private void SwitchVisuals(bool isPC)
    {
        // Se non ci sono pannelli registrati in questa scena, non fare nulla
        if (pcPanel == null && consolePanel == null && mobilePanel == null) return;

        // Se isPC è true, mostriamo WASD e spegniamo il resto
        if (isPC)
        {
            if (pcPanel != null) pcPanel.SetActive(true);
            if (consolePanel != null) consolePanel.SetActive(false);
            if (mobilePanel != null) mobilePanel.SetActive(false);
        }
        else
        {
            // Se non è PC, dobbiamo capire se è un Controller (R36S) o Touch puro
            // Controlliamo se c'è un joystick collegato o se siamo su Android/iOS senza input mouse
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                if (Input.GetJoystickNames().Length == 0)
                {
                    // R36S o Gamepad collegato
                    if (pcPanel != null) pcPanel.SetActive(false);
                    if (consolePanel != null) consolePanel.SetActive(true);
                    if (mobilePanel != null) mobilePanel.SetActive(false);
                }
                else
                {
                    // Touch puro (Telefono)// Telefono Touch puro
                    if (pcPanel != null) pcPanel.SetActive(false);
                    if (consolePanel != null) consolePanel.SetActive(false);
                    if (mobilePanel != null) mobilePanel.SetActive(true);
                }
#else
            // Caso di fallback (es. controller collegato al PC)
            if (pcPanel != null) pcPanel.SetActive(false);
            if (consolePanel != null) consolePanel.SetActive(true);
            if (mobilePanel != null) mobilePanel.SetActive(false);
#endif
        }
    }

    public bool IsUsingGamepad() { return isUsingGamepad; }


    // Cerca il primo oggetto selezionabile attivo nella scena se tutto il resto fallisce
    private GameObject FindFirstActiveButton()
    {
        Selectable[] allSelectables = FindObjectsOfType<Selectable>();
        foreach (Selectable s in allSelectables)
        {
            if (s.gameObject.activeInHierarchy && s.interactable) return s.gameObject;
        }
        return null;
    }

}