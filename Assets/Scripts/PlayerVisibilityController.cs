using UnityEngine;

public class PlayerVisibilityController : MonoBehaviour
{
    public static void SetVisibility()
    {
        GameObject player = GlobalVars.player;
        player.SetActive(!player.activeInHierarchy);
    }
}
