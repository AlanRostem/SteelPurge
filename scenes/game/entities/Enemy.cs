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
	}

	public void TakeDamage(uint damage)
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
}
