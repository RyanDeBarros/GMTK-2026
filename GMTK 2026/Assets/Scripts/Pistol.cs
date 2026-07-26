using UnityEngine;
using UnityEngine.Assertions;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private bool flipDirection;

    private PlayerController controller;

    private float minAimAngle;
    private float maxAimAngle;
    private float aimAngle;
    private bool aimCCW;
    private float aimSpeed;

    private void Awake()
    {
        Assert.IsNotNull(bulletSpawn);
        Assert.IsNotNull(bulletPrefab);
        Assert.IsNotNull(crosshair);

        controller = GetComponent<PlayerController>();
        Assert.IsNotNull(controller);
    }

    private void Update()
    {
        if (controller.IsAiming())
        {
            crosshair.SetActive(true);

            float deltaAngle = Time.deltaTime * aimSpeed;
            if (aimCCW)
            {
                aimAngle += deltaAngle;
                if (aimAngle > maxAimAngle)
                {
                    aimAngle = maxAimAngle;
                    aimCCW = !aimCCW;
                }
            }
            else
            {
                aimAngle -= deltaAngle;
                if (aimAngle < minAimAngle)
                {
                    aimAngle = minAimAngle;
                    aimCCW = !aimCCW;
                }
            }

            crosshair.transform.localEulerAngles = new(0f, 0f, aimAngle);
        }
        else
            crosshair.SetActive(false);
    }

    public void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawn.position, crosshair.transform.rotation).GetComponent<Bullet>();
        bullet.owner = controller;
        bullet.direction = crosshair.transform.right * (flipDirection ? -1f : 1f);
    }

    public void SetInitialAngle(float initialAngle, bool ccw, float aimSpeed, float minAimAngle, float maxAimAngle)
    {
        aimAngle = initialAngle;
        aimCCW = ccw;
        this.aimSpeed = aimSpeed;
        this.minAimAngle = minAimAngle;
        this.maxAimAngle = maxAimAngle;
    }
}
