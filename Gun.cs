using Godot;
using System;

public class Gun : RayCast2D
{
	[Export]
	public float DamageRange;

    private float _direction = 1;
	public override void _Ready()
	{
        ConfigureScanLine(0);   
	}

    public void ScanHit(float angle = 0)
    {
        ConfigureScanLine(angle);
        // TODO: Hit scan logic
    }
    
    private void ConfigureScanLine(float angle)
    {
        CastTo = new Vector2(
                Mathf.Cos(angle) * DamageRange * _direction,
                Mathf.Sin(angle) * DamageRange
            );
    }
}
