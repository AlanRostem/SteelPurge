using Godot;
using System;

public class Prop : Node2D
{
	protected Map ParentMap;

	public override void _Ready()
	{
		var parent = GetParent();
		if (OS.IsDebugBuild())
		{
			if (!(parent is Map))
			{
				throw new Exception("Parent node is not an instance of Map");
			}
		}

		ParentMap = (Map) parent;
	}
}
