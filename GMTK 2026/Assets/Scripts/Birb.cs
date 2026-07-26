using UnityEngine;
using UnityEngine.Assertions;

public class Birb : MonoBehaviour
{
    [SerializeField] private float offscreenPositionX = 10f;
    [SerializeField] private float offscreenPositionY = 20f;
    [SerializeField] private float perchPositionX = 0f;
    [SerializeField] private float perchPositionY = 1f;

    [SerializeField] private float perchDurationMin = 2f;
    [SerializeField] private float perchDurationMax = 4f;

    [SerializeField] private float slomoSlowDown = 0.5f;
    [SerializeField] private float flightSpeed = 16f;

    [SerializeField] private float offsetScreenWaitMin = 3f;
    [SerializeField] private float offsetScreenWaitMax = 20f;
    [SerializeField] private float offsetScreenWaitMedian = 12f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] flyingFrames;
    [SerializeField] private float idleAnimationRate = 2f;
    [SerializeField] private float flyingAnimationRate = 5f;
    
    private float animationDebt = 0f;
    private int frameIndex = 0;
    private Sprite[] frames;
    private float animationRate;

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
        Assert.IsNotNull(spriteRenderer);
        Assert.IsTrue(idleFrames.Length > 0);
        Assert.IsTrue(flyingFrames.Length > 0);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        frames = idleFrames;
        animationRate = idleAnimationRate;
    }

    private void Start()
    {
        transform.position = new(offscreenPositionX, offscreenPositionY, transform.position.z);
        GenerateOffscreenWait();
    }

    private void Update()
    {
        animationDebt += Time.deltaTime * animationRate;
        while (animationDebt > 1f)
        {
            --animationDebt;
            ++frameIndex;
            frameIndex %= frames.Length;
            spriteRenderer.sprite = frames[frameIndex];
        }

        switch (phase)
        {
            case Phase.Offscreen:
                if (waitTime > 0f)
                    waitTime -= DeltaTime();
                if (waitTime <= 0f && (MatchManager.Instance.Phase == MatchPhase.Countdown || MatchManager.Instance.Phase == MatchPhase.ChooseAction))
                    SetPhase(Phase.FlyingDown);

                break;

            case Phase.FlyingDown:
                {
                    Vector3 pos = transform.position;
                    Vector2 dir = new(perchPositionX - pos.x, perchPositionY - pos.y);
                    dir = DeltaTime() * flightSpeed * dir.normalized;
                    pos.x = Mathf.Clamp(pos.x + dir.x, perchPositionX, offscreenPositionX);
                    pos.y = Mathf.Clamp(pos.y + dir.y, perchPositionY, offscreenPositionY);

                    if (pos.y == perchPositionY)
                    {
                        pos.x = perchPositionX;
                        SetPhase(Phase.Perching);
                        waitTime = Mathf.Lerp(perchDurationMin, perchDurationMax, Random.value);
                    }

                    transform.position = pos;
                }

                break;

            case Phase.Perching:
                waitTime -= DeltaTime();
                if (waitTime <= 0f)
                    SetPhase(Phase.FlyingOff);

                break;

            case Phase.FlyingOff:
                {
                    Vector3 pos = transform.position;
                    Vector2 dir = new(offscreenPositionX - pos.x, offscreenPositionY - pos.y);
                    dir = DeltaTime() * flightSpeed * dir.normalized;
                    pos.x = Mathf.Clamp(pos.x + dir.x, perchPositionX, offscreenPositionX);
                    pos.y = Mathf.Clamp(pos.y + dir.y, perchPositionY, offscreenPositionY);


                    if (pos.y == offscreenPositionY)
                    {
                        pos.x = offscreenPositionX;
                        SetPhase(Phase.Offscreen);
                        GenerateOffscreenWait();
                    }

                    transform.position = pos;
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
        if (phase != Phase.Perching)
            return;

        Bullet bullet = other.GetComponentInParent<Bullet>();
        if (bullet == null)
            return;

        bullet.owner.GetHit();
        bullet.Despawn();

        if (!alreadyHit)
        {
            alreadyHit = true;
            SetPhase(Phase.FlyingOff);
            audioSource.clip = getHitSFX;
            audioSource.Play();
        }
    }

    private void SetPhase(Phase phase)
    {
        this.phase = phase;
        
        if (phase == Phase.FlyingOff)
            spriteRenderer.flipX = true;

        if (phase == Phase.FlyingDown)
        {
            spriteRenderer.flipX = false;
            alreadyHit = false;
        }

        if (phase == Phase.FlyingOff || phase == Phase.FlyingDown)
        {
            frames = flyingFrames;
            animationRate = flyingAnimationRate;
        }
        else
        {
            frames = idleFrames;
            animationRate = idleAnimationRate;
        }

        frameIndex %= frames.Length;
    }

    private void GenerateOffscreenWait()
    {
        if (Random.value < 0.5f)
            waitTime = Mathf.Lerp(offsetScreenWaitMin, offsetScreenWaitMedian, Random.value);
        else
            waitTime = Mathf.Lerp(offsetScreenWaitMedian, offsetScreenWaitMax, Random.value);
    }
}
