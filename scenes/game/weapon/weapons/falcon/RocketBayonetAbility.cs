using Godot;
using System;

public class RocketBayonetAbility : TacticalAbility
{
	[Export] public uint BayonetDamage = 450;
	[Export] public float RocketSpeed = 250;
	public override void OnActivate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.Velocity.x = RocketSpeed * player.HorizontalLookingDirection;
		player.Velocity.y = 0;
		player.IsGravityEnabled = false;
		player.CanMove = false;
		GetWeapon().MeleeHitBoxEnabled = true;
	}

	public override void OnEnd()
	{
		var player = GetWeapon().OwnerPlayer;
		player.IsGravityEnabled = true;
		player.CanMove = true;
		GetWeapon().MeleeHitBoxEnabled = true;
	}
	
	private void _OnFalconOnMeleeHit(VulnerableHitbox hitBox)
	{
		hitBox.TakeHit(BayonetDamage, (int)GetWeapon().OwnerPlayer.HorizontalLookingDirection);
		DeActivate();
	}
}
