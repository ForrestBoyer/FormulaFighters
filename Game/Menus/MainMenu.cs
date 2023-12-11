using Godot;
using System;
using System.Net;

public partial class MainMenu : Node2D
{
	public PackedScene worldScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		worldScene = GD.Load<PackedScene>("res://Game/world.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("quit")) 
		{
			GetTree().Quit();
		}
	}

	public void _on_start_button_pressed()
	{
		GetTree().ChangeSceneToPacked(worldScene);
	}

	public void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
