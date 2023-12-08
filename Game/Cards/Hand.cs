using Godot;

public partial class Hand : CardContainer
{
	private Vector2 leftmostCardPosition;
	private PackedScene cardScene;
	private Card card;

	public int MaxHandSize { get; set; }

	// Called whenever cards in hand are changed and need to be redrawn
	public void UpdateHand()
	{
		PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/card.tscn");
		leftmostCardPosition = new Vector2(408f, 600f);

		foreach (Card card in GetChildren()) 
		{
			RemoveChild(card);
			card.QueueFree();
		}
		
		// for each card in cardList
		for(int i = 0; i < size; i++) 
		{
			if (Cards[i].CardType == CardType.Number) 
			{
				card = cardScene.Instantiate<Card>();
				card.InitCard(Cards[i].IntVal, i);
				card.HomePosition = leftmostCardPosition;
				card.MoveTo(card.HomePosition);
				AddChild(card);
			} 
			else if (Cards[i].CardType == CardType.Operator) 
			{
				card = cardScene.Instantiate<Card>();
				card.InitCard(Cards[i].OpVal, i);
				card.HomePosition = leftmostCardPosition;
				card.MoveTo(card.HomePosition);
				AddChild(card);
			}
			leftmostCardPosition.X += 80f;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
