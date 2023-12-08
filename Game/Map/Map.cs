using Godot;
using System;
using System.Collections.Generic;

public partial class Map : CanvasLayer
{
	private int STARTING_INVENTORY_SIZE = 10;
	private int MAX_STARTING_NUM = 10;
	private int CHANCE_OF_STARTING_MULT_OP = 5;

    // Index of current level in Levels array
    public int Current_Depth = 0;
	public Inventory Inventory { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ------------- Making Starting Inventory of Cards ------------------
		PackedScene inventoryScene = GD.Load<PackedScene>("res://Game/Cards/inventory.tscn");
		PackedScene cardScene = GD.Load<PackedScene>("res://Game//Cards/card.tscn");
		Inventory = inventoryScene.Instantiate<Inventory>();

		Random rand = new Random();

		for (int i = 0; i < STARTING_INVENTORY_SIZE; i++)
		{
			Card card = cardScene.Instantiate<Card>();

			// Generate Number
			if (rand.Next(2) == 0)
			{
				card.InitCard(rand.Next(1, MAX_STARTING_NUM), i);
				Inventory.AddCard(card);
			}
			// Generate Operator
			else
			{
				// 1 in CHANCE_OF_STARTING_MULT_OP chance of each operator being a * instead of a +
				if (rand.Next(CHANCE_OF_STARTING_MULT_OP) == 0)
				{
					card.InitCard("*", i);
				}
				else
				{
					card.InitCard("+", i);
				}
				Inventory.AddCard(card);
			}
		}
		// -------------------------------------------------------------------
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
}
