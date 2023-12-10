// REWARD SCREEN
// Screen will display 3 different cards and a "None" Button
// After a card is clicked, a signal containing the card will be emitted
// After the "None" button is clicked, a different signal will be emitted
// NOTE: COPY THE CARD FROM THE SIGNAL OR CHANGE ITS PARENT BEFORE UNLOADING REWARD SCREEN
using Godot;
using System;

public partial class rewards : Node2D
{
    // Signal for when a card is chosen
    [Signal]
    public delegate void CardChosenEventHandler(Card chosen);
    // Signal for when the player chooses not to take a card
    [Signal]
    public delegate void NoCardChosenEventHandler();
    // Center of Screen
    private Vector2 center = new Vector2(640, 360);
    // Reward Cards
    private Card card1;
    private Card card2;
    private Card card3;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Position = center;
        // Get Child Cards
        card1 = GetNode<Card>("rewardCard1");
        card2 = GetNode<Card>("rewardCard2");
        card3 = GetNode<Card>("rewardCard3");

        // Initialize Cards First Time
        card1.InitCardRandom();
        card2.InitCardRandom();
        card3.InitCardRandom();
        // Ensure None are the Same
        while(card1.CardType == card2.CardType && card1.OpVal == card2.OpVal && card1.IntVal == card2.IntVal){
            card2.InitCardRandom();
        }
        while((card1.CardType == card3.CardType && card1.OpVal == card3.OpVal && card1.IntVal == card3.IntVal) ||
              (card2.CardType == card3.CardType && card2.OpVal == card3.OpVal && card2.IntVal == card3.IntVal)){
            card3.InitCardRandom();
        }
        // Refresh Card Graphic
        card1.Refresh();
        card2.Refresh();
        card3.Refresh();

        // Make it so Cards Cannot be Moved
        card1._isDraggable = false;
        card2._isDraggable = false;
        card3._isDraggable = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    // When a card is chosen, emit a signal containing selected card
    public void _on_reward_card_clicked(int cardNum){
        GD.Print(cardNum);
        switch(cardNum){
        case 1:
            EmitSignal(SignalName.CardChosen, card1);
            break;
        case 2:
            EmitSignal(SignalName.CardChosen, card2);
            break;
        case 3:
            EmitSignal(SignalName.CardChosen, card3);
            break;
        }
    }

    public void _on_button_pressed(){
        EmitSignal(SignalName.NoCardChosen);
    }
}
