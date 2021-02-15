using Godot;
using System;

public class AbilityBar : ProgressBar
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent<AbilityIcon>().GetParent<HUD>().GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		var ability = _player.WeaponHolder.EquippedWeapon.TacticalAbilityRef;
		if (ability.IsActive)
		{
			MaxValue = ability.Duration;
			Value = ability.CurrentDuration;
			if (Value < 0)
				Value = 0;
		}

		if (ability.IsOnCoolDown)
		{
			MaxValue = ability.CoolDown;
			Value = ability.CoolDown - ability.CurrentCoolDown;
		}
	}
}
