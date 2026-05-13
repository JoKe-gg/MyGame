using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCombat : Attackable
{
    [SerializeField] Weapon Weapon;
    private PlayerActionMap input;
    private void Awake()
    {
        bool error = false;

        if (Weapon == null) 
        {
            Debug.Log($"Miised \"{nameof(Weapon)}\" in the script \"{nameof(PlayerCombat)}\".");
            error = true;
        }
        if (error) 
        {
            enabled = false;
        }
        input = new PlayerActionMap();
    }
    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        if (input.Player.attack.IsPressed())
        {
            Weapon.TryAttack();
        }
        if (input.Player.ability.IsPressed())
        {
            Weapon.TryAbilityAttack();
        }
    }
}
