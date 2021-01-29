using Godot;
using System;

public class DamageReceptionCooldownTimer : Timer
{
	private void _on_Player_TriggerDamageReceptionCooldown()
	{
		Start();
	}
}
