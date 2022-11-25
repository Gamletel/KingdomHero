using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnterTheGunController), typeof(GunCameraController), typeof(TrajectoryRenderer))]
public class Balista : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] private Bullet bullet;
    [SerializeField] private int poolCount;
    [SerializeField] private bool autoExpand;
    [SerializeField] private ObjectPool<Bullet> firstPool;
    [SerializeField] private ObjectPool<Bullet> secondPool;
    [SerializeField] private ObjectPool<Bullet> thirdPool;

    [Header("Arrows")]
    [SerializeField] private GameObject firstLoadedBullet;
    private Transform firstBulletSpawnPoint;

    [Space(10)]
    [SerializeField] private GameObject secondLoadedBullet;
    private Transform secondBulletSpawnPoint;

    [Space(10)]
    [SerializeField] private GameObject thirdLoadedBullet;
    private Transform thirdBulletSpawnPoint;

    [Space(10)]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadingTime;
    private float _bulletMass;
    private bool _canShoot = true;
    private bool _isAiming;
    private float _shootDelay = .5f;
    private ReloadingImgController _reloadingImgController;

    /*Other*/
    private EnterTheGunController _enterTheGunController;
    [SerializeField] private TrajectoryRenderer _trajectory3;

    private void Awake()
    {
        firstBulletSpawnPoint = firstLoadedBullet.transform;
        firstPool = new ObjectPool<Bullet>(bullet, poolCount, firstBulletSpawnPoint);

        secondBulletSpawnPoint = secondLoadedBullet.transform;
        secondPool = new ObjectPool<Bullet>(bullet, poolCount, secondBulletSpawnPoint);

        thirdBulletSpawnPoint = thirdLoadedBullet.transform;
        thirdPool = new ObjectPool<Bullet>(bullet, poolCount, thirdBulletSpawnPoint);

        _enterTheGunController = GetComponent<EnterTheGunController>();
        _reloadingImgController = GetComponentInChildren<ReloadingImgController>();
    }

    private void Start()
    {
        _bulletMass = bullet.GetComponent<Rigidbody>().mass;
    }

    private void Update()
    {
        if (GlobalVars.IsPlayerMoving())
            return;

        if (Input.GetMouseButton(0) && _enterTheGunController.ThisZone)
        {
            Vector3 speed3 = (GlobalVars.lookAtPoint.position - thirdBulletSpawnPoint.position) * bulletSpeed;
            _trajectory3.ShowTrajectory(thirdBulletSpawnPoint.position, speed3, _bulletMass);
            _isAiming = true;
        }
        else
        {
            _trajectory3.HideTrajectory();
        }

        if (Input.GetMouseButtonUp(0) && _enterTheGunController.ThisZone && _canShoot && _isAiming)
            Fire();

    }

    private void Fire()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        Reload(reloadingTime);
        Vector3 pos1 = GlobalVars.lookAtPoint.position + Vector3.up * 1f;
        Vector3 pos2 = GlobalVars.lookAtPoint.position + Vector3.up * .5f;
        Vector3 pos3 = GlobalVars.lookAtPoint.position;
        var firstBullet = firstPool.GetFreeElement();
        var firstHeading = pos1 - transform.position;
        firstBullet.GetComponent<Rigidbody>().AddForce(firstHeading * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(_shootDelay);
        var secondBullet = secondPool.GetFreeElement();
        var secondHeading = pos2 - transform.position;
        secondBullet.GetComponent<Rigidbody>().AddForce(secondHeading * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(_shootDelay);
        var thirdBullet = thirdPool.GetFreeElement();
        var thirdHeading = pos3 - transform.position;
        thirdBullet.GetComponent<Rigidbody>().AddForce(thirdHeading * bulletSpeed, ForceMode.Impulse);
        _isAiming = false;
    }

    private void Reload(float reloadingTime)
    {
        _reloadingImgController.OnFired(reloadingTime);
        _canShoot = false;
        StartCoroutine(Reloading(reloadingTime));
    }

    private IEnumerator Reloading(float reloadingTime)
    {
        firstLoadedBullet.SetActive(false);
        yield return new WaitForSeconds(_shootDelay);
        secondLoadedBullet.SetActive(false);
        yield return new WaitForSeconds(_shootDelay);
        thirdLoadedBullet.SetActive(false);
        yield return new WaitForSeconds(_shootDelay);
        firstLoadedBullet.SetActive(true);
        yield return new WaitForSeconds(_shootDelay);
        secondLoadedBullet.SetActive(true);
        yield return new WaitForSeconds(_shootDelay);
        thirdLoadedBullet.SetActive(true);
        _canShoot = true;
        StopCoroutine(Reloading(reloadingTime));
    }
}
