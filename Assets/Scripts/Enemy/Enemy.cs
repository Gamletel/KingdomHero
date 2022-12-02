using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Material _deathMaterial;
    private Material _defaultMaterial;
    private SkinnedMeshRenderer _meshRenderer;

    /*EnemyStats*/
    [field: SerializeField] public float _speed { get; private set; }
    [SerializeField] private int maxHP;
    private int curHP;
    [field:SerializeField] public int attackRange { get; private set; }
    [SerializeField] private int damage;
    [SerializeField] private int attackSpeed;

    private Wall _wall;
    private Rigidbody _rb;
    private bool _canAttack = true;
    private bool _isAttacking;
    private Animator _animator;
    private Collider _col;

    //Animator
    int _isMovingHash;
    int _attackHash;
    int _getDamageHash;
    int _isDeadHash;

    public void ApplyVars()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _col = GetComponent<Collider>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    private void Awake()
    {
        ApplyVars();
        _isMovingHash = Animator.StringToHash("isMoving");
        _attackHash = Animator.StringToHash("Attack");
        _getDamageHash = Animator.StringToHash("getDamage");
        _isDeadHash = Animator.StringToHash("isDead");
    }

    private void Start()
    {
        curHP = maxHP;
        transform.LookAt(transform.position + Vector3.back);
        _animator.SetBool(_isMovingHash, true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isAttacking || curHP <= 0)
            return;
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, -12), _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wall component))
        {
            _wall = component;
            StartCoroutine(Attacking());
        }
    }

    private void OnEnable()
    {
        _rb.isKinematic = true;
        _animator.SetBool(_isMovingHash, true);
    }

    private void OnDisable()
    {
        curHP = maxHP;
        StopCoroutine(Attacking());
        _rb.velocity = Vector3.zero;
        //_rb.constraints = RigidbodyConstraints.FreezeRotation;

        _meshRenderer.material = _defaultMaterial;

        _isAttacking = false;
        _canAttack = true;
        _col.enabled = true;
    }

    private IEnumerator Attacking()
    {
        if (_canAttack)
        {
            _isAttacking = true;
            _animator.SetBool(_isMovingHash, false);

            while (_canAttack)
            {
                yield return new WaitForSeconds(attackSpeed);
                SetAttackTrigger();
            }
        }
    }

    private void SetAttackTrigger()
    {
        _animator.SetTrigger(_attackHash);
    }

    public void Attack()
    {
        _wall.GetDamage(damage);
    }

    public void GetDamage(int dmg, float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        _animator.SetTrigger(_getDamageHash);
        curHP -= dmg;
        if (curHP <= 0)
            StartCoroutine(Death(repulsiveForce, force, dmgPos));
    }

    private void Die(float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        _meshRenderer.material = _deathMaterial;
        _col.enabled = false;
        _canAttack = false;
        
        _animator.SetBool(_isMovingHash, false);
        _animator.SetBool(_isDeadHash, true);


        _rb.isKinematic = false;
        _rb.AddForce(force, ForceMode.Impulse);
        //_rb.AddRelativeTorque(-force/_rb.mass, ForceMode.Impulse);
    }

    private IEnumerator Death(float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        Die(repulsiveForce,force , dmgPos);
        KilledEnemiesCounter.OnEnemyKilled();

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
    }
}
