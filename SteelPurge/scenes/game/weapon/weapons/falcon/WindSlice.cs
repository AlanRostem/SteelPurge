using Godot;
using System;

public class WindSlice : Projectile
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	private bool _alreadyHitFloor = false;
	public override void _Ready()
	{
		CurrentCollisionMode = CollisionMode.Snap;
	}

	protected override void _OnMovement(float delta)
	{
		if (IsOnWall())
			Disappear();

		if (!_alreadyHitFloor)
		{
			_alreadyHitFloor = true;
			return;
		}
		
		if (!IsOnFloor())
			Disappear();
	}
}
