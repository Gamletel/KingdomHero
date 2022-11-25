using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnZone : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private Transform _curSpawnPoint;

    [Header("Enemies")]
    [SerializeField] private EnemyWarrior _enemyWarrior;
    [SerializeField] private EnemyArcher _enemyArcher;

    [Header("Pool Settings")]
    [SerializeField] private int _warriorPoolCount;
    [SerializeField] private int _archerPoolCount;
    private ObjectPool<EnemyWarrior> _enemyWarriorPool;
    private ObjectPool<EnemyArcher> _enemyArcherPool;

    private int _curWarriorAmount;
    private int _curArcherAmount;
    private int _enemiesAmount;
    private int _curEnemies = 0;

    private void Awake()
    {
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        _enemyWarriorPool = new ObjectPool<EnemyWarrior>(_enemyWarrior, _warriorPoolCount, _curSpawnPoint);
        _enemyArcherPool = new ObjectPool<EnemyArcher>(_enemyArcher, _archerPoolCount, _curSpawnPoint);
        _enemiesAmount = _warriorPoolCount + _archerPoolCount;
        StartCoroutine(EnemySpawner());
        KilledEnemiesCounter.SetEnemiesAmount(_enemiesAmount);
    }

    private IEnumerator EnemySpawner()
    {
        while (_enemiesAmount + 1 >= _curEnemies)
        {
            yield return new WaitForSeconds(_spawnDelay);
            Spawn();
        }
        Debug.Log("Все враги заспавнены!");
    }

    private void Spawn()
    {
        int i = Random.Range(0, 2);
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position + new Vector3(Random.Range(-1f, 1f), 0, 0);

        switch (i)
        {
            case 0:
                if (_curWarriorAmount != _warriorPoolCount)
                {
                    _enemyWarriorPool.GetFreeElement();
                    _curWarriorAmount++;
                }
                else
                    i = 1;
                
                return;

            case 1:
                if (_curArcherAmount != _archerPoolCount)
                {
                    _enemyArcherPool.GetFreeElement();
                    _curArcherAmount++;
                }
                else
                    i = 0;
                
                return;
        }
        _curEnemies++;
    }
}
