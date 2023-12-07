using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Timers;

public partial class card_test : Node2D
{
	enum State
	{
		StartTurn,
		DuringTurn,
		EndTurn,
		WinLevel,
		LoseLevel
	}
	private const int HandSize = 7;
	private int DeckSize;
	private deck newDeck;
	private hand_manager newHand;
	private discard_pile newDiscard;
	private State combat;
	private new_card[] testInventory;
	private new_card testCard;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/new_card.tscn");
		// Test inventory
		testInventory = new new_card[21];
		for(int i = 0; i < 21; i++) {
			if(i % 2 == 0) {
				testCard = cardScene.Instantiate<new_card>();
				testCard.InitCard(2, i);
				testInventory[i] = testCard;
				//testInventory[i] = new new_card(2, i);
			} else {
				testCard = cardScene.Instantiate<new_card>();
				testCard.InitCard("+", i);
				testInventory[i] = testCard;
				//testInventory[i] = new new_card("+", i);
			}
		}

		DeckSize = testInventory.Length;

		// ---------   Loading Card Deck, Hand, and Discard Pile ---------------
		// Gets scenes for deck, hand, and discard pile
		PackedScene deckScene = GD.Load<PackedScene>("res://Game/Cards/deck.tscn");
		PackedScene handScene = GD.Load<PackedScene>("res://Game/Cards/hand_manager.tscn");
		PackedScene discardPileScene = GD.Load<PackedScene>("res://Game/Cards/discard_pile.tscn");
		// --------------------------------------------------------

		// --------- Create a deck, hand, and discard pile ---------
		newDeck = deckScene.Instantiate<deck>();
		newDeck.Position = new Vector2(100f, 600f);
		// Deck needs to get card list from inventory
		newDeck.InitCardContainer(DeckSize, testInventory);
		AddChild(newDeck);

		newHand = handScene.Instantiate<hand_manager>();
		newHand.Position = new Vector2(0f, 0f);

		newDiscard = discardPileScene.Instantiate<discard_pile>();
		newDiscard.Position = new Vector2(1180f, 600f);
		newDiscard.InitCardContainer(DeckSize);
		AddChild(newDiscard);
		// --------------------------------------------------------

		// --------- Initial Start Turn ---------
		// Shuffle Deck
		newDeck.Shuffle();
		// Draw New Hand From Deck
		newHand.InitCardContainer(HandSize, newDeck.DrawCards(HandSize));
		AddChild(newHand);
		// combat = DuringTurn
		combat = State.DuringTurn;
		// --------------------------------------

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (combat == State.StartTurn) {
			// Discard Hand (to discard)
			// newHand.EmptyContainer();
			// Shuffle Deck
			// newDeck.Shuffle();
			// Draw New Hand From Deck
			// newHand.AddCard(newDeck.DrawCards(HandSize));
			// combat = DuringTurn
			// combat = State.DuringTurn;
		} else if (combat == State.DuringTurn) {
			// Building Equation
			// Submit Equation
			// Discard Equation (to discard)
			// combat = EndTurn
			combat = State.EndTurn;

		} else if (combat == State.EndTurn) {
			// Calculate Damage
			// Update Entities Health
			// Check For Win/Lose
			// combat = StartTurn
			await ToSignal(GetTree().CreateTimer(5.0f), SceneTreeTimer.SignalName.Timeout);
			combat = State.StartTurn;
			
		}
	}
}
