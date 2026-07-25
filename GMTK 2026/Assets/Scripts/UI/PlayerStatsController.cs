using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI dodgeText;

    [SerializeField] private GameObject shootPrompt;
    [SerializeField] private GameObject reloadPrompt;
    [SerializeField] private GameObject dodgePrompt;
    [SerializeField] private GameObject slomoPrompt;

    private void Awake()
    {
        Assert.IsNotNull(controller);
        Assert.IsNotNull(ammoText);
        Assert.IsNotNull(dodgeText);

        Assert.IsNotNull(shootPrompt);
        Assert.IsNotNull(reloadPrompt);
        Assert.IsNotNull(dodgePrompt);
        Assert.IsNotNull(slomoPrompt);
    }

    private void Update()
    {
        ammoText.text = $"{controller.Ammo} / {controller.MaxAmmo}";
        dodgeText.text = controller.DodgeCooldown > 0 ? $"{controller.DodgeCooldown}" : "";

        // TODO use tint overlay instead to disable action UI
        shootPrompt.SetActive(controller.CanShoot());
        reloadPrompt.SetActive(controller.CanReload());
        dodgePrompt.SetActive(controller.CanDodge());

        slomoPrompt.SetActive(MatchManager.Instance.Phase == MatchPhase.Slomo && controller.ChosenAction == PlayerAction.Shoot);
    }
}
