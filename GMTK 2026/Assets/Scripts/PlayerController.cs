using UnityEngine;

public enum SlomoAction
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

    private SlomoAction chosenAction = SlomoAction.None;
    public SlomoAction ChosenAction => chosenAction;

    public void Shoot()
    {
        if (chosenAction != SlomoAction.None) // TODO also early exit if no ammo
            return;

        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            chosenAction = SlomoAction.Shoot;
            Debug.Log(name + ": Shoot");
            // TODO shoot bullet
            // TODO animation
            // TODO sfx
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Reload()
    {
        if (chosenAction != SlomoAction.None) // TODO also early exit if ammo is full
            return;

        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            chosenAction = SlomoAction.Reload;
            Debug.Log(name + ": Reload");
            // TODO reload ammo
            // TODO animation
            // TODO sfx
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Dodge()
    {
        if (chosenAction != SlomoAction.None) // TODO also early exit if dodge is on cooldown
            return;

        if (MatchManager.Instance.Phase == MatchPhase.ChooseAction)
        {
            chosenAction = SlomoAction.Dodge;
            Debug.Log(name + ": Dodge");
            // TODO animation
            // TODO sfx
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public bool IsDodging()
    {
        return chosenAction == SlomoAction.Dodge;
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
        chosenAction = SlomoAction.None;
    }
}
