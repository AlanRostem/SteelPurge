using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 SpawnPoint { get; private set; }
	public EntityPool Entities { get; private set; }

	private World _parentWorld;
	
	public override void _Ready()
	{
		SpawnPoint = GetNode<Fabricator>("Environment/Fabricator").Position;
		Entities = GetNode<EntityPool>("EntityPool");
		_parentWorld = GetParent<World>();
	}
	
	private void _OnTransferAreaPlayerEntered(object body)
	{
		var player = (Player) body;
		_parentWorld.SwitchToNextSegment();
		player.InitiateRespawnSequence();
		player.Position = _parentWorld.CurrentSegment.SpawnPoint;
	}
}
