using Godot;
using System;

public class DestructibleObstacle : StaticBody2D
{
    [Export] public uint Health = 1000;

    [Signal]
    public delegate void Destroyed();
    
    private void OnHit(uint damage)
    {
        if (damage >= Health)
        {
            EmitSignal(nameof(Destroyed));
            QueueFree();
        }

        Health -= damage;
    }
}