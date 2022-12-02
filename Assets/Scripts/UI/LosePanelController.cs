using UnityEngine;

public class LosePanelController : MonoBehaviour
{
    private void Start()
    {
        LoseController.playerLose += EnablePanel;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        LoseController.playerLose -= EnablePanel;
    }

    private void EnablePanel()
    {
        gameObject.SetActive(true);
    } 
}
