using Godot;
using System;
using System.Collections;
using Object = Godot.Object;

public class Enemy : Entity
{
	private static readonly PackedScene ScrapScene = GD.Load<PackedScene>("res://scenes/entities/Scrap.tscn");

	[Export] public uint ScrapValue = 50;

	[Export] public uint BaseHitPoints = 45;
	private uint _hp;
	private bool _isDead = false;

	public override void _Ready()
	{
		base._Ready();
		_hp = BaseHitPoints;
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
			var scrap = (Scrap) ScrapScene.Instance();
			scrap.Position = Position;
			scrap.Value = ScrapValue;
			_isDead = true;
			_hp = 0;
		}
		else
		{
			_hp -= damage;
		}
	}


	private void _OnDisappear()
	{
		if (!_isDead)
		{
			QueueFree();
		}
	}
	
	
	private void _OnVulnerableHitboxHit(uint damage)
	{
		TakeDamage(damage);
	}

}

