using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        WinController.playerWin += PlayerWin;
        KilledEnemiesCounter.enemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        _animator.SetTrigger("enemyKilled");
    }

    private void OnDestroy()
    {
        WinController.playerWin -= PlayerWin;
        KilledEnemiesCounter.enemyKilled += OnEnemyKilled;
    }

    private void PlayerWin()
    {
        _animator.SetTrigger("Win");
    }
}
