using Godot;
using System;

/// <summary>
/// Serves as a checkpoint and crafting station. The order in
/// which these are stacked in the Node Tree determines their
/// checkpoint order. 
/// </summary>
public class Fabricator : Area2D
{
	private World _world;
	public uint Order { get; private set; }
	[Export] public bool IsDefaultSpawnPoint = false;
	public override void _Ready()
	{
		_world = GetParent<World>();
		if (IsDefaultSpawnPoint)
		{
			_world.CurrentCheckPoint = this;
		}
		Order = _world.LastCheckPointUuid++;
	}
	
	private void _OnPlayerEnter(object body)
	{
		// TODO: Consider more factors after Prototype 1
		if (_world.CurrentCheckPointEarned >= Order)
			return;
		_world.CurrentCheckPointEarned = Order;
		_world.CurrentCheckPoint = this;
	}
}
