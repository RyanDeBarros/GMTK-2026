using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int lives = 5;

    public void Shoot()
    {
        if (MatchManager.Instance.Phase == MatchPhase.Slomo)
        {
            Debug.Log(name + ": Shoot");
            // TODO
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Reload()
    {
        if (MatchManager.Instance.Phase == MatchPhase.Slomo)
        {
            Debug.Log(name + ": Reload");
            // TODO
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
    }

    public void Dodge()
    {
        if (MatchManager.Instance.Phase == MatchPhase.Slomo)
        {
            Debug.Log(name + ": Dodge");
            // TODO
        }
        else if (MatchManager.Instance.Phase == MatchPhase.Countdown)
            GetHit();
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
}
