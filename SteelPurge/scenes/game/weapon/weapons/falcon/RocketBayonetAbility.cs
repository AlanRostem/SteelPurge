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

		var player = weapon.OwnerPlayer;
		player.VelocityX = RocketSpeed * player.HorizontalLookingDirection;
		player.VelocityY = 0;
		player.IsGravityEnabled = false;
		player.IsRamSliding = false;
		player.CanMove = false;
		player.IsAimingDown = false;
		player.CanAimDown = false;
		player.CanSwapDirection = false;
		player.CurrentCollisionMode = KinematicEntity.CollisionMode.Slide;
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;
		// TODO: Detect when player is on a slope and stop the ability
		if (player.IsOnWall() || player.IsOnSlope)
		{
			DeActivate();
			player.VelocityX = 0;
		}
	}

	public override void OnEnd()
	{
		var weapon = GetWeapon();
		weapon.MeleeHitBoxEnabled = false;
		weapon.CanFire = true;

		var player = weapon.OwnerPlayer;
		player.IsGravityEnabled = true;
		player.CanMove = true;
		player.CanAimDown = true;
		player.CanSwapDirection = true;
		player.CurrentCollisionMode = KinematicEntity.CollisionMode.Snap;
	}
	
	private void _OnFalconOnMeleeHit(VulnerableHitbox hitBox)
	{
		if (!IsActive) return;
		hitBox.TakeHit(BayonetDamage, new Vector2(GetWeapon().OwnerPlayer.HorizontalLookingDirection, 0));
		DeActivate();
		var player = GetWeapon().OwnerPlayer;
		player.VelocityX = 0;
	}
}
