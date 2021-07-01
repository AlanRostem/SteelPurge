using Godot;
using System;

public class Hazard : StaticEntity
{
	[Export] public uint Damage = 50;
	
	private void _OnPlayerTouch(Player player)
	{
		player.TakeDamage(Damage, new Vector2(-player.HorizontalLookingDirection, 0));
	}
}
