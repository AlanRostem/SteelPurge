using Godot;
using System;

public class Gun : RayCast2D
{
	[Export] public float DamageRange;

	private float _direction = 1;

	public override void _Ready()
	{
		ConfigureScanLine(0);
	}

	public void ScanHit(float angle = 0)
	{
		Enabled = true;
		ForceRaycastUpdate();
		ConfigureScanLine(angle);
		var collider = GetCollider();
		if (collider != null)
		{
			var pos = GetCollisionPoint() - GlobalPosition;
			pos.y = 0;
			CastTo = pos;
			if (collider is Enemy enemy)
			{
				enemy.QueueFree();
			}
		}
		Enabled = false;
	}

	private void ConfigureScanLine(float angle)
	{
		CastTo = new Vector2(
			Mathf.Cos(angle) * DamageRange * _direction,
			Mathf.Sin(angle) * DamageRange
		);
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("fire"))
		{
			ScanHit();
		}
	}
}
