using Godot;
using System;

public class HUD : Control
{
	[Signal]
	public delegate void HealthChanged(uint hp);

    public override void _Ready()
    {
        var player = GetNode<Player>("../../Level1/Player");
        player.Connect(nameof(Player.UpdateHealth), this, nameof(UpdateHealth));
    }

	public void UpdateHealth(uint hp)
	{
		GD.Print("Health: ", hp);
    }

	public void UpdateScrapCount(uint hp)
	{
	}

	public void UpdateWeaponName(string name)
	{
	}

	public void UpdateAmmo(uint clip, uint reserve)
	{
	}

	public void UpdateTacticalAbility(TacticalAbility ability)
	{
	}

	public void UpdateTacticalAbilityDuration(float time)
	{
	}

	public void UpdateTacticalAbilityCooldown(float time)
	{
	}
}
