using UnityEngine;
using UnityEngine.Assertions;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private GameObject bulletPrefab;

    private float aimAngle = 0f;

    private void Awake()
    {
        Assert.IsNotNull(bulletSpawn);
        Assert.IsNotNull(bulletPrefab);
    }

    private void Update()
    {
        if (MatchManager.Instance.Phase == MatchPhase.Slomo)
        {
            // TODO update aimAngle and render crosshair
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(WorldAimDirection(), Vector3.up));
    }

    public Vector3 WorldAimDirection()
    {
        return gameObject.transform.localToWorldMatrix.MultiplyVector(new(Mathf.Cos(aimAngle), Mathf.Sin(aimAngle), 0f)).normalized;
    }
}
