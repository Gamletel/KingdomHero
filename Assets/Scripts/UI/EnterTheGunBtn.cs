using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTheGunBtn : MonoBehaviour
{
    public static GameObject enterTheGunBtn { get; private set; }

    private void Awake()
    {
        enterTheGunBtn = GameObject.Find("EnterTheGunBtn");
        enterTheGunBtn.SetActive(false);
    }

    public void EnterTheGun()
    {
        gameObject.SetActive(false);
        GlobalVars.moveJoystick.SetActive(false);
        FireJoystickController.FireJoystickObj.SetActive(true);
        EnterTheGunController.enteredTheGun = true;
        GunCameraController.SetCameraActive(true);
        EnterTheGunController.OnEnterTheGun(true);
    }
}
