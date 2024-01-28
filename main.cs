using System;
using System.Collections.Generic;
using System.Linq;

class Card : IComparable<Card>
{
    public string Suit { get; set; }
    public string Rank { get; set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int CompareTo(Card other)
    {
        return String.Compare(Rank, other.Rank);
    }
}

class Player
{
    public string Name { get; set; }
    public List<Card> Hand { get; set; }

    public Player(string name)
    {
        Name = name;
        Hand = new List<Card>();
    }

    public void PrintHand()
    {
        Console.WriteLine($"{Name}'s hand:");
        foreach (var card in Hand)
        {
            Console.WriteLine($"{card.Rank} of {card.Suit}");
        }
    }
}

class Game
{
    public List<Player> Players { get; set; }
    public List<Card> Deck { get; set; }

    public Game(List<string> playerNames)
    {
        Players = new List<Player>();
        foreach (var playerName in playerNames)
        {
            Players.Add(new Player(playerName));
        }

        InitializeDeck();
        ShuffleDeck();
        DealCards();
    }

    private void InitializeDeck()
    {
        Deck = new List<Card>();
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                Deck.Add(new Card(suit, rank));
            }
        }
    }

    private void ShuffleDeck()
    {
        Random random = new Random();
        Deck = Deck.OrderBy(card => random.Next()).ToList();
    }

    private void DealCards()
    {
        int numberOfPlayers = Players.Count;
        int cardsPerPlayer = Deck.Count / numberOfPlayers;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Players[i].Hand.AddRange(Deck.GetRange(i * cardsPerPlayer, cardsPerPlayer));
        }
    }

    private int CompareCards(Card card1, Card card2)
    {
        return String.Compare(card1.Rank, card2.Rank);
    }

    private Player DetermineRoundWinner(List<Card> cardsInPlay)
    {
        return Players.First(player => player.Hand.Contains(cardsInPlay.Max()));
    }

    public void Play()
    {
        while (Players.All(player => player.Hand.Count > 0))
        {
            List<Card> cardsInPlay = Players.Select(player => player.Hand.First()).ToList();

            Console.WriteLine("Cards in play:");
            foreach (var cardInPlay in cardsInPlay)
            {
                Console.WriteLine($"{cardInPlay.Rank} of {cardInPlay.Suit}");
            }

            Player roundWinner = DetermineRoundWinner(cardsInPlay);
            Console.WriteLine($"{roundWinner.Name} wins the round!");

            roundWinner.Hand.AddRange(cardsInPlay);

            foreach (var player in Players)
            {
                player.Hand.Remove(cardsInPlay.First());
            }

            foreach (var player in Players)
            {
                player.PrintHand();
            }

            Console.WriteLine("Press Enter to continue to the next round...");
            Console.ReadLine();
        }

        Player gameWinner = Players.First(player => player.Hand.Count == Deck.Count);
        Console.WriteLine($"{gameWinner.Name} wins the game!");
    }
}

class Program
{
    static void Main()
    {
        List<string> playerNames = new List<string> { "Player1", "Player2" };
        Game game = new Game(playerNames);

        game.Play();
    }
}
