using Godot;
using System;

public class DeathZone : Node2D
{
	private void _OnPlayerEnter(Player player)
	{
		player.Die();
	}
}
