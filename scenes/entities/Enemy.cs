using Godot;
using System;
using System.Collections;

public class Enemy : Entity
{
	private PackedScene _scrapScene = GD.Load<PackedScene>("res://scenes/entities/Scrap.tscn");

	[Export] public uint ScrapValue = 50;

	[Export] public uint BaseHitPoints = 45;
	private uint _hp;

	public override void _Ready()
	{
		base._Ready();
		_hp = BaseHitPoints * ParentMap.Round;
	}

	public virtual void OnDie()
	{
	}

	public void TakeDamage(uint damage)
	{
		if (damage >= _hp)
		{
			OnDie();
			QueueFree();
			var scrap = (Scrap) _scrapScene.Instance();
			scrap.Position = Position;
			scrap.Value = ScrapValue;
			ParentMap.AddChild(scrap);
			ParentMap.CurrentExpectedEnemies--;
			ParentMap.EnemiesOnMap--;
			if (ParentMap.CurrentExpectedEnemies == 0)
			{
				ParentMap.BeginNewRound();
			}
			_hp = 0;
		}
		else
		{
			_hp -= damage;
		}
	}


	private void _OnDisappear()
	{
		ParentMap.EnemiesOnMap--;
		QueueFree();
	}
}
