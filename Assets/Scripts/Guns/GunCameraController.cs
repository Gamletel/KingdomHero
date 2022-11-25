using UnityEngine;
using Cinemachine;
using System.Collections;

public class GunCameraController : MonoBehaviour
{
    [field: SerializeField] public GameObject gunCamera { get; private set; }
    [Header("Перемещение камеры по оси Y")]
    [SerializeField] private bool _useYcoordinate = false;

    private Transform lookAtPoint;
    private Vector2 _startPos;
    private EnterTheGunController _enterTheGunController;

    private void Awake()
    {
        gunCamera.SetActive(false);
        _enterTheGunController = GetComponent<EnterTheGunController>();
    }

    private void Start()
    {
        Bullet.isHit += DisableCamera;
        lookAtPoint = GlobalVars.lookAtPoint;
        _startPos = lookAtPoint.transform.position;
    }

    private void Update()
    {
        if (OnThisGun() && _enterTheGunController.ThisZone)
        {
            GlobalVars.moveJoystick.SetActive(false);
            lookAtPoint.position = new Vector2(lookAtPoint.position.x + FireJoystickController.FireJoystick.Horizontal / 15,
                lookAtPoint.position.y + FireJoystickController.FireJoystick.Vertical / 15);

            gunCamera.SetActive(true);
            switch (_useYcoordinate)
            {
                case true:
                    transform.LookAt(lookAtPoint);
                    break;
                case false:
                    transform.LookAt(new Vector3(lookAtPoint.position.x, transform.position.y, lookAtPoint.position.z));
                    break;
            }
        }
    }

    private bool OnThisGun()
    {
        return FireJoystickController.FireJoystick.Direction != Vector2.zero;
    }

    public void DisableCamera()
    {
        if (OnThisGun())
            return;
        lookAtPoint.position = _startPos;
        gunCamera.SetActive(false);
        GlobalVars.moveJoystick.SetActive(true);
    }
}
