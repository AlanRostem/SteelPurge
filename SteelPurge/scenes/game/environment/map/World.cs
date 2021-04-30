using Godot;
using System;

public class World : Node2D
{
	public Player PlayerNode { get; private set; }
	public EntityPool Entities { get; private set; }

	public uint LastCheckPointUuid = 0;
	public uint CurrentCheckPointEarned = 0;
	public Fabricator CurrentCheckPoint = null;

	public override void _Ready()
	{
		PlayerNode = GetNode<Player>("Player");
		Entities = GetNode<EntityPool>("EntityPool");
	}
}
