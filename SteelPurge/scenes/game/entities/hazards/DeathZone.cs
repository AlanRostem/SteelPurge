using Godot;
using System;

public class DeathZone : StaticEntity
{
	private void _OnPlayerEnter(Player player)
	{
		player.Die();
	}
}
