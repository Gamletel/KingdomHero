using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    private DynamicJoystick _joystick;
    [SerializeField] private float _speed;
    private Animator _animator;
    private Rigidbody _rb;
    private bool _isMoving;

    void Start()
    {
        _joystick = GlobalVars.moveJoystick.GetComponent<DynamicJoystick>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        LoseController.playerLose += DisablePlayer;
        WinController.playerWin += DisablePlayer;
    }
    private void OnDisable()
    {
        LoseController.playerLose -= DisablePlayer;
        WinController.playerWin -= DisablePlayer;
    }

    public void DisablePlayer()
    {
        GlobalVars.moveJoystick.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!GlobalVars.IsPlayerMoving())
            return;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _isMoving = true;
        }
        else
        {
            _animator.SetFloat("speed", 0);
            _isMoving = false;
        }

        if (!_isMoving)
            return;

        _rb.velocity = new Vector3(_joystick.Horizontal * _speed, _rb.velocity.y, _joystick.Vertical * _speed);
        transform.rotation = Quaternion.LookRotation(_rb.velocity.normalized);
        _animator.SetFloat("speed", _rb.velocity.magnitude);
    }
}
