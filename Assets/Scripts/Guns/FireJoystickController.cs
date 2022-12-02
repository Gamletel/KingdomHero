using UnityEngine;

public class FireJoystickController : MonoBehaviour
{
    public static GameObject FireJoystickObj { get; private set; }
    public static DynamicJoystick FireJoystick { get; private set; }

    private void Awake()
    {
        FireJoystickObj = gameObject;
        FireJoystick = FireJoystickObj.GetComponent<DynamicJoystick>();
        SetJoystickVisibility();
    }

    public static void SetJoystickVisibility()
    {
        FireJoystickObj.SetActive(!FireJoystickObj.activeInHierarchy);
    }
}
