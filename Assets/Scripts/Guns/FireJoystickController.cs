using UnityEngine;

public class FireJoystickController : MonoBehaviour
{
    public static GameObject FireJoystickObj { get; private set; }
    public static FixedJoystick FireJoystick { get; private set; }

    private void Awake()
    {
        FireJoystickObj = gameObject;
        FireJoystick = FireJoystickObj.GetComponent<FixedJoystick>();
        SetJoystickVisibility();
    }

    public static void SetJoystickVisibility()
    {
        FireJoystickObj.SetActive(!FireJoystickObj.activeInHierarchy);
    }
}
