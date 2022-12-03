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
    private TrajectoryRenderer _trajectory1;
    private TrajectoryRenderer _trajectory2;
    [SerializeField] private TrajectoryRenderer _trajectory3;
    
    private GunCameraController _gunCameraController;

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
        _gunCameraController = GetComponent<GunCameraController>();
    }

    private void Start()
    {
        _bulletMass = bullet.GetComponent<Rigidbody>().mass;
    }

    private void Update()
    {
        if (!EnterTheGunController.enteredTheGun)
        {
            _trajectory3.HideTrajectory();
            return;
        }

        if (GlobalVars.IsPlayerMoving())
            return;

        if (Input.GetMouseButton(0) && _enterTheGunController.ThisZone && EnterTheGunController.enteredTheGun)
        {

            Vector3 speed3;
            switch (_gunCameraController.useSwipeForRotating)
            {
                case true:
                    speed3 = firstBulletSpawnPoint.transform.forward * bulletSpeed;
                    _trajectory3.ShowTrajectory(firstBulletSpawnPoint.position, speed3, _bulletMass);
                    _isAiming = true;
                    break;

                case false:
                    speed3 = (GlobalVars.lookAtPoint.position - transform.position) * bulletSpeed;
                    _trajectory3.ShowTrajectory(thirdBulletSpawnPoint.position, speed3, _bulletMass);
                    _isAiming = true;
                    break;
            }
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

        Vector3 pos1;
        Vector3 pos2;
        Vector3 pos3;

        switch (_gunCameraController.useSwipeForRotating)
        {
            case true:
                pos1 = firstLoadedBullet.transform.forward;
                pos2 = secondLoadedBullet.transform.forward;
                pos3 = thirdLoadedBullet.transform.forward;
                break;

            case false:
                pos1 = GlobalVars.lookAtPoint.position - transform.position;
                pos2 = GlobalVars.lookAtPoint.position - transform.position;
                pos3 = GlobalVars.lookAtPoint.position - transform.position;
                break;
        }
        var firstBullet = firstPool.GetFreeElement();
        firstBullet.GetComponent<Rigidbody>().AddForce(pos1 * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(_shootDelay);
        var secondBullet = secondPool.GetFreeElement();
        secondBullet.GetComponent<Rigidbody>().AddForce(pos2 * bulletSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(_shootDelay);
        var thirdBullet = thirdPool.GetFreeElement();
        thirdBullet.GetComponent<Rigidbody>().AddForce(pos3 * bulletSpeed, ForceMode.Impulse);
        
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
