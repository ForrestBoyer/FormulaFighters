using Godot;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

public partial class card_container : Node2D
{
	protected List<new_card> cardList = new List<new_card>();
	protected int size;

	// Initializer for card container with empty card list
	public void InitCardContainer()
	{
		size = cardList.Count;
	}

	// Initializer for card container with list of cards
	public void InitCardContainer(List<new_card> Cards)
	{
		cardList.AddRange(Cards);
		size = cardList.Count;
	}

	// Add Card to End
	public void AddCard(new_card Card)
	{
		cardList.Append(Card);
		size = cardList.Count;
	}

	// Replace Cards
	public void ReplaceCards(List<new_card> Cards){
		cardList.Clear();
		cardList = Cards;
		size = cardList.Count;
	}

	// Remove Card from end
	public new_card RemoveCard()
	{		
		new_card ret;
		ret = cardList[cardList.Count - 1];
		cardList.RemoveAt(cardList.Count - 1);
		size = cardList.Count;
		return ret;
	}

	// Empty Container
	public void EmptyContainer() 
	{
		cardList.Clear();
		size = cardList.Count();
	}

	// Shuffle
	public void ShuffleCards()
	{
		foreach (var card in cardList)
		{
			if (card.cardType == new_card.CardType.Number)
			{
				GD.Print(card.intVal);
			}
			else
			{
				GD.Print(card.opVal);
			}
		}
		cardList.Shuffle();
		foreach (var card in cardList)
		{
			if (card.cardType == new_card.CardType.Number)
			{
				GD.Print(card.intVal);
			}
			else
			{
				GD.Print(card.opVal);
			}
		}
	}

	// Draw
	public List<new_card> DrawCards(int Num)
	{
		List<new_card> ret;

		if (cardList.Count < Num) 
		{
			return null;
		}

		ret = cardList.Take(Num).ToList();
		
		return ret;
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