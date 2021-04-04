using Godot;
using System;

public class RocketBayonetAbility : TacticalAbility
{
	public override void OnActivate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.Velocity.x = 140 * player.HorizontalLookingDirection;
		player.Velocity.y = 0;
		player.IsGravityEnabled = false;
		player.CanMove = false;
	}

	public override void OnEnd()
	{
		var player = GetWeapon().OwnerPlayer;
		player.IsGravityEnabled = true;
		player.CanMove = true;
	}
}
