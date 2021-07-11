using Godot;
using System;
using Godot.Collections;

public class MagmaSpikes : Hazard
{
	public override Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetAny(nameof(Rotation), Rotation);
		return data.GetJson();
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		Rotation = eData.GetAny<float>(nameof(Rotation));
	}
}
