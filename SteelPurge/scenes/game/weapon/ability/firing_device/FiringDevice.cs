using Godot;
using System;

public class FiringDevice : Node2D
{
	private float _alternationOffset = 2f;
	
	[Export] public bool AreProjectilesAlternatingInOffsets = true;
	
	private Weapon _weapon;
	
	public T GetWeapon<T>() where T : Weapon
	{
		return (T)_weapon;
	}

	public Weapon GetWeapon()
	{
		return _weapon;
	}
	
	public override void _Ready()
	{
		base._Ready();
		_weapon = GetParent<Weapon>();
		GetWeapon().Connect(nameof(Weapon.Fired), this, nameof(OnFireInit));
		GetWeapon().FiringDevice = this;
	}

	public Projectile FireProjectile(PackedScene projectileScene, float angle = 0)
	{
		return FireProjectile(projectileScene, _weapon.DamagePerShot, angle);
	}
	public Projectile FireProjectile(PackedScene projectileScene, uint damage, float angle = 0)
	{
		var player = GetWeapon().OwnerPlayer;
		var world = player.ParentWorld.CurrentSegment.Entities;
		
		var projectile = world.SpawnEntityDeferred<Projectile>(projectileScene, player.Position + GetWeapon().Position);

		projectile.DirectionAngle = Mathf.Rad2Deg(angle);
		projectile.VisualAngle = 0;
		projectile.DirectionSign = GetWeapon().OwnerPlayer.HorizontalLookingDirection;
		if (player.IsAimingDown)
		{
			projectile.DirectionAngle += 90;
			projectile.VisualAngle += 90;
		}
		else if (player.IsAimingUp)
		{
			projectile.DirectionAngle -= 90;
			projectile.VisualAngle -= 90;
		}
		else if (player.HorizontalLookingDirection < 0)
		{
			projectile.DirectionAngle = 180 - projectile.DirectionAngle;
		}

		if (AreProjectilesAlternatingInOffsets && !player.IsAimingDown && !player.IsAimingUp)
		{
			projectile.SpritePosition = new Vector2(0, _alternationOffset *= -1);
		}

		projectile.InitWithAngularVelocity(GetWeapon());
		projectile.Damage = damage;

		GetWeapon().Connect("tree_exited", projectile, nameof(projectile._OnParentWeaponLost));
		return projectile;
	}
	
	
	public virtual void OnFireInit()
	{
		OnFireOutput();
	}

	public virtual void OnFireOutput()
	{
		
	}


	public virtual void OnSwap()
	{
		
	}
}
