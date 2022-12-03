using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTheGunBtn : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ExitTheGun()
    {
        gameObject.SetActive(false);
        GlobalVars.moveJoystick.SetActive(true);
        FireJoystickController.FireJoystickObj.SetActive(false);
        EnterTheGunController.enteredTheGun = false;
        GunCameraController.SetCameraActive(false);
        EnterTheGunController.OnEnterTheGun(false);
    }
}
