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
			Velocity.x = Mathf.Lerp(Velocity.x, MaxVelocity * 1.5f * Mathf.Cos(angle), 0.14f);
			Velocity.y = Mathf.Lerp(Velocity.y, MaxVelocity * 1.5f * Mathf.Sin(angle), 0.14f);
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
			var firingDevice = (TalconFiringDevice) player.PlayerInventory.EquippedWeapon.FiringDevice;
			player.PlayerInventory.EquippedWeapon.CurrentAmmo++;
			firingDevice.Talons.Remove(this);
			if (player.PlayerInventory.EquippedWeapon.CurrentAmmo == player.PlayerInventory.EquippedWeapon.MaxAmmoCount)
			{
				firingDevice.GetWeapon().CanFire = true;
			}
			QueueFree();
		}
	}
}
