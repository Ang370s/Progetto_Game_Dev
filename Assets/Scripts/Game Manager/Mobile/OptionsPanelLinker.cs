using UnityEngine;

public class OptionsPanelLinker : MonoBehaviour
{
    public GameObject pcPanel;
    public GameObject consolePanel;
    public GameObject mobilePanel;

    // Questo serve per quando carichi la SCENA intera (Menu Opzioni)
    void Start()
    {
        Register();
    }

    // Questo serve per quando attivi il PANNELLO in gioco (Pausa)
    void OnEnable()
    {
        Register();
    }

    private void Register()
    {
        if (UIInputSwitcher.Instance != null)
        {
            UIInputSwitcher.Instance.RegisterPanels(pcPanel, consolePanel, mobilePanel);
        }
    }
}