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

	public override void _Ready()
	{
		base._Ready();
		Health = BaseHitPoints;
	}

	public virtual void OnDie()
	{
	}

	public void TakeDamage(uint damage)
	{

		
		if (damage >= Health)
		{
			OnDie();
			QueueFree();

			if (_isDead) return;
			_isDead = true;
			
			var scrap = (Scrap)ScrapScene.Instance();
			ParentWorld.AddChild(scrap);
			scrap.Position = Position;
			scrap.Count = ScrapDropKilled;

			Health = 0;
		}
		else
		{
			var scrap = (Scrap)ScrapScene.Instance();
			ParentWorld.AddChild(scrap);
			scrap.Position = Position;
			scrap.Count = ScrapDropHit;
			Health -= damage;
		}
	}

	private void _OnVulnerableHitboxHit(uint damage)
	{
		TakeDamage(damage);
	}
}
