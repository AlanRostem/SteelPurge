using Godot;
using System;

public class KineticBubbleAbility : TacticalAbility
{
	public override void OnActivate()
	{
		GetWeapon().OwnerPlayer.CanTakeDamage = false;
	}

	public override void OnEnd()
	{
		GetWeapon().OwnerPlayer.CanTakeDamage = true;
	}
}
