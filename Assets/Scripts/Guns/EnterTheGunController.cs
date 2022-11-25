using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnterTheGunController : MonoBehaviour
{
    public bool ThisZone { get; private set; }
    private GunCameraController _gunCameraController;

    private void Awake()
    {
        _gunCameraController = GetComponent<GunCameraController>();
        _gunCameraController.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FireJoystickController.SetJoystickVisibility();
            ThisZone = !ThisZone;
            _gunCameraController.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FireJoystickController.SetJoystickVisibility();
            ThisZone = !ThisZone;
            _gunCameraController.enabled = false;
        }
    }
}
