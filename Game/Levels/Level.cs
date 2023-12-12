using Godot;
using System;
using System.Collections.Generic;

public enum Phases
{
	Attack,
	Defense
};

public partial class Level : Node2D
{
	public Random rand;
	public int Depth { get; set; }
	public Phases CurrentPhase { get; set; } = Phases.Attack;

	// Settings
	private int HealthScale = 10;
	private int HandSize = 7;
	private int NumCardSlots = 5;

	// Level Objects
	public TextureRect Background { get; set; }
	public Enemy Enemy { get; set; }
	public Player Player { get; set; }
	public Deck Deck { get; set; } = null;
	public DiscardPile DiscardPile { get; set; }
	public Hand Hand { get; set; }
	public List<CardSlot> CardSlots { get; set; } = new List<CardSlot>();
	public Label ResultLabel { get; set; }
	public Label EquationLabel { get; set; }
	public Button SubmitButton { get; set; }
	public Map Map { get; set; }
    public LevelMarker LevelMarker { get; set; }

	// Win/Lose Scene
	public PackedScene rewardScreen;
	public PackedScene gameOverScreen;
	public PackedScene winScreen;

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
		Enemy.Position = new Vector2(1100f, 450f);
        if(Depth == 9){
            Enemy.Position += new Vector2(0, -50f);
        }
		AddChild(Enemy);
        Enemy.UpdateIntention();

		// Connect death signals
		Player.Death += () => BeginDeathTimer(false);
		Enemy.Death += () => BeginDeathTimer(true);
		// --------------------------------------------------------
		PackedScene cardSlotScene = GD.Load<PackedScene>("res://Game/Cards/card_slot.tscn");

		Vector2 cardSlotPosition = new Vector2(430f, 450f);

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
		ResultLabel.Position = cardSlotPosition + new Vector2(-40f, -20f);

		// Put submit button in correct position
		SubmitButton = GetNode<Button>("SubmitButton");
		SubmitButton.Position = ResultLabel.Position + new Vector2(100f, 10f);
        SubmitButton.Text = "Attack!";

		// Put Equation Label in correct position and set its timer up
		EquationLabel = GetNode<Label>("EquationLabel");
		EquationLabel.Position = new Vector2(450f, 500f);

		Timer equationErrorTimer = EquationLabel.GetChild<Timer>(0);
		equationErrorTimer.Timeout += () => { EquationLabel.Visible = false; };

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
        rand = GetNode<World>("/root/World").RNG;
		int index = rand.Next(texturePaths.Length);
		var texture = (Texture2D)GD.Load(texturePaths[index]);

		if (texture != null)
		{
			Background.Texture = texture;
		}
		else
		{
			GD.Print("Failed to load Level texture");
		}
		// ----------------------------------------------------------
		// ----------------- Win/Lose Screens -------------------
		gameOverScreen = GD.Load<PackedScene>("res://Game/Menus/game_over.tscn");
		rewardScreen = GD.Load<PackedScene>("res://Game/Cards/Rewards.tscn");
		winScreen = GD.Load<PackedScene>("res://Game/Menus/win_screen.tscn");
		// ------------------------------------------------------
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LinkCardSignals(CardContainer container)
	{
		foreach (var child in container.GetChildren())
		{
			if (child is Card card)
			{
				card.CardMovedToSlot += UpdateResult;
			}
		}
	}

