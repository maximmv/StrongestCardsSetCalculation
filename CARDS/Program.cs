using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CARDS
{
    public enum CardSuit
    {
        Hearts = 3,
        Diamonds = 4,
        Clubs = 5,
        Spades = 6
    }
    public enum CardValue
    {
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }
    public class Card
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }
       

        public void OutputCard(int leftCurPosition)
        {
            string cardValue;
            string space = " ";
            if (Value < CardValue.Jack)
            {
                cardValue = ((int)Value).ToString();
                if (Value == CardValue.Ten)
                    space = "";
            }
            else
            {
                switch (Value)
                {
                    case CardValue.Jack:
                        cardValue = "J";
                        break;
                    case CardValue.Queen:
                        cardValue = "Q";
                        break;
                    case CardValue.King:
                        cardValue = "K";
                        break;
                    case CardValue.Ace:
                        cardValue = "A";
                        break;
                    default:
                        throw new ArgumentException("Unknown card value");
                }
            }            
            if (Suit == (CardSuit)3|| Suit == (CardSuit)4) {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.CursorLeft = leftCurPosition;
            Console.WriteLine($"{(char)9556}{(char)9552}{(char)9552}{(char)9552}{(char)9559}");
            Console.CursorLeft = leftCurPosition;
            Console.WriteLine($"{ (char)9553}{cardValue}{space} {(char)9553}");
            Console.CursorLeft = leftCurPosition;
            Console.WriteLine($"{ (char)9553} {(char)Suit} {(char)9553}");
            Console.CursorLeft = leftCurPosition;
            Console.WriteLine($"{ (char)9553} {space}{cardValue}{(char)9553}");
            Console.CursorLeft = leftCurPosition;
            Console.WriteLine($"{(char)9562}{(char)9552}{(char)9552}{(char)9552}{(char)9565}");
            Console.ResetColor();
        }
    }

    class Deck
    {
        public Card[] Cards { get; set; }
        public Deck()
        {
            this.Cards = new Card[36];
            int tmp = 0;

            foreach (var value in Enum.GetValues(typeof(CardValue)))
            {
                foreach (var suit in Enum.GetValues(typeof(CardSuit)))
                    Cards[tmp++] = (new Card((CardSuit)suit, (CardValue)value));
            }
        }
        public void Mix()
        {
            Random rnd = new Random();
            Card tcard;
            int pos = 0;
            for (int i = 0; i < 36; i++)
            {
                for (int j = 0; j < 36; j++)
                {
                    tcard = this.Cards[j];
                    pos = rnd.Next() % 35;
                    this.Cards[j] = this.Cards[pos];
                    this.Cards[pos] = tcard;
                }
            }
        }
        public Card  GetTrumpCard()
        {
            Random rnd = new Random();
            Card TrumpCard = Cards[rnd.Next() % 35];
            return TrumpCard;
        }
    }
    class Player
    {
        public string Name { get; set; }
        public List<Card> PlayerDeck { get; set; }
        public Player(string name)
        {
            this.Name = name;
            PlayerDeck = new List<Card>();
        }

    }
    class Game
    {
        private Deck deck;
        public List<Player> players;
        Card trumpCard = null;
        int playersQuan;
        public Game()
        {
            deck = new Deck();
            deck.Mix();
            trumpCard=deck.GetTrumpCard();            
            SetPlayers();
            Console.WriteLine("The trump card is:");
            trumpCard.OutputCard(0);
            SetPlayerCards();
            GetCards();
            CalculateStrong();
        }

        public void SetPlayers()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter the players quantity(2 - 6):");

                    playersQuan = int.Parse(Console.ReadLine());
                    if (playersQuan < 2 || playersQuan > 6)
                    {
                        throw new IndexOutOfRangeException("Please set players number from 2 to 6");
                    }
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            players = new List<Player>(playersQuan);
          

            for (int i = 0; i < playersQuan; i++)
            {     
                players.Insert(i, (new Player($"Player #{i+1}")));
            }
            
        }
        public void SetPlayerCards()
        {
            for (int i = 0; i < 36; i++)
            {
                players[i % playersQuan].PlayerDeck.Add(deck.Cards[i]);
            }
        }
        public void GetCards()
        {
            foreach (Player p in players)
            {
                int i = 0;
                Console.WriteLine(p.Name);

                foreach (Card c in p.PlayerDeck)
                {

                    c.OutputCard(i * 6);                    
                    i++;
                    Console.CursorTop -= 5;
                }
                Console.CursorTop += 6;
            }
        }
        public void CalculateStrong()
        {
            List<int> calculation = new List<int>();
            int sumValue = 0;
            int maxSum = 0;
            int playerPos = 0;
            
            foreach (Player p in players)
            {
                sumValue = 0;
                foreach (Card c in p.PlayerDeck)
                {
                    if (c.Suit == trumpCard.Suit)
                    {
                        sumValue +=((int)c.Value + 9);                        
                    }
                    else
                    {
                        sumValue += (int)c.Value;                       
                    }                   
                }
                calculation.Add(sumValue);
                if (sumValue > maxSum)
                {
                    maxSum = sumValue;
                    playerPos = players.IndexOf(p);
                }                
            }
           
            Console.WriteLine("Player #{0} has the strongest set",(playerPos+1));
            Console.WriteLine();
            int i = 0;
            foreach (int sums in calculation)
            {
                Console.WriteLine("Player #{0} has {1} points",++i, sums);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
        }
    }
}
