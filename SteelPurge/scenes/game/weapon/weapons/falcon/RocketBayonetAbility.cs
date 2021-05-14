using Godot;

public class RocketBayonetAbility : TacticalAbility
{
	[Export] public uint BayonetDamage = 450;
	[Export] public float RocketSpeed = 250;
	public override void OnActivate()
	{
		var weapon = GetWeapon();
		weapon.MeleeHitBoxEnabled = true;
		weapon.CanFire = false;
		weapon.CanMelee = false;
		weapon.IsMeleeAttacking = false;

		var player = weapon.OwnerPlayer;
		player.Velocity.x = RocketSpeed * player.HorizontalLookingDirection;
		player.Velocity.y = 0;
		player.IsGravityEnabled = false;
		player.CanMove = false;
		player.IsAimingDown = false;
		player.CanAimDown = false;
		player.CanSwapDirection = false;
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;
		if (player.IsOnWall() || player.IsOnSlope)
		{
			DeActivate();
			player.Velocity.x = 0;
		}
	}

	public override void OnEnd()
	{
		var weapon = GetWeapon();
		weapon.MeleeHitBoxEnabled = false;
		weapon.CanFire = true;
		weapon.CanMelee = true;

		var player = weapon.OwnerPlayer;
		player.IsGravityEnabled = true;
		player.CanMove = true;
		player.CanAimDown = true;
		player.CanSwapDirection = true;
	}
	
	private void _OnFalconOnMeleeHit(VulnerableHitbox hitBox)
	{
		if (!IsActive) return;
		hitBox.TakeHit(BayonetDamage, new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 0));
		DeActivate();
		var player = GetWeapon().OwnerPlayer;
		player.Velocity.x = 0;
	}
}
