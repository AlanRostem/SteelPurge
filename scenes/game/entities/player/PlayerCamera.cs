using Godot;
using System;

public class PlayerCamera : Camera2D
{
	[Export] public float Offset = 40;
	[Export] public float Smoothness = 0.005f;
	private Player _parent;
	
	public override void _Ready()
	{
		_parent = GetParent<Player>();
	}

	private float PosX
	{
		get => Position.x;
		set => Position = new Vector2(value, Position.y);
	} 
	
	public override void _Process(float delta)
	{
		if (_parent.HorizontalLookingDirection < 0)
		{
			PosX = Mathf.Lerp(PosX, -Offset, Smoothness);
		}
		else if (_parent.HorizontalLookingDirection > 0)
		{
			PosX = Mathf.Lerp(PosX, Offset, Smoothness);			
		}
	}
}
