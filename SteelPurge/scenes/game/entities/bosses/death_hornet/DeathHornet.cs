using Godot;
using System;

public class DeathHornet : Boss
{
	public override void _Ready()
	{
		base._Ready();
	}
	
	private void _OnRogueHit(HornetRogue body)
	{
		var damage = 5000u; // Change back to 500u
		TakeDamage(damage, Vector2.Zero);
		body.QueueFree();
		var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, body.Position);
		scrap.Count = body.ScrapDropKilled;
	}
}
