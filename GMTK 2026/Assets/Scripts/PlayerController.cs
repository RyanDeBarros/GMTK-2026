using UnityEngine;

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

    [SerializeField] private int lives = 5;
    [SerializeField] private int maxAmmo = 2;
    [SerializeField] private int dodgeCooldownTurns = 3;

    private int ammo;
    private int dodgeCooldown = 0;

    public int MaxAmmo => maxAmmo;
    public int Ammo => ammo;
    public int DodgeCooldown => dodgeCooldown;

    private PlayerAction chosenAction = PlayerAction.None;
    public PlayerAction ChosenAction => chosenAction;

    private void Start()
    {
        ammo = maxAmmo;
    }

    public void Shoot()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (chosenAction == PlayerAction.None && ammo > 0)
            {
                chosenAction = PlayerAction.Shoot;
                --ammo;

                Debug.Log(name + ": Shoot");
                // TODO shoot bullet
                // TODO animation
                // TODO sfx
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Reload()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (chosenAction == PlayerAction.None && ammo < maxAmmo)
            {
                chosenAction = PlayerAction.Reload;
                ++ammo;

                Debug.Log(name + ": Reload");
                // TODO reload ammo
                // TODO animation
                // TODO sfx
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Dodge()
    {
        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            if (chosenAction == PlayerAction.None && dodgeCooldown <= 0)
            {
                chosenAction = PlayerAction.Dodge;
                dodgeCooldown = dodgeCooldownTurns;

                Debug.Log(name + ": Dodge");
                // TODO animation
                // TODO sfx
            }
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public bool IsDodging()
    {
        return chosenAction == PlayerAction.Dodge;
    }

    public void GetHit()
    {
        Debug.Log(name + ": Ow!");
        --lives;
        // TODO animation
        // TODO sfx

        if (IsDead())
            MatchManager.Instance.SetPhase(MatchPhase.End);
    }

    public void GetCritHit()
    {
        Debug.Log(name + ": Ow! (crit)");
        lives -= 2;
        // TODO animation
        // TODO sfx

        if (IsDead())
            MatchManager.Instance.SetPhase(MatchPhase.End);
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
