using UnityEngine;


public class GunCameraController : MonoBehaviour
{
    public delegate void ActivateCamera(bool isActive);
    public static event ActivateCamera activateCamera;
    [field: SerializeField] public GameObject gunCamera { get; private set; }
    [Header("Перемещение камеры по оси Y")]
    [SerializeField] private bool _useYcoordinate = false;

    private Transform lookAtPoint;
    private Vector2 _startPos;
    private EnterTheGunController _enterTheGunController;

    private bool _canAiming;

    private void Awake()
    {
        gunCamera.SetActive(false);
        _enterTheGunController = GetComponent<EnterTheGunController>();
    }

    private void Start()
    {
        lookAtPoint = GlobalVars.lookAtPoint;
        _startPos = lookAtPoint.transform.position;
        activateCamera += SetGunCamera;
    }

    private void FixedUpdate()
    {
        if (_enterTheGunController.ThisZone && EnterTheGunController.enteredTheGun)
        {

            switch (_useYcoordinate)
            {
                case true:
                    if (Input.touchCount != 0)
                    {
                        Touch touch = Input.GetTouch(0);
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;
                        if (touch.phase != TouchPhase.Canceled)
                        {
                            if (Physics.Raycast(ray, out hit, 100))
                                lookAtPoint.position = hit.point;
                        }
                        transform.LookAt(lookAtPoint);
                    }
                        
                    break;

                case false:
                    lookAtPoint.position = new Vector2(lookAtPoint.position.x + FireJoystickController.FireJoystick.Horizontal / 5,
                lookAtPoint.position.y + FireJoystickController.FireJoystick.Vertical / 5);
                    transform.LookAt(new Vector3(lookAtPoint.position.x, transform.position.y, lookAtPoint.position.z));
                    break;
            }
        }
    }

    public static void SetCameraActive(bool isActive)
    {
        activateCamera?.Invoke(isActive);
    }

    private void SetGunCamera(bool isActive)
    {
        if (_enterTheGunController.ThisZone)
            gunCamera.SetActive(isActive);
    }
}
