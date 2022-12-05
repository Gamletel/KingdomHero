using UnityEngine;

public class PlayerVisibilityController : MonoBehaviour
{
    private void Start()
    {
        LoseController.playerLose += DisablePlayer;
    }

    private void OnDestroy()
    {
        LoseController.playerLose -= DisablePlayer;
    }

    public static void SetVisibility()
    {
        GlobalVars.player.SetActive(!GlobalVars.player.activeInHierarchy);
    }

    private void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}
