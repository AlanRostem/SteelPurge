using Godot;
using System;
using Object = Godot.Object;

public class Entity : KinematicBody2D
{
	private static float Gravity = 600;
	private static Vector2 Up = new Vector2(0, -1);
	public Vector2 Velocity;
	public Map ParentMap;

	public override void _Ready()
	{
		var parent = GetParent();
		if (OS.IsDebugBuild())
		{
			if (!(parent is Map))
			{
				throw new Exception("Parent node is not an instance of Map");
			}
		}

		ParentMap = (Map) parent;
	}

	public override void _PhysicsProcess(float delta)
	{
		Velocity.y += Gravity * delta;
		Velocity = MoveAndSlide(Velocity, Up, true);
		for (var i = 0; i < GetSlideCount(); i++)
		{
			var collision = GetSlideCollision(i);
			_OnCollision(collision.Collider);
		}
		_OnMovement(delta);
	}

	public virtual void _OnCollision(Object collider)
	{
		
	}

	protected virtual void _OnMovement(float delta)
	{
		
	}
}
