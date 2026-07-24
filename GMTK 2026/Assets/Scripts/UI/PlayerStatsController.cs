using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI dodgeText;

    private void Awake()
    {
        Assert.IsNotNull(controller);
        Assert.IsNotNull(ammoText);
        Assert.IsNotNull(dodgeText);
    }

    private void Update()
    {
        ammoText.text = $"{controller.Ammo} / {controller.MaxAmmo}";
        dodgeText.text = controller.DodgeCooldown > 0 ? $"{controller.DodgeCooldown}" : "";
    }
}
