using Godot;
using System;

public class Talon : Projectile
{
	private Sprite _sprite;
	private bool _followPlayer = false;
	
	public override void _Ready()
	{
		base._Ready();
		_sprite = GetNode<Sprite>("Sprite");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		_sprite.FlipH = Velocity.x < 0;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_followPlayer)
		{
			var angle = OwnerWeapon.OwnerPlayer.Position.AngleToPoint(Position);
			Velocity.x = MaxVelocity * Mathf.Cos(angle);
			Velocity.y = MaxVelocity * Mathf.Sin(angle);
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
			QueueFree();
		}
	}
}
