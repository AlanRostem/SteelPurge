using Godot;
using System;

public class StaticEntity : Node2D
{
	public World ParentWorld { get; protected set; }
	
	public override void _Ready()
	{
		ParentWorld = GetParent().GetParent().GetParent<World>();
	}
	
	public virtual void FeedEntityData(EntityData<KinematicEntity> data)
	{
		Position = data.WorldPosition;
	}
	
	public virtual EntityData<KinematicEntity> ExportEntityData()
	{
		return null;
	}
}
