using Godot;
using System;

public class Talon : Projectile
{
	private bool _followPlayer = false;

	public override void _PhysicsProcess(float delta)
	{
		if (_followPlayer)
		{
			var angle = OwnerWeapon.OwnerPlayer.Position.AngleToPoint(Position);
			Velocity.x = Mathf.Lerp(Velocity.x, MaxVelocity * Mathf.Cos(angle), 0.14f);
			Velocity.y = Mathf.Lerp(Velocity.y, MaxVelocity * Mathf.Sin(angle), 0.14f);
		}

		base._PhysicsProcess(delta);
	}

	public override void _OnHit()
	{
		if (!_followPlayer)
		{
			Velocity = new Vector2();
			_followPlayer = true;
		}
	}

	public override void _OnLostVisual()
	{
		if (!_followPlayer)
		{
			Velocity = new Vector2();
			_followPlayer = true;
		}
	}

	private void _OnPlayerDetectionAreaPlayerEntered(object body)
	{
		if (_followPlayer)
		{
			var player = (Player) body;
			var firingDevice = (TalconFiringDevice) player.EquippedWeapon.FiringDevice;
			firingDevice.Ammo++;
			firingDevice.Talons.Remove(this);
			if (firingDevice.Ammo == TalconFiringDevice.MaxAmmo)
			{
				firingDevice.GetWeapon().CanFire = true;
			}
			QueueFree();
		}
	}
}
