using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	public Random rand = new Random();
	public int Depth { get; set; }

	// Settings
	private int HealthScale = 10;
	private int HandSize = 7;
	private int NumCardSlots = 5;

	// Level Objects
	public TextureRect Background { get; set; }
	public Enemy Enemy { get; set; }
	public Player Player { get; set; }
	public Deck Deck { get; set; }
	public DiscardPile DiscardPile { get; set; }
	public Hand Hand { get; set; }
	public List<CardSlot> CardSlots { get; set; } = new List<CardSlot>();
	public Label ResultLabel { get; set; }
	public Label EquationLabel { get; set; }
	public Map Map { get; set; }

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
		PackedScene cardSlotScene = GD.Load<PackedScene>("res://Game/Cards/card_slot.tscn");

		// Get map because it holds the inventory
		Map = (Map)GetParent();

		// Initiate starting deck in level with what is in inventory
		Deck = deckScene.Instantiate<Deck>();
		Deck.Position = new Vector2(100f, 600f);
		Deck.SetCards(Map.Inventory.Cards);

		GD.Print(Map.Inventory.Cards.Count);

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
			CardSlots.Add(slot);
			AddChild(slot);
		}

		// Put result label in correct position
		ResultLabel = GetNode<Label>("ResultLabel");
		ResultLabel.Position = cardSlotPosition + new Vector2(-20f, -20f);

		// Put submit button in correct position
		Button submitButton = GetNode<Button>("SubmitButton");
		submitButton.Position = ResultLabel.Position + new Vector2(60f, 10f);

		// Put Equation Label in correct position and set its timer up
		EquationLabel = GetNode<Label>("EquationLabel");
		EquationLabel.Position = new Vector2(450f, 500f);

		Timer timer = EquationLabel.GetChild<Timer>(0);
		timer.Timeout += () => { EquationLabel.Visible = false; };

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
			Enemy.TakeDamage(1000);
		}
	}

	public void _on_submit_button_pressed()
	{
		var equationInfo = ValidEquation();

		// When equation is valid
		if (equationInfo.Item1)
		{
			NewTurn();
		}
		// When equation is invalid
		else
		{
			// Display error message for short amount of time
			EquationLabel.Text = equationInfo.Item2;
			EquationLabel.Visible = true;
			Timer timer = EquationLabel.GetChild<Timer>(0);
			timer.Start();
		}
	}

	/// <summary>
	/// Checks Validity of the equation
	/// </summary>
	/// <returns>Returns a tuple that contains whether or not it was a valid equation, and a string describing why if invalid</returns>
	public (bool, string, List<(CardType, string)>) ValidEquation()
	{
		// Fills equation with cards in slots
		var equation = new List<(CardType, string)>();
		foreach (CardSlot slot in CardSlots)
		{
			if (!slot.SlotOpen)
			{
				if (slot.CardInSlot.CardType == CardType.Number)
				{
					equation.Add((CardType.Number, slot.CardInSlot.IntVal.ToString()));
				}
				else
				{
					equation.Add((CardType.Operator, slot.CardInSlot.OpVal));
				}
			}
		}

		// Empty equation works, more or less passing a turn
		if (equation.Count == 0)
		{
			return (true, "Empty Equation", equation);
		}

		// Can't start with operator
		if (equation[0].Item1 == CardType.Operator)
		{
			return (false, "Invalid Equation: Equation cannot begin with an operator.", equation);
		}

		// Can't end with operator
		if (equation[^1].Item1 == CardType.Operator)
		{
			return (false, "Invalid Equation: Equation cannot end with an operator.", equation);
		}

		// This loop makes sure there isn't two of the same type of cards in a row
		CardType previous = CardType.Number; // Not relevant will be overriten immediatly
		for (int i = 0; i < equation.Count; i++)
		{
			if (i == 0)
			{
				previous = equation[i].Item1;
			}
			else
			{
				if (equation[i].Item1 == previous)
				{
					return (false, "Invalid Equation: Cannot have two of the same type in a row.", equation);
				}
				else
				{
					previous = equation[i].Item1;
				}
			}
		}

		return (true, "Valid Equation", equation);
	}

	/// <summary>
	/// Updates the label on the level to show the new equations value, does nothing if equation is invalid
	/// </summary>
	public void UpdateResult()
	{
		GD.Print("IN UPDATE RESULT");

		var equationInfo = ValidEquation();

		if (equationInfo.Item1)
		{
			ResultLabel.Text = EvaluateEquation(equationInfo.Item3).ToString();
		}
	}

	/// <summary>
	/// Evaluates the equation
	/// </summary>
	/// <param name="equation"></param>
	/// <returns>The integer value of the equation</returns>
	public int EvaluateEquation(List<(CardType, string)> equation)
	{
		return 5;
	}

	public void NewTurn()
	{
		foreach (CardSlot slot in CardSlots)
		{
			slot.SlotOpen = true;
			slot.CardInSlot = null;
		}

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
			FreeEverything();
		}
		else
		{	
			GD.Print("Player Died");
			// TODO: Go back to main menu, possibly have a lose screen?

			map.UpdateMarkers();
			FreeEverything();
		}
	}

	public void FreeEverything()
	{
		Deck.QueueFree();
		Hand.QueueFree();
		DiscardPile.QueueFree();
		Player.QueueFree();
		Enemy.QueueFree();
		QueueFree();
	}
}
