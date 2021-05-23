using Godot;
using System;

public class FireBeamSpot : StaticEntity
{
	public uint FireDamage = 45;
	private CollisionShape2D _fireAreaShape;
	private Timer _fireDurationTimer;
	private bool _triggered = false;
	
	public override void _Ready()
	{
		base._Ready();
		_fireAreaShape = GetNode<CollisionShape2D>("FireArea/CollisionShape2D");
		_fireDurationTimer = GetNode<Timer>("FireDurationTimer");
	}
	
	
	private void _OnRogueEnterTrigger(HornetRogue rogue)
	{
		if (_triggered) return;
		_fireDurationTimer.Start();
		_fireAreaShape.Disabled = false;
		_fireAreaShape.Visible = true;
		_triggered = true;
		rogue.QueueFree();
	}
	
	private void _OnPlayerEnterFire(object body)
	{
		GD.Print("huh");
		var player = (Player) body;
		player.TakeDamage(FireDamage, new Vector2(-player.HorizontalLookingDirection, 0));
	}

	private void _OnPlayerLeaveFireArea(Player body)
	{
		
	}

	private void _OnFireEnd()
	{
		_fireAreaShape.Disabled = true;
		_fireAreaShape.Visible = false;
		_triggered = false;
	}
}
