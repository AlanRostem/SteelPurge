using Godot;
using System;

public class WorldSegment : Node2D
{
	public Vector2 InitialSpawnPoint => GetNode<Position2D>("SpawnPoint").Position;

	public EntityPool Entities { get; private set; }

	public World ParentWorld { get; private set; }
	
	private CustomTileMap _tileMap;
	
	public override void _Ready()
	{
		Entities = GetNode<EntityPool>("EntityPool");
		ParentWorld = GetParent<World>();
		_tileMap = GetNode<CustomTileMap>("Environment/CustomTileMap");
		var rect = _tileMap.GetUsedRect();
		rect.Position *= CustomTileMap.Size;
		rect.Size *= CustomTileMap.Size;
		ParentWorld.PlayerNode.SetCameraBounds(rect);
		ParentWorld.CurrentReSpawnPoint = new Vector2(InitialSpawnPoint);
	}
	
	private void _OnTransferAreaPlayerEntered(object body)
	{
		ParentWorld.SwitchToNextSegment();
	}
}
