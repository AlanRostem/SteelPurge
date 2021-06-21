using Godot;
using System;

public class BurstFireTimer : Node
{
    [Export] public uint RateOfFire = 400;
    [Export] public uint BurstCount = 3;

    private Weapon _weapon;
    private Timer _timer;
    private uint _currentBurstCount = 0;

    public override void _Ready()
    {
        _weapon = GetParent<Weapon>();
        _timer = GetNode<Timer>("Timer");
        _timer.WaitTime = 60f / RateOfFire;
    }

    public void Start()
    {
        _weapon.EmitSignal(nameof(Weapon.Fired));
        _currentBurstCount++;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void _OnFire()
    {
        _weapon.EmitSignal(nameof(Weapon.Fired));
        _currentBurstCount++;
        if (_currentBurstCount == BurstCount)
        {
            _currentBurstCount = 0;
            _timer.Stop();
        }
    }
}