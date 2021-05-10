using Godot;
using System;

public class PlayerCamera : Camera2D
{
	public float MaxMargin = 0.4f;
	public float MinMargin = 0.08f;
	public float Smoothness = 0.03f;
	private Player _player;

	public override void _Ready()
	{
		_player = GetParent<Player>();
	}

	public override void _Process(float delta)
	{
		if (_player.PlayerInventory.EquippedWeapon.IsFiring)
		{
			if (_player.HorizontalLookingDirection < 0)
			{
				DragMarginRight = Mathf.Lerp(DragMarginRight, MaxMargin, Smoothness);
			}
			else if (_player.HorizontalLookingDirection > 0)
			{
				DragMarginLeft = Mathf.Lerp(DragMarginLeft, MaxMargin, Smoothness);
			}
		}
		else
		{
			DragMarginLeft = Mathf.Lerp(DragMarginLeft, MinMargin, Smoothness);
			DragMarginRight = Mathf.Lerp(DragMarginRight, MinMargin, Smoothness);
		}
	}
}
