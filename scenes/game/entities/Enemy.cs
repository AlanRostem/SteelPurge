using Godot;
using System;
using System.Collections;
using Object = Godot.Object;

public class Enemy : Entity
{
	private static readonly PackedScene ScrapScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");

	[Export] public uint ScrapDropHit = 2;
	[Export] public uint ScrapDropKilled = 25;

	[Export] public uint BaseHitPoints = 45;
	private bool _isDead;
	private bool _dropScrap;
	private bool _isPlayerFound;
	public Player DetectedPlayer { get; private set; }

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
			var scrap = (Scrap) ScrapScene.Instance();
			ParentWorld.AddChild(scrap);
			scrap.Position = Position;
			scrap.Count = ScrapDropKilled;
			QueueFree();
		}

		if (_dropScrap)
		{
			_dropScrap = false;
			var scrap = (Scrap) ScrapScene.Instance();
			ParentWorld.AddChild(scrap);
			scrap.Position = Position;
			scrap.Count = ScrapDropHit;
		}

		if (_isPlayerFound)
		{
			_WhenPlayerDetected(DetectedPlayer);
		}
		else
		{
			_WhenPlayerNotSeen();
		}
	}

	public override void TakeDamage(uint damage, float direction = 0)
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

	private void _OnVulnerableHitboxHit(uint damage)
	{
		TakeDamage(damage);
	}

	protected virtual void _WhenPlayerDetected(Player player)
	{
		
	}

	protected virtual void _WhenPlayerNotSeen()
	{
		
	}
	
	private void _OnPlayerDetected(object body)
	{
		DetectedPlayer = (Player)body;
		_isPlayerFound = true;
	}
	
	private void _OnPlayerLeave(object body)
	{
		_isPlayerFound = false;
	}
}
