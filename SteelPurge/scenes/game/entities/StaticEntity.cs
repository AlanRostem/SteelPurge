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
		var sData = new StaticEntityData<StaticEntity>(data);
		Position = sData.WorldPosition;
	}
	
	public virtual Dictionary<string, object> ExportEntityData()
	{
		return new StaticEntityData<StaticEntity>(this).GetJson();
	}
	
	public virtual void OnRemoved()
	{
		
	}
}
