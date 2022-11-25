using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public delegate void PlayerWin();
    public static event PlayerWin playerWin;

    public static void OnPlayerWin()
    {
        playerWin?.Invoke();
    }
}
