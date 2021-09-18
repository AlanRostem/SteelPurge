using Godot;
using System;
using Godot.Collections;

public class MagmaSpikes : Hazard
{
	private LifeHitbox _lifeHitbox;
	
	public override void _Init()
	{
		base._Init();
		_lifeHitbox = GetNode<LifeHitbox>("LifeHitbox");
	}

	public override Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetAny(nameof(Rotation), Rotation);
		data.SetLifeHitbox(nameof(_lifeHitbox), _lifeHitbox);
		return data.GetJson();
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		Rotation = eData.GetAny<float>(nameof(Rotation));
		eData.ConfigureLifeHitbox(nameof(_lifeHitbox), _lifeHitbox);
	}
}
