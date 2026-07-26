using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

public enum PlayerAction
{
    None,
    Shoot,
    Reload,
    Dodge
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player1;
    public static PlayerController Player2;

    private Pistol pistol;
    public Pistol Pistol => pistol;

    [SerializeField] private int maxLives = 5;
    [SerializeField] private int maxAmmo = 2;
    [SerializeField] private int dodgeCooldownTurns = 3;

    private int lives;
    private int ammo;
    private int dodgeCooldown = 0;

    public int Lives => lives;
    public int MaxLives => maxLives;
    public int MaxAmmo => maxAmmo;
    public int Ammo => ammo;
    public int DodgeCooldown => dodgeCooldown;

    private PlayerAction chosenAction = PlayerAction.None;
    public PlayerAction ChosenAction => chosenAction;

    [SerializeField] private ClipGroup shootSFX;
    [SerializeField] private ClipGroup getHitSFX;
    [SerializeField] private ClipGroup getHitCritSFX;

    private AudioSource audioSource;

    private void Awake()
    {
        pistol = GetComponent<Pistol>();
        Assert.IsNotNull(pistol);

        ammo = maxAmmo;
        lives = maxLives;

        Assert.IsNotNull(shootSFX);
        Assert.IsNotNull(getHitSFX);
        Assert.IsNotNull(getHitCritSFX);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public bool CanSelectAction()
    {
        return (MatchManager.Instance.Phase == MatchPhase.Countdown || MatchManager.Instance.Phase == MatchPhase.ChooseAction) && chosenAction == PlayerAction.None;
    }

    public void Shoot()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (CanShoot())
            {
                chosenAction = PlayerAction.Shoot;
                Soundtrack.Play(Song.Slomo, true);
                // TODO animation
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
        else if (MatchManager.Instance.Phase == MatchPhase.Slomo && chosenAction == PlayerAction.Shoot)
        {
            chosenAction = PlayerAction.None;
            --ammo;
            pistol.Shoot();
            audioSource.clip = shootSFX.Poll();
            audioSource.Play();
            // TODO animation
        }
    }

    public bool CanShoot()
    {
        return CanSelectAction() && ammo > 0;
    }

    public bool IsAiming()
    {
        return MatchManager.Instance.Phase == MatchPhase.Slomo && chosenAction == PlayerAction.Shoot;
    }

    public void Reload()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (CanReload())
            {
                chosenAction = PlayerAction.Reload;
                ++ammo;

                // TODO animation
                // TODO sfx
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public bool CanReload()
    {
        return CanSelectAction() && ammo < maxAmmo;
    }

    public void Dodge()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (CanDodge())
            {
                chosenAction = PlayerAction.Dodge;
                dodgeCooldown = dodgeCooldownTurns;

                // TODO animation
                // TODO sfx
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public bool CanDodge()
    {
        return CanSelectAction() && dodgeCooldown <= 0;
    }

    public bool IsDodging()
    {
        return chosenAction == PlayerAction.Dodge;
    }

    public void GetHit()
    {
        --lives;
        audioSource.clip = getHitSFX.Poll();
        audioSource.Play();
        // TODO animation

        OnTakeDamage();
    }

    public void GetCritHit()
    {
        lives -= 2;
        audioSource.clip = getHitCritSFX.Poll();
        audioSource.Play();
        // TODO animation

        OnTakeDamage();
    }

    private void OnTakeDamage()
    {
        if (IsDead())
            MatchManager.Instance.MatchComplete();
        else
            MatchManager.Instance.CheckCountdownMusicChange();
    }

    public bool IsDead()
    {
        return lives <= 0;
    }

    public void StartCountdownPhase()
    {
        chosenAction = PlayerAction.None;

        if (dodgeCooldown > 0)
            --dodgeCooldown;
    }
}
