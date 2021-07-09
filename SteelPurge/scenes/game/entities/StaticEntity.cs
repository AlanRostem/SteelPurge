using Godot;
using System;
using Godot.Collections;

public class StaticEntity : Node2D
{
	public World ParentWorld { get; protected set; }
	
	public override void _Ready()
	{
		ParentWorld = GetParent().GetParent().GetParent<World>();
	}
	
	public virtual void FeedEntityData(Dictionary<string, object> data)
	{
		var entityData = new EntityData(data);
		Position = entityData.GetVector(nameof(Position));
	}
	
	public virtual Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData();
		data.SetAny(nameof(Filename), Filename);
		data.SetVector(nameof(Position), Position);
		return data.GetJson();
	}
	
	public virtual void OnRemoved()
	{
		
	}
}
