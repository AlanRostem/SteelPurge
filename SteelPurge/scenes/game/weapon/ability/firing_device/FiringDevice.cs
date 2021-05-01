using Godot;
using System;

public class FiringDevice : WeaponAbility
{
	public override void _Ready()
	{
		base._Ready();
		GetWeapon().Connect(nameof(Weapon.Fired), this, nameof(OnFire));
		GetWeapon().FiringDevice = this;
	}

	public Projectile FireProjectile(PackedScene projectileScene, float angle = 0)
	{
		var player = GetWeapon().OwnerPlayer;
		var world = player.ParentWorld.Entities;
		var projectile = world.SpawnEntityDeferred<Projectile>(projectileScene, player.Position);
		projectile.DirectionAngle = Mathf.Rad2Deg(angle);
		projectile.VisualAngle = Mathf.Rad2Deg(angle);
		projectile.DirectionSign = GetWeapon().Scale.x;
		if (player.IsAimingDown)
		{
			projectile.DirectionAngle += 90;
			projectile.VisualAngle += Mathf.Rad2Deg(90);
		}
		else if (player.HorizontalLookingDirection < 0)
		{
			projectile.DirectionAngle = 180 - projectile.DirectionAngle;
		}

		projectile.Position = player.Position;
		projectile.InitWithAngularVelocity(GetWeapon());
		return projectile;
	}
	
	
	public virtual void OnFire()
	{

	}
}
