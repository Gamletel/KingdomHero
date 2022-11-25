using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController : MonoBehaviour
{
    public delegate void PlayerLose();
    public static event PlayerLose playerLose;

    public static void OnPlayerLose()
    {
        playerLose?.Invoke();
    }
}
