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
	private bool _isPlayerNearShop = false;

	public override void _Ready()
	{
		_world = GetParent().GetParent<World>();
		Order = _world.LastCheckPointUuid++;
	}

	public override void _Process(float delta)
	{
		if (!_isPlayerNearShop) return;
		// TODO: Code
	}

	private void _OnPlayerEnter(object body)
	{
		_isPlayerNearShop = true;
		// TODO: Consider more factors after Prototype 1
		if (_world.CurrentCheckPointEarned >= Order)
			return;
		_world.CurrentCheckPointEarned = Order;
		_world.CurrentCheckPoint = this;
	}

	private void _OnPlayerLeave(object body)
	{
		_isPlayerNearShop = false;
	}
}
