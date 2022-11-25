using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Gun
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private int _poolCount;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _reloadingTime;
    [SerializeField] private GameObject _loadedBullet;

    private void Awake()
    {
        ApplyVars(_poolCount, _autoExpand, _bulletSpeed, _reloadingTime);
        bullet = _bullet;
        bulletSpawnPoint = _bulletSpawnPoint;
        pool = new ObjectPool<Bullet>(bullet, poolCount, bulletSpawnPoint);
        loadedBullet = _loadedBullet;
    }
}
