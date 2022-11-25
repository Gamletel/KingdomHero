using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
public class Bullet : MonoBehaviour
{
    public delegate void IsHit();
    public static event IsHit isHit;

    [Header("Bullet Stats")]
    [SerializeField] private int _dmg;

    [Header("For Rotating Bullets")]
    [SerializeField] private bool _isRotating;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector3 _rotationVector;

    [Header("For AOE Bullets")]
    [SerializeField] private bool _isAOE;
    [SerializeField] private float _AOEradius;
    private SphereCollider _AOECollider;

    [Header("Other")]
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private bool _destroyOnHit;
    private Rigidbody _rb;
    private MeshCollider _col;
    private float _lifetime = 5f;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<MeshCollider>();
        _col.convex = true;
        if (_isAOE)
        {
            _AOECollider = gameObject.AddComponent<SphereCollider>();
            _AOECollider.radius = _AOEradius;
            _AOECollider.isTrigger = true;
            _AOECollider.enabled = false;
        }
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (_isRotating)
        {
            case true:
                if (_isGrounded)
                    return;
                transform.Rotate(_rotationVector, _rotationSpeed);
                break;

            case false:
                Vector3 dir = _rb.velocity;

                if (dir.sqrMagnitude > 0.1f)
                    transform.LookAt(dir);
                break;
        }

    }

    private void OnEnable()
    {
        this.StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isHit?.Invoke();
            Instantiate(_hitParticle, transform.position, Quaternion.identity);
            GetComponent<Collider>().enabled = false;
            Debug.Log("В землю! " + collision.gameObject.name);
            this._rb.constraints = RigidbodyConstraints.FreezeAll;
            _isGrounded = true;

            if (_isAOE)
                StartCoroutine(Explosion());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy component))
        {
            isHit?.Invoke();
            Debug.Log("Попал!");
            component.GetDamage(_dmg, _rb.mass, (other.transform.position - transform.position).normalized + Vector3.up * _rb.mass, transform.position);

            if (_destroyOnHit)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Включение и отключение коллайдера у AOE снарядов
    /// </summary>
    /// <returns></returns>
    private IEnumerator Explosion()
    {
        _col.enabled = false;
        _AOECollider.enabled = true;
        yield return new WaitForSeconds(.1f);
        _AOECollider.enabled = false;
    }

    private void OnDisable()
    {
        this.StopCoroutine(LifeTime());
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.constraints = RigidbodyConstraints.None;
        _isGrounded = false;
        GetComponent<Collider>().enabled = true;

        if (_isAOE)
            _AOECollider.enabled = false;
    }

    public IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifetime);

        this._rb.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
