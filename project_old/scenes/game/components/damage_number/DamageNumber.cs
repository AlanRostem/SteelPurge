using Godot;
using System;

public class DamageNumber : FloatingTempText
{	
	private uint _damage = 0;

	public uint Damage
	{
		get => _damage;
		set
		{
			Text = value.ToString();
			ExistenceTimer?.CallDeferred("start");
			_damage = value;
		}
	}
}


