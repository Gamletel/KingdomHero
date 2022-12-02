using UnityEngine;

public class WinPanelController : MonoBehaviour
{
    private void Start()
    {
        WinController.playerWin += EnableWinPanel;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        WinController.playerWin -= EnableWinPanel;
    }

    private void EnableWinPanel()
    {
        gameObject.SetActive(true);
    }
}