	public void _on_submit_button_pressed()
	{
		var equationInfo = ValidEquation();

		// When equation is valid
		if (equationInfo.Item1)
		{
            if(CurrentPhase == Phases.Attack){
			    int damage = EvaluateEquation(equationInfo.Item3) - Enemy.Defense;
			    Enemy.TakeDamage(damage);
                CurrentPhase = Phases.Defense;
                SubmitButton.Text = "Defend!";
            } else {
                int damage = Enemy.Attack - EvaluateEquation(equationInfo.Item3);
                Player.TakeDamage(damage);
                CurrentPhase = Phases.Attack;
                SubmitButton.Text = "Attack!";
            }
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
		var equationInfo = ValidEquation();

		if (equationInfo.Item1)
		{
			ResultLabel.Text = "= " + EvaluateEquation(equationInfo.Item3).ToString();
			SubmitButton.Disabled = false;
		}
		else
		{
			ResultLabel.Text = "= ";
			SubmitButton.Disabled = true;
		}
	}

	/// <summary>
	/// Evaluates the equation
	/// </summary>
	/// <param name="equation"></param>
	/// <returns>The integer value of the equation</returns>
	public int EvaluateEquation(List<(CardType, string)> equation)
	{
		while (equation.Contains((CardType.Operator, "x")))
		{
			int index = equation.IndexOf((CardType.Operator, "x"));
			int result = equation[index + 1].Item2.ToInt() * equation[index - 1].Item2.ToInt();
			equation.RemoveAt(index + 1);
			equation[index] = (CardType.Number, result.ToString());
			equation.RemoveAt(index - 1);
		}

		while (equation.Contains((CardType.Operator, "+")))
		{
			int index = equation.IndexOf((CardType.Operator, "+"));
			int result = equation[index + 1].Item2.ToInt() + equation[index - 1].Item2.ToInt();
			equation.RemoveAt(index + 1);
			equation[index] = (CardType.Number, result.ToString());
			equation.RemoveAt(index - 1);
		}

		if (equation.Count == 1)
		{
			return equation[0].Item2.ToInt();
		}
		else if (equation.Count == 0)
		{
			return 0;
		}
		else
		{
			GD.Print("Something broke with evaluating equations");
			return -1;
		}
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
		LinkCardSignals(Hand);
        Enemy.UpdateIntention();
		UpdateResult();
	}

	public void BeginDeathTimer(bool win)
	{
		SubmitButton.Visible = false;
		Timer deathTimer = GetNode<Timer>("DeathTimer");
		deathTimer.Timeout += () => EndLevel(win);
		deathTimer.Start();
	}

	public void EndLevel(bool win)
	{
		Map map = GetNode<Map>("/root/World/Map");

		if (Depth >= 9) {
			FreeEverything();
			GetTree().ChangeSceneToPacked(winScreen);
		} else if (win) {
			map.Current_Depth++;
			map.UpdateMarkers();
			Rewards Rewards = rewardScreen.Instantiate<Rewards>();
			GetNode("/root/World").AddChild(Rewards);
			FreeEverything();
		} else {	
			map.UpdateMarkers();
			FreeEverything();
			GetTree().ChangeSceneToPacked(gameOverScreen);
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

    public void _on_menu_opened(){
        GetNode<Button>("SubmitButton").Disabled = true;
        Hand.Dragging_Off();
    }
    public void _on_menu_closed(){
        GetNode<Button>("SubmitButton").Disabled = false;
        Hand.Dragging_On();
    }

    public void _on_level_selected(){
        // ------------ Loading Card Stuff ------------------------
        PackedScene deckScene = GD.Load<PackedScene>("res://Game/Cards/CardContainers/Deck/deck.tscn");
        PackedScene handScene = GD.Load<PackedScene>("res://Game/Cards/CardContainers/Hand/hand.tscn");
        PackedScene discardPileScene = GD.Load<PackedScene>("res://Game/Cards/CardContainers/DiscardPile/discard_pile.tscn");
        PackedScene cardScene = GD.Load<PackedScene>("res://Game//Cards/card.tscn");

        // Get map because it holds the inventory
        Map = (Map)GetParent();

        
        Deck = deckScene.Instantiate<Deck>();
        Deck.Position = new Vector2(100f, 600f);
        Deck.SetCards(Map.Inventory.Cards);

        Deck.ShuffleCards();
        Deck.Connect("MenuOpened", new Callable(this, nameof(_on_menu_opened)));
        Deck.Connect("MenuClosed", new Callable(this, nameof(_on_menu_closed)));
        AddChild(Deck);

        // Add discard pile
        DiscardPile = discardPileScene.Instantiate<DiscardPile>();
        DiscardPile.Position = new Vector2(1180f, 600f);
        DiscardPile.Connect("MenuOpened", new Callable(this, nameof(_on_menu_opened)));
        DiscardPile.Connect("MenuClosed", new Callable(this, nameof(_on_menu_closed)));
        AddChild(DiscardPile);

        // Add hand
        Hand = handScene.Instantiate<Hand>();
        AddChild(Hand);
        Hand.AddCards(Deck.DrawCards(7));
        Hand.UpdateHand();
        LinkCardSignals(Hand);
    }
}
