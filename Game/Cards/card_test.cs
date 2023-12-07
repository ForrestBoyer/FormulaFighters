using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
	private Godot.Collections.Array<new_card> testInventory;
	private new_card testCard;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/new_card.tscn");
		// Test inventory
		testInventory = new Godot.Collections.Array<new_card>();
		for(int i = 0; i < 21; i++) {
			if(i % 2 == 0) {
				testCard = cardScene.Instantiate<new_card>();
				testCard.InitCard(2, i);
				testInventory.Insert(i, testCard);
			} else {
				testCard = cardScene.Instantiate<new_card>();
				testCard.InitCard("+", i);
				testInventory.Insert(i, testCard);
			}
		}
		GD.Print("Inventory Size: ", testInventory.Count());

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
		newDeck.InitCardContainer(testInventory);
		AddChild(newDeck);

		newHand = handScene.Instantiate<hand_manager>();
		// newHand.Position = new Vector2(408f, 600f);

		newDiscard = discardPileScene.Instantiate<discard_pile>();
		newDiscard.Position = new Vector2(1180f, 600f);
		newDiscard.InitCardContainer();
		AddChild(newDiscard);
		// --------------------------------------------------------

		// --------- Initial Start Turn ---------
		// Shuffle Deck
		newDeck.ShuffleCards();
		// Draw New Hand From Deck
		newHand.InitCardContainer(newDeck.DrawCards(HandSize));
		AddChild(newHand);
		newHand.updateHand();
		// combat = DuringTurn
		combat = State.DuringTurn;
		// --------------------------------------

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("quit")) {
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("right_click")) {
			combat = State.StartTurn;
		}
		if (combat == State.StartTurn) {
			// Discard Hand (to discard)
			// newHand.EmptyContainer();
			// Shuffle Deck
			newDeck.ShuffleCards();
			// Draw New Hand From Deck
			newHand.ReplaceCards(newDeck.DrawCards(HandSize));
			// Update Hand
			newHand.updateHand();
			GD.Print("Update Hand In Process");
			// combat = DuringTurn
			combat = State.DuringTurn;
		} else if (combat == State.DuringTurn) {
			// Building Equation
			// Submit Equation
			// Discard Equation (to discard)
			// combat = EndTurn
			// combat = State.EndTurn;

		} else if (combat == State.EndTurn) {
			// Calculate Damage
			// Update Entities Health
			// Check For Win/Lose
			// combat = StartTurn
			// combat = State.StartTurn;
			
		}
	}
}
