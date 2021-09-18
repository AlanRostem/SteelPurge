using Godot;
using System;

public class FireArea : Area2D
{
	private CollisionShape2D _shape;

	public override void _Ready()
	{
		_shape = GetNode<CollisionShape2D>("CollisionShape2D");
		_shape.Disabled = true;
		Visible = false;
	}
	
 	private void _OnEnabled()
	{
		_shape.Disabled = false;
		Visible = true;
	}   
	
	private void _OnDisabled()
	{
		Visible = false;
		_shape.Disabled = true;
	}
}
