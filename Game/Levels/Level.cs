using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	private int HealthScale = 10;
	public TextureRect Background { get; set; }
	public Random rand = new Random();
	public Enemy Enemy { get; set; }
	public Player Player { get; set; }
	public deck Deck { get; set; }
	public int Depth { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ---------   Loading Enemies and Players ---------------
		// Gets scenes for player and enemy
		PackedScene enemyScene = GD.Load<PackedScene>("res://Game/Entities/Enemy/enemy.tscn");
		PackedScene playerScene = GD.Load<PackedScene>("res://Game//Entities/Player/player.tscn");

		// Create a player and set its position
		Player = playerScene.Instantiate<Player>();
		Player.Position = new Vector2(360f, 450f);
		AddChild(Player);

		// Create an enemy and set its position
		Enemy = enemyScene.Instantiate<Enemy>();
		Enemy.Position = new Vector2(920f, 450f);
		AddChild(Enemy);

		// Connect death signals
		Player.Death += () => EndLevel(false);
		Enemy.Death += () => EndLevel(true);
		// --------------------------------------------------------

		// ------------ Loading Card Stuff ------------------------

		PackedScene deckScene = GD.Load<PackedScene>("res://Game/Cards/deck.tscn");
		PackedScene handScene = GD.Load<PackedScene>("res://Game/Cards/hand_manager.tscn");
		PackedScene discardPileScene = GD.Load<PackedScene>("res://Game/Cards/discard_pile.tscn");
		PackedScene cardScene = GD.Load<PackedScene>("res://Game//Cards/new_card.tscn");

		// Add Deck
		Deck = deckScene.Instantiate<deck>();
		Deck.Position = new Vector2(100f, 600f);

		// Get map because it holds the inventory
		Map map = (Map)GetParent();

		// Initiate starting deck in level with what is in inventory
		Deck.InitCardContainer(map.Inventory.Cards);
		AddChild(Deck);

		// --------------------------------------------------------

		// ----------------- Setting Background -------------------
		Background = GetNode<TextureRect>("Background");

		// Paths to all possible backgrounds
		string[] texturePaths = 
		{
			"res://Game/Levels/Backgrounds/redBricksRoom.png",
			"res://Game/Levels/Backgrounds/darkBlueRoom.png",
			"res://Game/Levels/Backgrounds/throneRoom.png",
		};

		// Loads random texture
		int index = rand.Next(texturePaths.Length);
		var texture = (Texture2D)GD.Load(texturePaths[index]);

		if (texture != null)
		{
			Background.Texture = texture;
		}
		else
		{
			GD.Print("Faied to load Level texture");
		}
		// ----------------------------------------------------------
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void EndLevel(bool win)
	{
		Map map = GetNode<Map>("/root/World/Map");

		if (win)
		{
			GD.Print("Enemy Died");
			// TODO: Open rewards screen

			map.Current_Depth++;
			map.UpdateMarkers();
			QueueFree();
		}
		else
		{	
			GD.Print("Player Died");
			// TODO: Go back to main menu, possibly have a lose screen?

			map.UpdateMarkers();
			QueueFree();
		}
	}
}
