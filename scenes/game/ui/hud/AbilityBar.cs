using Godot;
using System;

public class AbilityBar : ProgressBar
{
	/*
	public override void _Process(float delta)
	{
		var ability = _player.WeaponInventory.EquippedWeapon.TacticalAbilityRef;
		if (ability == null) return;
		if (ability.IsActive)
		{
			MaxValue = ability.Duration;
			Value = ability.CurrentDuration;
		}

		if (ability.IsOnCoolDown)
		{
			MaxValue = ability.CoolDown;
			Value = ability.CoolDown - ability.CurrentCoolDown;
		}
	}*/
}
