using Godot;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

public partial class CardContainer : Node2D
{
	public List<Card> Cards = new List<Card>();

	public int size;
    public Vector2 centerScreen = new Vector2(640, 360);
    protected int leftSideOffset = 100;
    protected int spaceHorizontal = 100;
    protected int spaceVertical = 75;
    protected int rowSize = 12;

    protected Card CopyCard(Card c){
        PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/card.tscn");
        Card newCard = cardScene.Instantiate<Card>();
        newCard.CardType = c.CardType;
        newCard.IntVal = c.IntVal;
        newCard.OpVal = c.OpVal;
        AddChild(newCard);
        return newCard;
    }
	// Initializer for card container with list of cards
	public void SetCards(List<Card> cards)
	{
        Empty();
        
		Cards = new List<Card>();
        foreach(Card c in cards){
            Card newCard = CopyCard(c);
            newCard.Visible = false;
            newCard._isDraggable = false;
            Cards.Add(newCard);
        }
	}

	// Add Card to End
	public void AddCard(Card card)
	{
        Card newCard = CopyCard(card);
        newCard.Visible = false;
        newCard._isDraggable = false;
		Cards.Add(newCard);
		size = Cards.Count;
	}

	public void AddCards(List<Card> cards)
	{
		foreach (Card card in cards)
		{
		    Card newCard = CopyCard(card);
            newCard.Visible = false;
            newCard._isDraggable = false;
            Cards.Add(newCard);
		}

		size = Cards.Count;
	}

	// Empty Container
	public void Empty() 
	{
        foreach (Card card in Cards){
            card.QueueFree();
        }
		Cards.Clear();
		size = Cards.Count;
	}

	// Shuffle
	public void ShuffleCards()
	{
		Cards.Shuffle();
	}

	// Draw
	public List<Card> DrawCards(int Num)
	{
		List<Card> ret;

		if (Cards.Count < Num) 
		{
			return null;
		}

		ret = Cards.Take(Num).ToList();

		foreach (Card card in ret)
		{
			Cards.Remove(card);
		}

		size = Cards.Count;
		
		return ret;
	}

    protected void ShowCards()
    {
        
        GetNode<ColorRect>("ColorRect").Visible = false;
        GetNode<Label>("CloseX").Visible = true;
        int i = 0;
        foreach(Card c in Cards)
        {
            c.MoveTo(new Vector2(i % rowSize * spaceHorizontal + leftSideOffset, 
                                 i / rowSize * spaceHorizontal + leftSideOffset));
            i++;
            c.Visible = true;
        }
    }

    protected void HideCards()
    {
        GetNode<ColorRect>("ColorRect").Visible = true;
        GetNode<Label>("CloseX").Visible = false;
        foreach(Card c in Cards)
        {
            c.Visible = false;
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
