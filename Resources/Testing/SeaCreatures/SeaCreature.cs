using Godot;
using System;

public partial class SeaCreature : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_hitbox_mouse_entered()
	{
		Scale = new Vector2(1.5f, 1.1f);
	}

	public void _on_hitbox_mouse_exited()
	{
		Scale = new Vector2(1f, 1f);
	}
}
