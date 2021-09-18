using Godot;

public class DamageIndicator : Node2D
{
	private Timer _damageIndicatorTimer;

	private Node2D _node;

	public override void _Ready()
	{
		_damageIndicatorTimer = GetNode<Timer>("DamageIndicatorTimer");
	}

	public void Indicate(Color initialColor)
	{
		Indicate(initialColor, (Node2D) GetParent());
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="initialColor"></param>
	/// <param name="node">Node that will change its color. Should be a parent or grandparent node.</param>
	public void Indicate(Color initialColor, Node2D node)
	{
		_node = node;
		node.Modulate = initialColor;
		_damageIndicatorTimer.Start();
	}

	private void _OnRevertColor()
	{
		_node.Modulate = new Color(1, 1, 1, 1);
	}
}
