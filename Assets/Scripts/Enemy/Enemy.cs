using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    /*EnemyStats*/
    private int maxHP;
    private int curHP;
    private int attackRange;
    private int damage;
    private int attackSpeed;

    private Wall _wall;
    private Rigidbody _rb;
    private bool _canAttack = true;
    private bool _isAttacking;
    private Animator _animator;
    private Collider _col;

    public void ApplyVars(int maxHP, int attackRange, int attackSpeed, int damage)
    {
        this.maxHP = maxHP;
        this.attackRange = attackRange;
        this.attackSpeed = attackSpeed;
        this.damage = damage;
        this._rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _col = GetComponent<Collider>();
    }

    private void Start()
    {
        curHP = maxHP;
        transform.LookAt(transform.position + Vector3.back);
        _animator.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        if (_isAttacking)
            return;
        if (_canAttack)
            Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wall component))
        {
            _wall = component;
            StartCoroutine(Attacking());
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Attacking());
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.constraints = RigidbodyConstraints.None;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        _isAttacking = false;
        _canAttack = true;
        _col.enabled = true;
    }

    private void Move()
    {
        _rb.AddForce(Vector3.back, ForceMode.Impulse);
    }

    private IEnumerator Attacking()
    {
        if (_canAttack)
        {
            _isAttacking = true;
            _animator.SetBool("isMoving", false);

            while (_canAttack)
            {
                yield return new WaitForSeconds(attackSpeed);
                Attack();
            }
        }
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        _wall.GetDamage(damage);
    }

    public void GetDamage(int dmg, float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        _animator.SetTrigger("getDamage");
        curHP -= dmg;

        if (curHP <= 0)
            StartCoroutine(Death(repulsiveForce, force, dmgPos));
    }

    private void Die(float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        _col.enabled = false;
        _canAttack = false;
        
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isDead", true);

        _rb.constraints = RigidbodyConstraints.None;
        _rb.AddForce(force);
        _rb.AddRelativeTorque(force);
        Debug.LogWarning(force);
    }

    private IEnumerator Death(float repulsiveForce, Vector3 force, Vector3 dmgPos)
    {
        Die(repulsiveForce,force , dmgPos);
        KilledEnemiesCounter.OnEnemyKilled();

        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
    }
}
