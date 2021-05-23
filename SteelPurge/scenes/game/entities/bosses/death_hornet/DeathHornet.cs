using Godot;
using System;

public class DeathHornet : Boss
{
	private static readonly PackedScene RogueScene 
		= GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/HornetRogue.tscn");

	[Export] public uint CriticalDamageByRogue = 400u;

	private Position2D _bottomRogueSpawnPoint;
	private Position2D _leftRogueSpawnPoint;
	private Position2D _rightRogueSpawnPoint;
	private Timer _rogueSpawnTimer;

	public override void _Ready()
	{
		base._Ready();
		_bottomRogueSpawnPoint = GetNode<Position2D>("BottomRogueSpawnPoint");
		_leftRogueSpawnPoint = GetNode<Position2D>("LeftRogueSpawnPoint");
		_rightRogueSpawnPoint = GetNode<Position2D>("RightRogueSpawnPoint");
		_rogueSpawnTimer = GetNode<Timer>("RogueSpawnTimer");
		
		// TODO: Remove this test later
		_rogueSpawnTimer.Start();
	}

	private void ShootRogueFromSide(int direction)
	{
		var position = direction < 0 ? _leftRogueSpawnPoint.Position : _rightRogueSpawnPoint.Position;
		var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene, position + Position);
		rogue.Direction = direction;
	}
	
	private void DropRogueFromBelow()
	{
		var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene, _bottomRogueSpawnPoint.Position + Position);
		rogue.Direction = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
	}
	
	private void _OnRogueHit(HornetRogue body)
	{
		TakeDamage(CriticalDamageByRogue, Vector2.Zero);
		body.QueueFree();
		var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, body.Position);
		scrap.Count = body.ScrapDropKilled;
	}
	
	private void _OnSpawnRogue()
	{
		DropRogueFromBelow();
	}
}
