using UnityEngine;
using UnityEngine.Assertions;

public class Birb : MonoBehaviour
{
    [SerializeField] private float offscreenPositionY = 20f;
    [SerializeField] private float perchPositionY = 1f;

    [SerializeField] private float perchDurationMin = 2f;
    [SerializeField] private float perchDurationMax = 4f;

    [SerializeField] private float slomoSlowDown = 0.5f;
    [SerializeField] private float flightSpeed = 20f;

    [SerializeField] private float offsetScreenWaitMin = 3f;
    [SerializeField] private float offsetScreenWaitMax = 20f;
    [SerializeField] private float offsetScreenWaitMedian = 12f;

    private enum Phase
    {
        Offscreen,
        FlyingDown,
        Perching,
        FlyingOff
    }

    Phase phase = Phase.Offscreen;
    float waitTime = 0f;

    [SerializeField] private AudioClip getHitSFX;
    private AudioSource audioSource;
    private bool alreadyHit = false;

    private void Awake()
    {
        Assert.IsNotNull(getHitSFX);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        transform.position = new(transform.position.x, offscreenPositionY, transform.position.z);
        GenerateOffscreenWait();
    }

    private void Update()
    {
        switch (phase)
        {
            case Phase.Offscreen:
                if (waitTime > 0f)
                    waitTime -= DeltaTime();
                if (waitTime <= 0f && (MatchManager.Instance.Phase == MatchPhase.Countdown || MatchManager.Instance.Phase == MatchPhase.ChooseAction))
                {
                    phase = Phase.FlyingDown;
                    alreadyHit = false;
                }

                break;

            case Phase.FlyingDown:
                {
                    Vector3 pos = transform.position;
                    pos.y = Mathf.Clamp(pos.y - DeltaTime() * flightSpeed, perchPositionY, offscreenPositionY);
                    transform.position = pos;

                    if (pos.y == perchPositionY)
                    {
                        phase = Phase.Perching;
                        waitTime = Mathf.Lerp(perchDurationMin, perchDurationMax, Random.value);
                    }
                }

                break;

            case Phase.Perching:
                waitTime -= DeltaTime();
                if (waitTime <= 0f)
                    phase = Phase.FlyingOff;

                break;

            case Phase.FlyingOff:
                {
                    Vector3 pos = transform.position;
                    pos.y = Mathf.Clamp(pos.y + DeltaTime() * flightSpeed, perchPositionY, offscreenPositionY);
                    transform.position = pos;

                    if (pos.y == offscreenPositionY)
                    {
                        phase = Phase.Offscreen;
                        GenerateOffscreenWait();
                    }
                }

                break;
        }
    }

    private float DeltaTime()
    {
        float dt = Time.deltaTime;
        if (MatchManager.Instance.Phase == MatchPhase.Slomo)
            dt *= slomoSlowDown;
        return dt;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponentInParent<Bullet>();
        if (bullet == null)
            return;

        bullet.owner.GetHit();
        bullet.Despawn();

        if (!alreadyHit)
        {
            alreadyHit = true;
            phase = Phase.FlyingOff;
            audioSource.clip = getHitSFX;
            audioSource.Play();
        }
    }

    private void GenerateOffscreenWait()
    {
        if (Random.value < 0.5f)
            waitTime = Mathf.Lerp(offsetScreenWaitMin, offsetScreenWaitMedian, Random.value);
        else
            waitTime = Mathf.Lerp(offsetScreenWaitMedian, offsetScreenWaitMax, Random.value);
    }
}
