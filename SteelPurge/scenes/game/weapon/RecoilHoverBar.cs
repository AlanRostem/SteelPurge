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
			_Draw();
		}
	}

	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
		_maxShots = _weapon.MaxRecoilHoverShots;
		_currentShots = _weapon.MaxRecoilHoverShots;
	}

	public override void _Draw()
	{
		
	}
}
