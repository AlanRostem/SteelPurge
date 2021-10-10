using Godot;
using System;

public class RecoilHoverBar : Control
{
	private Weapon _weapon;
	private uint _currentShots = 0;
	private uint _maxShots = 0;
	private Timer _visibilityTimer;

	public uint CurrentShots
	{
		get => _currentShots;
		set
		{
			_currentShots = value;
			Update();
			if (!Visible)
			{
				Visible = true;
			}

			if (_currentShots == _maxShots)
				_visibilityTimer.Start();
		}
	}

	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
		_visibilityTimer = GetNode<Timer>("VisibilityTimer");
		_maxShots = _weapon.MaxRecoilHoverShots;
		CurrentShots = _weapon.MaxRecoilHoverShots;
		Visible = false;
	}

	public override void _Draw()
	{
		const uint maxHeight = 2;
		const uint totalWidth = 8 * 2;
		const uint margin = 1;
		for (var i = 0; i < _maxShots; i++)
		{
			var rect = new Rect2(i * (totalWidth / _maxShots + margin) - totalWidth / 2, 
				0, totalWidth / _maxShots - margin, maxHeight);
			var color = i < _currentShots ? new Color(0, 255, 255) : new Color(0, 0, 0);
			DrawRect(rect, color);
		}
	}
	
	private void _OnInvisible()
	{
		Visible = false;
	}
}
