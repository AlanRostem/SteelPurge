using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 InitialSpawnPoint => GetNode<Position2D>("SpawnPoint").Position;

	public EntityPool Entities { get; private set; }

	public Node2D Environment { get; private set; }

	public World ParentWorld { get; private set; }

	private CustomTileMap _tileMap;

	public override void _Ready()
	{
		Entities = GetNode<EntityPool>("EntityPool");
		Environment = GetNode<Node2D>("Environment");
		ParentWorld = GetParent<World>();
		_tileMap = GetNode<CustomTileMap>("Environment/CustomTileMap");
		var rect = _tileMap.GetUsedRect();
		rect.Position *= CustomTileMap.Size;
		rect.Size *= CustomTileMap.Size;
		ParentWorld.PlayerNode.SetCameraBounds(rect);
	}
	
	
	private void _OnTransferAreaPlayerEntered(object body)
	{
		ParentWorld.SwitchToNextSegment();
	}
}
