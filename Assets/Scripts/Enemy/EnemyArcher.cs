using UnityEngine;

public class EnemyArcher : Enemy
{
    [SerializeField] private int _maxHP;
    [SerializeField] private int _attackRange;
    [SerializeField] private int _attackSpeed;
    [SerializeField] private int _damage;

    private void Awake()
    {
        ApplyVars(_maxHP, _attackRange, _attackSpeed, _damage);
    }
}
