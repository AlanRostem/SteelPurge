using Godot;
using System;

public class MovingPlatform : KinematicEntity
{
    [Export] public int Direction = 1;
    [Export] public float TravelDistanceInTiles = 3;
    [Export] public float MovementVelocity = 30;
    private float _currentDistance = 0;

    protected override void _OnMovement(float delta)
    {
        if (Direction > 0)
        {
            _currentDistance += MovementVelocity * delta;
            Velocity.x = MovementVelocity;
            if (_currentDistance >= CustomTileMap.Size * TravelDistanceInTiles)
                Direction = -1;
        }
        else if (Direction < 0)
        {
            _currentDistance -= MovementVelocity * delta;
            Velocity.x = -MovementVelocity;
            if (_currentDistance <= -CustomTileMap.Size * TravelDistanceInTiles)
                Direction = 1;
        }
    }
}