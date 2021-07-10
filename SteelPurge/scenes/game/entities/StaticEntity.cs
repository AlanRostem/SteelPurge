using Godot;
using System;
using Godot.Collections;

public class StaticEntity : Node2D
{
	private bool _needsEntityData = false;
	private Dictionary<string, object> _entityData;
	
	public World ParentWorld { get; protected set; }
	
	public override void _Ready()
	{
		ParentWorld = GetParent().GetParent().GetParent<World>();
		if (_needsEntityData)
		{
			CallDeferred(nameof(FeedEntityData), _entityData);
			_needsEntityData = false;
		}
	}
	
	public void FeedEntityDataForLater(Dictionary<string, object> data)
	{
		_needsEntityData = true;
		_entityData = data;
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
