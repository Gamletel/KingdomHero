using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitController : MonoBehaviour
{
    public delegate void EnemyHit();
    public static event EnemyHit enemyHit;

    [SerializeField] private ParticleSystem _enemyHitParticle;
    private static ParticleSystem _particle;

    private void Awake()
    {
        _particle = _enemyHitParticle;
    }

    public static void OnEnemyHit(Vector3 hitPosition)
    {
        Instantiate(_particle, hitPosition, Quaternion.identity);
        _particle.Play();
    }
}
