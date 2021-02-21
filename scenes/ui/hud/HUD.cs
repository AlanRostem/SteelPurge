using Godot;
using System;

public class HUD : Control
{
	[Signal]
	public delegate void HealthChanged(uint hp);

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
