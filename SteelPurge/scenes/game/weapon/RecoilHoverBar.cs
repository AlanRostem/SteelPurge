using Godot;
using System;

public class RecoilHoverBar : Control
{
	private Weapon _weapon;
	private uint _currentShots = 0;
	private uint _maxShots = 0;

	public uint CurrentShots
	{
		get => _currentShots;
		set
		{
			_currentShots = value;
			Update();
		}
	}

	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
		_maxShots = _weapon.MaxRecoilHoverShots;
		CurrentShots = _weapon.MaxRecoilHoverShots;
	}

	public override void _Draw()
	{
		const float maxHeight = 2f;
		const float totalWidth = 8f * 2f;
		for (var i = 0; i < _maxShots; i++)
		{
			var rect = new Rect2(i * (totalWidth / _maxShots + 1) - totalWidth / 2, 
				0, totalWidth / _maxShots, maxHeight);
			var color = i < _currentShots ? new Color(0, 255, 255) : new Color(150);
			GD.Print(color);
			DrawRect(rect, color);
		}
	}
}
