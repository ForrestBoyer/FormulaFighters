using System.Runtime.ExceptionServices;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public partial class Level : Node2D
{
	private int HealthScale = 10;
	private int HandSize = 7;
	private int NumCardSlots = 5;
	public Random rand = new Random();
	public int Depth { get; set; }

	// Level Objects
	public TextureRect Background { get; set; }
	public Enemy Enemy { get; set; }
	public Player Player { get; set; }
	public Deck Deck { get; set; }
	public DiscardPile DiscardPile { get; set; }
	public Hand Hand { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// ---------   Loading Enemies and Players ---------------
		// Gets scenes for player and enemy
		PackedScene enemyScene = GD.Load<PackedScene>("res://Game/Entities/Enemy/enemy.tscn");
		PackedScene playerScene = GD.Load<PackedScene>("res://Game//Entities/Player/player.tscn");

		// Create a player and set its position
		Player = playerScene.Instantiate<Player>();
		Player.Position = new Vector2(240f, 450f);
		AddChild(Player);

		// Create an enemy and set its position
		Enemy = enemyScene.Instantiate<Enemy>();
		Enemy.Position = new Vector2(1040f, 450f);
		AddChild(Enemy);

		// Connect death signals
		Player.Death += () => EndLevel(false);
		Enemy.Death += () => EndLevel(true);
		// --------------------------------------------------------

		// ------------ Loading Card Stuff ------------------------

		PackedScene deckScene = GD.Load<PackedScene>("res://Game/Cards/deck.tscn");
		PackedScene handScene = GD.Load<PackedScene>("res://Game/Cards/hand.tscn");
		PackedScene discardPileScene = GD.Load<PackedScene>("res://Game/Cards/discard_pile.tscn");
		PackedScene cardScene = GD.Load<PackedScene>("res://Game//Cards/card.tscn");
		PackedScene cardSlotScene = GD.Load<PackedScene>("res://Game//Cards/card_slot.tscn");

		// Add Deck
		Deck = deckScene.Instantiate<Deck>();
		Deck.Position = new Vector2(100f, 600f);

		// Get map because it holds the inventory
		Map map = (Map)GetParent();

		// Initiate starting deck in level with what is in inventory
		Deck.SetCards(map.Inventory.Cards);
		Deck.ShuffleCards();
		AddChild(Deck);

		// Add discard pile
		DiscardPile = discardPileScene.Instantiate<DiscardPile>();
		DiscardPile.Position = new Vector2(1180f, 600f);
		AddChild(DiscardPile);

		// Add hand
		Hand = handScene.Instantiate<Hand>();
		Hand.SetCards(Deck.DrawCards(7));
		Hand.UpdateHand();
		AddChild(Hand);

		Vector2 cardSlotPosition = new Vector2(450f, 450f);

		// Create Card Slots
		for (int i = 0; i < NumCardSlots; i++)
		{
			CardSlot slot = cardSlotScene.Instantiate<CardSlot>();
			slot.Position = cardSlotPosition;
			cardSlotPosition.X += 80f;
			AddChild(slot);
		}

		// Put result label in correct position
		Label resultLabel = GetNode<Label>("ResultLabel");
		resultLabel.Position = cardSlotPosition + new Vector2(-20f, -20f);

		// Put submit button in correct position
		Button submitButton = GetNode<Button>("SubmitButton");
		submitButton.Position = resultLabel.Position + new Vector2(60f, 10f);

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
		if (Input.IsActionJustPressed("right_click")) 
		{
			EndLevel(true);
		}
	}

	public void _on_submit_button_pressed()
	{
		NewTurn();
	}

	public void NewTurn()
	{
		// Move cards from hand to discard pile
		DiscardPile.AddCards(Hand.DrawCards(Hand.size));

		// If not enough cards in deck to draw
		if (Deck.size < HandSize)
		{
			int remainingCardsToDraw = HandSize - Deck.size;

			// Move remaining cards in deck to hand
			Hand.AddCards(Deck.DrawCards(Deck.size));

			// Move discard pile to deck
			Deck.AddCards(DiscardPile.DrawCards(DiscardPile.size));

			Deck.ShuffleCards();

			Hand.AddCards(Deck.DrawCards(remainingCardsToDraw));
		}
		else
		{
			Hand.AddCards(Deck.DrawCards(HandSize));
		}

		Hand.UpdateHand();
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
