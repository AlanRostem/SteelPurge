using Godot;
using System;

public class Enemy : Entity
{
	private PackedScene _scrapScene = GD.Load<PackedScene>("res://scenes/entities/Scrap.tscn");
	
	[Export] public uint ScrapValue = 50;
	
 	[Export]
	public uint BaseHitPoints = 45;
	private uint _hp;

	public override void _Ready()
	{
		base._Ready();
		_hp = BaseHitPoints * ParentMap.PlayerRef.Stats.Round;
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
			var scrap = (Scrap)_scrapScene.Instance();
			scrap.Position = Position;
			scrap.Value = ScrapValue;
			ParentMap.AddChild(scrap);
			_hp = 0;
		}
		else
		{
			_hp -= damage;
		}
	}
}
