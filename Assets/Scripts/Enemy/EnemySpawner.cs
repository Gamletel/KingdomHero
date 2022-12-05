using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void NewWave(int wave);
    public static event NewWave newWave;

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

    [Header("Wave Settings")]
    [SerializeField] private int _maxWave = 3;
    [SerializeField] private int _warriorsToSpawn;
    [SerializeField] private int _archersToSpawn;
    [SerializeField] private int _bigsToSpawn;
    private int _curWave = 0;

    private int _curWarriorAmount;
    private int _curArcherAmount;
    private int _curBigAmount;
    private int _enemiesAmount;
    private int _curEnemies = 0;

    private void Awake()
    {
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;

        _enemyWarriorPool = new ObjectPool<EnemyWarrior>(_enemyWarrior, _warriorPoolCount, _curSpawnPoint);
        _enemyArcherPool = new ObjectPool<EnemyArcher>(_enemyArcher, _archerPoolCount, _curSpawnPoint);
        _enemyBigPool = new ObjectPool<EnemyBig>(_enemyBig, _bigPoolCount, _curSpawnPoint);
    }

    private void Start()
    {
        StartWaveController.waveStarted += StartSpawner;
        KilledEnemiesCounter.allEnemiesKilled += StartSpawner;
    }

    private void OnDestroy()
    {
        StartWaveController.waveStarted -= StartSpawner;
        KilledEnemiesCounter.allEnemiesKilled -= StartSpawner;
    }

    private void StopSpawner()
    {
        StopCoroutine(Spawner());
    }

    private void StartSpawner()
    {
        newWave?.Invoke(_curWave + 1);
        StartCoroutine(WaveDelay());
    }

    private IEnumerator WaveDelay()
    {
        if (_curWave < _maxWave)
        {
            _warriorsToSpawn += 10;
            _archersToSpawn += 15;
            _bigsToSpawn += 3;


            _curWarriorAmount = 0;
            _curArcherAmount = 0;
            _curBigAmount = 0;

            _curEnemies = 0;

            _enemiesAmount = _warriorsToSpawn + _archersToSpawn + _bigsToSpawn;
            KilledEnemiesCounter.SetEnemiesAmount(_enemiesAmount);

            yield return new WaitForSeconds(5f);
            StartCoroutine(Spawner());
            _curWave++;
        }
        else
        {
            StopCoroutine(WaveDelay());
            WinController.OnPlayerWin();
        }
    }

    private IEnumerator Spawner()
    {
        while (_enemiesAmount + 1 >= _curEnemies)
        {
            yield return new WaitForSeconds(_spawnDelay);
            Spawn();
        }
        Debug.Log($"Все враги заспавнены! Следующая волна: #{_curWave}");
    }

    private void Spawn()
    {
        int i = Random.Range(0, 100);
        _curSpawnPoint.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position + new Vector3(Random.Range(-2f, 2f), 0, 0);

        if (i <= 30)
        {
            if (_curWarriorAmount != _warriorsToSpawn)
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
            if (_curArcherAmount != _archersToSpawn)
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
            if (_curBigAmount != _bigsToSpawn)
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
