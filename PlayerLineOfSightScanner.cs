using Godot;
using System;

public class PlayerLineOfSightScanner : RayCast2D
{
	[Signal]
	public delegate void PlayerSighted();
	
	[Signal]
	public delegate void PlayerVisualLost();
	
	private bool _isPlayerSeen = false;
	
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var collider = GetCollider();
		if (collider is Player)
		{
			if (!_isPlayerSeen)
			{
				_isPlayerSeen = true;
				EmitSignal(nameof(PlayerSighted));
			}
		}
		else
		{
			if (_isPlayerSeen)
			{
				EmitSignal(nameof(PlayerVisualLost));
				_isPlayerSeen = false;				
			}
		}
	}
}
