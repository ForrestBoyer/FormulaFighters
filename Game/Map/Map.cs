using Godot;
using System;

public partial class Map : CanvasLayer
{
    // Index of current level in Levels array
    public int Current_Depth = 0;
	public Inventory Inventory { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Inventory = GetNode<Inventory>("/root/World/Inventory");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame. 
	public override void _Process(double delta)
	{
	}

	public void UpdateMarkers()
	{
		foreach (Node child in GetChildren())
		{
			if (child is LevelMarker levelMarker)
			{
				levelMarker.Scale = new Vector2(1.5f, 1.5f);
				if (levelMarker.Depth == Current_Depth)
				{
					if (levelMarker.IsBoss)
					{
						levelMarker.TextureNormal = (Texture2D)GD.Load("res://Game/Map/LevelMarkerIcons/current_boss.png");
					}
					else
					{
						levelMarker.TextureNormal = (Texture2D)GD.Load("res://Game/Map/LevelMarkerIcons/current.png");
					}
				}
				else
				{
					if (levelMarker.IsBoss)
					{
						levelMarker.TextureNormal = (Texture2D)GD.Load("res://Game/Map/LevelMarkerIcons/normal_boss.png");
					}
					else
					{
						levelMarker.TextureNormal = (Texture2D)GD.Load("res://Game/Map/LevelMarkerIcons/normal.png");
					}
				}
			}
		}
	}

    public void _on_menu_opened(){
        foreach (Node child in GetChildren())
		{
			if (child is LevelMarker levelMarker) {
                levelMarker.Disabled = true;
            }
        }
    }

    public void _on_menu_closed(){
        foreach (Node child in GetChildren())
		{
			if (child is LevelMarker levelMarker) {
                if(levelMarker.Depth != Current_Depth - 1)
                levelMarker.Disabled = false;
            }
        }
    }
}
