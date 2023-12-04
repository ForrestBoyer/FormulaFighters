using Godot;
using System;

public partial class card_dock : StaticBody2D
{
	private drag Drag;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Drag = GetNode<drag>("/root/drag");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Drag.is_dragging) {
			Modulate = new Color("ORANGE_RED", 1);
		}
		else {
			Modulate = new Color("ORANGE", 1);
		}
	}
}
