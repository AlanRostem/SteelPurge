using Godot;
using System;
using Godot.Collections;

public class Flare : Projectile
{
	private CustomTimer _lifeTimer;
	
	public override void _Init()
	{
		base._Init();
		_lifeTimer = GetNode<CustomTimer>("LifeTimer");
	}

	protected override void OnDamageDealt(uint damage, VulnerableHitbox hitbox)
	{
		hitbox.TakeHit(0, VulnerableHitbox.DamageType.Heat);
	}

	public override Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetTimer(nameof(_lifeTimer), _lifeTimer);
		return data.GetJson();
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		eData.ConfigureTimer(nameof(_lifeTimer), _lifeTimer);
	}
}
