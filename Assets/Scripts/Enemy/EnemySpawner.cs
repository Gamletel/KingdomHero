using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform _curSpawnPoint;
    private float _spawnDelay = .5f;

    [Header("Enemies")]
    [SerializeField] private EnemyWarrior _enemyWarrior;
    [SerializeField] private EnemyArcher _enemyArcher;
    [SerializeField] private EnemyBig _enemyBig;

    [Header("Pool Settings")]
    [SerializeField] private int _warriorPoolCount;
    [SerializeField] private int _archerPoolCount;
    [SerializeField] private int _bigPoolCount;
    private ObjectPool<EnemyWarrior> _enemyWarriorPool;
    private ObjectPool<EnemyArcher> _enemyArcherPool;
    private ObjectPool<EnemyBig> _enemyBigPool;

    [HideInInspector] public static EnemyWarrior[] warriorArray;
    [HideInInspector] public static GameObject[] archersArray;
    [HideInInspector] public static GameObject[] bigEnemyArray;

    private int _curWarriorAmount;
    private int _curArcherAmount;
    private int _curBigAmount;
    private int _enemiesAmount;
    private int _curEnemies = 0;

    private void Awake()
    {
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;

        _enemyWarriorPool = new ObjectPool<EnemyWarrior>(_enemyWarrior, _warriorPoolCount, _curSpawnPoint);
        warriorArray = FindObjectsOfType<EnemyWarrior>();
        _enemyArcherPool = new ObjectPool<EnemyArcher>(_enemyArcher, _archerPoolCount, _curSpawnPoint);
        _enemyBigPool = new ObjectPool<EnemyBig>(_enemyBig, _bigPoolCount, _curSpawnPoint);
    }

    private void Start()
    {
        StartWaveController.waveStarted += StartSpawn;
    }

    private void OnDestroy()
    {
        StartWaveController.waveStarted -= StartSpawn;
    }

    private void StopSpawner()
    {
        StopCoroutine(Spawner());
    }

    private void StartSpawn()
    {

        _enemiesAmount = _warriorPoolCount + _archerPoolCount + _bigPoolCount;

        StartCoroutine(Spawner());
        KilledEnemiesCounter.SetEnemiesAmount(_enemiesAmount);
    }

    private IEnumerator Spawner()
    {
        while (_enemiesAmount + 1 >= _curEnemies)
        {
            yield return new WaitForSeconds(_spawnDelay);
            Spawn();
        }
        StopSpawner();
        Debug.Log("Все враги заспавнены!");
    }

    private void Spawn()
    {
        int i = Random.Range(0, 100);
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position + new Vector3(Random.Range(-2f, 2f), 0, 0);

        if (i <= 30)
        {
            if (_curWarriorAmount != _warriorPoolCount)
            {
                _enemyWarriorPool.GetFreeElement();
                _curWarriorAmount++;
                _spawnDelay = .3f;
                _curEnemies++;
            }
            else
            {
                _spawnDelay = 0f;
            }
            return;
        }

        if (i > 30 && i < 90)
        {
            if (_curArcherAmount != _archerPoolCount)
            {
                _enemyArcherPool.GetFreeElement();
                _curArcherAmount++;
                _spawnDelay = .2f;
                _curEnemies++;
            }
            else
            {
                _spawnDelay = 0f;
            }
            return;
        }

        if (i >= 90)
        {
            if (_curBigAmount != _bigPoolCount)
            {
                _enemyBigPool.GetFreeElement();
                _curBigAmount++;
                _spawnDelay = .5f;
                _curEnemies++;
            }
            else
            {
                _spawnDelay = 0f;
            }
            return;
        }
    }
}
