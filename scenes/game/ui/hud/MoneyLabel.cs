using Godot;
using System;

public class MoneyLabel : Label
{
	private void _OnPlayerScrapCountChanged(uint count)
	{
		Text = "x" + count;
	}	
}
