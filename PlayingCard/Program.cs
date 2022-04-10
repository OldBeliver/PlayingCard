using System;
using System.Collections.Generic;

namespace PlayingCard
{
    class Program
    {
        static void Main(string[] args)
        {
            Dealer dealer = new Dealer();
            int number = 100;

            Console.WriteLine($"дилер распечатал новую колоду карт");
            dealer.CreateNewDeck();
            dealer.ShowDeck();


            dealer.Shuffle(number);
            Console.WriteLine($"\n... и перетасовал ее {number} раз");
            dealer.ShowDeck();

            Console.WriteLine();

            Console.WriteLine($"Сколько карт Вы желаете взять?: ");
            int userInput = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < userInput; i++)
            {
                dealer.HandOver();
            }

            Console.WriteLine($"Ваши карты");
            dealer.ShowArm();
            Console.WriteLine();
        }
    }

    class Dealer
    {
        private Deck _deck;
        private Player _player;
        private static Random _random;

        static Dealer()
        {
            _random = new Random();
        }

        public Dealer()
        {
            _player = new Player();
        }

        public Deck CreateNewDeck()
        {
            _deck = new Deck();
            string[] name = new string[] { "6", "7", "8", "9", "10", "В", "К", "Д", "Т" };
            string[] suit = new string[] { "♠", "♣", "♦", "♥" };

            for (int j = 0; j < name.Length; j++)
            {
                for (int i = 0; i < suit.Length; i++)
                {
                    string card = (name[j] + suit[i]);
                    _deck.AddNewCard(new Card(card));
                }
            }

            return _deck;
        }

        public void ShowDeck()
        {
            _deck.ShowInfo();
        }

        public void ShowArm()
        {
            _player.ShowArm();
        }

        public void Shuffle(int quantity)
        {            
            List<Card> deck = _deck.Cards;

            for (int i = 0; i < quantity; i++)
            {
                int depth = _random.Next(deck.Count);
                int range = _random.Next(1, deck.Count - depth);

                Card[] tempSlice = new Card[range];

                deck.CopyTo(depth, tempSlice, 0, range);
                deck.RemoveRange(depth, range);
                deck.InsertRange(0, tempSlice);
            }
        }

        public void HandOver()
        {
            Card card = _deck.GetCard();
            _deck.RemoveCard(card);
            _player.TakeCard(card);
        }
    }

    class Player
    {
        private Deck _arm;

        public Player()
        {
            _arm = new Deck();
        }

        public void TakeCard(Card card)
        {
            _arm.AddNewCard(card);
        }

        public void ShowArm()
        {
            _arm.ShowInfo();
        }
    }

    class Deck
    {        
        public List<Card> Cards { get; private set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public void ShowInfo()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].ToDysplay();
            }
        }

        public void AddNewCard(Card card)
        {
            Cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public Card GetCard()
        {
            int lastIndex = 0;

            if (Cards.Count >= 0)
            {
                lastIndex = Cards.Count - 1;
            }

            return Cards[lastIndex];
        }
    }

    class Card
    {
        public string Name { get; private set; }

        public Card(string name)
        {
            Name = name;
        }

        public void ToDysplay()
        {
            Console.ForegroundColor = Colored(Name);
            Console.Write($"{Name} ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private ConsoleColor Colored(string name)
        {
            return (name.Contains("♥") || name.Contains("♦")) ? ConsoleColor.Red : ConsoleColor.White;
        }
    }
}
