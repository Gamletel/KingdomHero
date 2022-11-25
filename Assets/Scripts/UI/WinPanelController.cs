using UnityEngine;

public class WinPanelController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void EnableWinPanel()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        WinController.playerWin -= EnableWinPanel;
    }

    private void OnDisable()
    {
        WinController.playerWin += EnableWinPanel;
    }
}
