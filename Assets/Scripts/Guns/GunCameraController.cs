using System;
using UnityEngine;


public class GunCameraController : MonoBehaviour
{
    public delegate void ActivateCamera(bool isActive);
    public static event ActivateCamera activateCamera;
    [field: SerializeField] public GameObject gunCamera { get; private set; }
    //[Header("Перемещение камеры по оси Y")]
    [field: SerializeField] public bool useSwipeForRotating { get; private set; }
    [field: SerializeField] public bool isRotateY { get; private set; }

    private Transform lookAtPoint;
    private EnterTheGunController _enterTheGunController;

    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed;
    private const float MIN_X= -40, MAX_X = 40;
    private const float MIN_Y = -75, MAX_Y = 75;
    private float _curX, _curY;
    private Quaternion rot;
    private Touch _touch;

    private Gun _gun;

    [SerializeField] private GameObject[] _bulletSpawnPoints;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        gunCamera.SetActive(false);
        _enterTheGunController = GetComponent<EnterTheGunController>();
    }

    private void Start()
    {
        lookAtPoint = GlobalVars.lookAtPoint;
        activateCamera += SetGunCamera;
    }

    private void Update()
    {
        if (_enterTheGunController.ThisZone && EnterTheGunController.enteredTheGun)
        {

            switch (useSwipeForRotating)
            {
                case true:
                    GunRotate();
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

    private void GunRotate()
    {
        if (Input.touchCount != 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Moved)
            {
                //_curX = (_curX > 180) ? _curX - 360 : _curX;
                //_curY = (_curY > 180) ? _curY - 360 : _curY;

                //_curX = Mathf.Clamp(_curX, MIN_Z, MIN_Z);
                //_curY = Mathf.Clamp(_curY, MIN_Y, MIN_Y);

                switch (isRotateY)
                {
                    case true:
                        CalculateRotate(transform);
                        transform.rotation = rot;
                        _gun.bulletSpawnPoint.transform.rotation = rot;
                        return;

                    case false:
                        if (_bulletSpawnPoints.Length != 0)
                        {
                            foreach (var bulletSpawnPoint in _bulletSpawnPoints)
                            {
                                CalculateRotate(bulletSpawnPoint.transform);
                                bulletSpawnPoint.transform.localRotation = Quaternion.Euler(_curX, 0, 0);
                                transform.rotation = Quaternion.Euler(0, _curY, 0);
                            }
                        }
                        else
                        {
                            CalculateRotate(_gun.bulletSpawnPoint.transform);
                            _gun.bulletSpawnPoint.transform.rotation = rot;
                            transform.rotation = Quaternion.Euler(0, _curY, 0);
                        }                      
                        return;
                }

            }
        }
    }

    /// <summary>
    /// Rotating Gun / Bullet spawn point
    /// </summary>
    /// <param name="transform">Object Transform</param>
    private void CalculateRotate(Transform transform)
    {
        _curX = transform.eulerAngles.x;
        _curY = transform.eulerAngles.y;

        _curX = (_curX > 180) ? _curX - 360 : _curX;
        _curY = (_curY > 180) ? _curY - 360 : _curY;

        _curX -= _touch.deltaPosition.y * Time.deltaTime * _rotationSpeed;
        _curY += _touch.deltaPosition.x * Time.deltaTime * _rotationSpeed;

        if (_curX > MAX_X)
            _curX = _curX - 2;
        if (_curX < MIN_X)
            _curX = _curX + 2;

        if (_curY > MAX_Y)
            _curY = _curY - 2;
        if (_curY < MIN_Y)
            _curY = _curY + 2;

        rot = Quaternion.Euler(_curX, _curY, 0);
    }
}
