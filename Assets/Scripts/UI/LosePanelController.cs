using UnityEngine;

public class LosePanelController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void EnablePanel()
    {
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        LoseController.playerLose -= EnablePanel;
    }

    private void OnDisable()
    {
        LoseController.playerLose += EnablePanel;
    }
}
