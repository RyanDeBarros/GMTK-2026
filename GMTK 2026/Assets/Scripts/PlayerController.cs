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

// TODO pan audio sources to left/right for P1/P2?
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

    private bool dodging = false;
    public bool Dodging => dodging;

    [SerializeField] private ClipGroup shootSFX;
    [SerializeField] private ClipGroup getHitSFX;
    [SerializeField] private ClipGroup getHitCritSFX;
    [SerializeField] private AudioClip reloadSFX;
    [SerializeField] private AudioClip dodgeSFX;
    [SerializeField] private AudioSource tooEarlyBuzzer;
    [SerializeField] private AudioSource unavailableAction;

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
        Assert.IsNotNull(reloadSFX);
        Assert.IsNotNull(dodgeSFX);
        Assert.IsNotNull(tooEarlyBuzzer);
        Assert.IsNotNull(unavailableAction);

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
            else
                unavailableAction.Play();
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
        {
            tooEarlyBuzzer.Play();
            GetHit();
        }
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
                audioSource.clip = reloadSFX;
                audioSource.Play();
                // TODO animation
            }
            else
                unavailableAction.Play();
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
        {
            tooEarlyBuzzer.Play();
            GetHit();
        }
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
                dodging = true;
                dodgeCooldown = dodgeCooldownTurns;
                audioSource.clip = dodgeSFX;
                audioSource.Play();
                // TODO animation
            }
            else
                unavailableAction.Play();
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
        {
            tooEarlyBuzzer.Play();
            GetHit();
        }
    }

    public bool CanDodge()
    {
        return CanSelectAction() && dodgeCooldown <= 0;
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
        if (chosenAction == PlayerAction.Shoot)
            unavailableAction.Play();

        chosenAction = PlayerAction.None;

        if (dodgeCooldown > 0)
            --dodgeCooldown;
    }

    public void StartChooseActionPhase()
    {
        dodging = false;
    }
}
