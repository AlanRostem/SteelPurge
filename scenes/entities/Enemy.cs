using Godot;
using System;

public class Enemy : Entity
{
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
			_hp = 0;
		}
		else
		{
			_hp -= damage;
		}
	}
}
