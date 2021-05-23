using Godot;
using System;

public class DeathHornet : Boss
{
	private static readonly PackedScene RogueScene 
		= GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/HornetRogue.tscn");

	[Export] public uint CriticalDamageByRogue = 1000u;

	private Position2D _bottomRogueSpawnPoint;
	private Position2D _leftRogueSpawnPoint;
	private Position2D _rightRogueSpawnPoint;

	public override void _Ready()
	{
		base._Ready();
		_bottomRogueSpawnPoint = GetNode<Position2D>("BottomRogueSpawnPoint");
		_leftRogueSpawnPoint = GetNode<Position2D>("LeftRogueSpawnPoint");
		_rightRogueSpawnPoint = GetNode<Position2D>("RightRogueSpawnPoint");
	}

	private void ShootRogueFromSide(int direction)
	{
		var position = direction < 0 ? _leftRogueSpawnPoint.Position : _rightRogueSpawnPoint.Position;
		var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene, position);
		rogue.Direction = direction;
	}
	
	private void DropRogueFromBelow()
	{
		var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene, _bottomRogueSpawnPoint.Position);
		rogue.Direction = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
	}
	
	private void _OnRogueHit(HornetRogue body)
	{
		TakeDamage(CriticalDamageByRogue, Vector2.Zero);
		body.QueueFree();
		var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, body.Position);
		scrap.Count = body.ScrapDropKilled;
	}
}
