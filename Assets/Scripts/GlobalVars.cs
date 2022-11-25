using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public static Transform lookAtPoint { get; private set; }
    public static GameObject player { get; private set; }
    public static GameObject moveJoystick { get; private set; }

    private void Awake()
    {
        lookAtPoint = GameObject.FindGameObjectWithTag("PointForShoot").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        moveJoystick = GameObject.Find("MoveJoystick");
    }

    public static bool IsPlayerMoving()
    {
        return moveJoystick.GetComponent<FixedJoystick>().Direction != Vector2.zero && !Input.GetMouseButtonUp(0);
    }
}
