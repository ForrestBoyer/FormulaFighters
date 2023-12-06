using Godot;
using System;
using System.Collections.Generic;

public partial class Map : CanvasLayer
{
    // An array to hold all of the levels
    // <Level, Level_number> (to support branches paths)
    public List<Level> Levels = new List<Level>();

    // Index of current level in Levels array
    public int Current_Depth = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
