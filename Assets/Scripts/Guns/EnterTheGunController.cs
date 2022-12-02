using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnterTheGunController : MonoBehaviour
{
    public delegate void EnterTheGun(bool entered);
    public static event EnterTheGun enterTheGun;

    public bool ThisZone { get; private set; }
    public static bool enteredTheGun;
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
            EnterTheGunBtn.enterTheGunBtn.SetActive(true);
            ThisZone = !ThisZone;
            _gunCameraController.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterTheGunBtn.enterTheGunBtn.SetActive(false);
            ThisZone = !ThisZone;
            _gunCameraController.enabled = false;
        }
    }

    public static void OnEnterTheGun(bool entered)
    {
        enterTheGun?.Invoke(entered);
    }
}
