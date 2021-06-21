using Godot;
using System;

public class BurstFireTimer : Node
{
	[Export] public uint RateOfFire = 400;
	[Export] public uint BurstCount = 3;
	[Export] public bool RecoilHover = true;

	private FiringDevice _device;
	private Timer _timer;
	private uint _currentBurstCount = 0;

	public override void _Ready()
	{
		_device = GetParent<FiringDevice>();
		_timer = GetNode<Timer>("Timer");
		_timer.WaitTime = 60f / RateOfFire;
	}

	public void Start()
	{
		_device.OnFireOutput();
		_currentBurstCount++;
		_timer.Start();
	}

	public void Stop()
	{
		_timer.Stop();
	}

	private void _OnFire()
	{
		if (RecoilHover)
			_device.GetWeapon().ProduceRecoilToHover();
		_device.OnFireOutput();
		_currentBurstCount++;
		if (_currentBurstCount >= BurstCount)
		{
			_currentBurstCount = 0;
			_timer.Stop();
		}
	}
}
