using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerInputRouter : MonoBehaviour
{
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;

    private PlayerInput playerInput;

    private class Moveset
    {
        public PlayerController controller;
        public InputAction shoot;
        public InputAction reload;
        public InputAction dodge;

        public void OnEnable()
        {
            shoot.performed += OnShoot;
            reload.performed += OnReload;
            dodge.performed += OnDodge;
        }

        public void OnDisable()
        {
            shoot.performed -= OnShoot;
            reload.performed -= OnReload;
            dodge.performed -= OnDodge;
        }

        private void OnShoot(InputAction.CallbackContext _)
        {
            controller.Shoot();
        }

        private void OnReload(InputAction.CallbackContext _)
        {
            controller.Reload();
        }

        private void OnDodge(InputAction.CallbackContext _)
        {
            controller.Dodge();
        }
    }

    private readonly Moveset moveset1 = new();
    private readonly Moveset moveset2 = new();

    private InputAction pauseAction;

    private void Awake()
    {
        Assert.IsNotNull(player1);
        Assert.IsNotNull(player2);

        playerInput = GetComponent<PlayerInput>();
        Assert.IsNotNull(playerInput);

        moveset1.controller = player1;
        moveset1.shoot = playerInput.actions["P1 Shoot"];
        moveset1.reload = playerInput.actions["P1 Reload"];
        moveset1.dodge = playerInput.actions["P1 Dodge"];

        moveset2.controller = player2;
        moveset2.shoot = playerInput.actions["P2 Shoot"];
        moveset2.reload = playerInput.actions["P2 Reload"];
        moveset2.dodge = playerInput.actions["P2 Dodge"];

        pauseAction = playerInput.actions["Pause"];
    }

    private void OnEnable()
    {
        moveset1.OnEnable();
        moveset2.OnEnable();
        pauseAction.performed += OnPause;
    }

    private void OnDisable()
    {
        moveset1.OnDisable();
        moveset2.OnDisable();
        pauseAction.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext _)
    {
        // TODO
    }
}
