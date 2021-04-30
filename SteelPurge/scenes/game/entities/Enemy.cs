using Godot;
using System;
using System.Collections;
using Object = Godot.Object;

public class Enemy : KinematicEntity
{
	private static readonly PackedScene ScrapScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");

	[Export] public uint ScrapDropHit = 2;
	[Export] public uint ScrapDropKilled = 25;

	[Export] public uint BaseHitPoints = 45;
	[Export] public float PlayerDetectionRange = 1000;
	private bool _isDead;
	private bool _dropScrap;
	public Player DetectedPlayer {get; private set; }

	public override void _Ready()
	{
		base._Ready();
		Health = BaseHitPoints;
	}

	public virtual void OnDie()
	{
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (_isDead)
		{
			var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
			scrap.Count = ScrapDropKilled;
			QueueFree();
		}

		if (_dropScrap)
		{
			_dropScrap = false;
			var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
			scrap.Count = ScrapDropHit;
		}

		var distance = Mathf.Abs(ParentWorld.PlayerNode.Position.x - Position.x);
		if (distance < PlayerDetectionRange)
		{
			if (DetectedPlayer is null)
				DetectedPlayer = ParentWorld.PlayerNode;
			_WhenPlayerDetected(ParentWorld.PlayerNode);
		}		
		else
		{
			_WhenPlayerNotSeen();
		}
	}

	public override void TakeDamage(uint damage, int direction = 0)
	{
		if (damage >= Health)
		{
			OnDie();

			_isDead = true;

			Health = 0;
		}
		else
		{
			_dropScrap = true;
			Health -= damage;
		}
	}

	private void _OnVulnerableHitboxHit(uint damage, int knockBackDirection)
	{
		TakeDamage(damage, knockBackDirection);
	}

	protected virtual void _WhenPlayerDetected(Player player)
	{
		
	}

	protected virtual void _WhenPlayerNotSeen()
	{
		
	}
}
