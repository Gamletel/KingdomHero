using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnterTheGunController), typeof(GunCameraController), typeof(TrajectoryRenderer))]
public abstract class Gun : MonoBehaviour
{
    /*ObjectPool*/
    protected Bullet bullet;
    protected int poolCount;
    protected bool autoExpand;
    protected ObjectPool<Bullet> pool;

    /*ShootSettings*/
    protected GameObject loadedBullet;
    protected Transform bulletSpawnPoint;
    protected float bulletSpeed;
    protected float reloadingTime;
    private float _bulletMass;
    private bool _canShoot = true;
    private bool _isAiming;
    private ReloadingImgController _reloadingImgController;

    /*Other*/
    protected TrajectoryRenderer _trajectory;
    private EnterTheGunController _enterTheGunController;
    private GunCameraController _gunCameraController;

    protected void ApplyVars(int poolCount, bool autoExpand,
        float bulletSpeed, float reloadingTime)
    {
        this.poolCount = poolCount;
        this.autoExpand = autoExpand;
        this.bulletSpeed = bulletSpeed;
        this.reloadingTime = reloadingTime;
        _enterTheGunController = GetComponent<EnterTheGunController>();
        _gunCameraController = GetComponent<GunCameraController>();
        _trajectory = GetComponent<TrajectoryRenderer>();
        _reloadingImgController = GetComponentInChildren<ReloadingImgController>();
    }

    private void Start()
    {
        _bulletMass = bullet.GetComponent<Rigidbody>().mass;
        EnterTheGunController.enterTheGun += CanShoot;
    }

    private void OnDestroy()
    {
        EnterTheGunController.enterTheGun -= CanShoot;
    }

    private void CanShoot(bool entered)
    {
        _canShoot = entered;
    }

    private void Update()
    {
        if (!EnterTheGunController.enteredTheGun)
        {
            _trajectory.HideTrajectory();
            return;
        }   
        if (GlobalVars.IsPlayerMoving())
            return;
        if (Input.GetMouseButton(0) && _enterTheGunController.ThisZone && EnterTheGunController.enteredTheGun)
        {
            Vector3 speed = (GlobalVars.lookAtPoint.position - transform.position) * bulletSpeed;
            _trajectory.ShowTrajectory(bulletSpawnPoint.position, speed, _bulletMass);
            _isAiming = true;
        }
        else
        {
            _trajectory.HideTrajectory();
        }
        if (Input.GetMouseButtonUp(0) && _enterTheGunController.ThisZone && _canShoot && _isAiming)
            Fire();

    }

    protected virtual void Fire()
    {
        var bullet = pool.GetFreeElement();
        var heading = GlobalVars.lookAtPoint.position - transform.position;
        bullet.GetComponent<Rigidbody>().AddForce(heading * bulletSpeed, ForceMode.Impulse);
        Reload(reloadingTime);
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
        loadedBullet.SetActive(false);
        yield return new WaitForSeconds(reloadingTime);
        _canShoot = true;
        StopCoroutine(Reloading(reloadingTime));
        loadedBullet.SetActive(true);
    }
}
