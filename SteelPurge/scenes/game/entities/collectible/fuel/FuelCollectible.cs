using Godot;
using System;

public class FuelCollectible : FallingCollectible
{

	private const uint MaxFades = 25;
	
	[Export] public uint Count = 20;

	private Timer _fadeAlternationTimer;
	private uint _fades = 0;
	
	public override void _Ready()
	{
		base._Ready();
		_fadeAlternationTimer = GetNode<Timer>("FadeAlternationTimer");
	}

	public override bool CollectionCondition(Player player)
	{
		var fuel = player.PlayerInventory.GetOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum);
		return fuel < player.PlayerInventory.MaxOrdinanceFuel;
	}

	public override void OnCollected(Player player)
	{
		player.PlayerInventory.IncreaseOrdinanceFuel(player.PlayerInventory.EquippedWeaponEnum, Count);
	}
	
	private void _OnStartFadingAway()
	{
		_fadeAlternationTimer.Start();
	}
	
	private void _OnFadeAlternation()
	{
		SetDeferred("visible", !Visible);
		_fades++;
		if (_fades >= MaxFades)
			QueueFree();
	}
}
