using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Serves as a checkpoint and crafting station
/// </summary>
public class Fabricator : StaticEntity
{
	private static PackedScene FloatingTextScene = GD.Load<PackedScene>("res://scenes/game/ui/FloatingTempText.tscn");
	
	[Export] public bool IsCheckPoint = false;

	private bool _touchedCheckPoint = false;

	private void _OnInteract(Player player)
	{
		// TODO: Heal player
	}

	private void _OnPlayerEntered(Player player)
	{
		if (player.IsRespawning) _touchedCheckPoint = true;
		if (!IsCheckPoint || _touchedCheckPoint) return;
		_touchedCheckPoint = true;
		var text = (FloatingTempText) FloatingTextScene.Instance();
		text.Text = "Checkpoint!";
		text.Modulate = Colors.Lime;
		ParentWorld.AddChild(text);
		text.Position = Position + new Vector2(0, -36);
		ParentWorld.CurrentReSpawnPoint = Position;
	}
}
