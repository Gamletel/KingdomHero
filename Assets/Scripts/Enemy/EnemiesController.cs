using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    private EnemyWarrior[] _enemiesPool;
    [SerializeField] private int _attackDistance;
    private bool _isAttacking;

    void Start()
    {
        _enemiesPool = EnemySpawner.warriorArray;
        Debug.Log(_enemiesPool.Length);
    }
    private void Update()
    {
        foreach (var enemy in _enemiesPool)
        {
            if (Vector3.Distance(enemy.transform.position, Wall.wallTransform.position) >= _attackDistance)
            {
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(enemy.transform.position.x, transform.position.y, -12), 4 * Time.deltaTime);
            }
            else
            {
                StartCoroutine(Attacking(enemy.GetComponent<Animator>()));
            }
        }
    }

    private IEnumerator Attacking(Animator animator, bool isAttacking = false)
    {
        if (!isAttacking)
        {
            animator.SetBool("isMoving", false);
            Debug.Log("1");
            while (true)
            {
                yield return new WaitForSeconds(3);
                animator.SetTrigger("Attack");
            }
        }
        isAttacking = true;
    }
}
