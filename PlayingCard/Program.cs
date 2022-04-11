using System;
using System.Collections.Generic;

namespace PlayingCard
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table();
            table.PlayToCard();
        }
    }

    class Table
    {
        private Dealer _dealer = new Dealer();
        private Player _player = new Player();

        int first = 1;
        int second = 2;
        int third = 3;
        int fourth = 4;

        private int number;
        private int maxCards = 36;        
        private bool isCorrectNumber = false;

        public void PlayToCard()
        {
            ShowMesage(first);

            _dealer.CreateNewDeck();
            _dealer.ShowDeck();

            ShowMesage(second);

            _dealer.Shuffle(100);
            _dealer.ShowDeck();

            while (isCorrectNumber == false)
            {
                ShowMesage(third);
                
                int.TryParse(Console.ReadLine(), out number);

                isCorrectNumber = (number > 0 && number < maxCards);

                if (isCorrectNumber == false)
                {
                    Console.WriteLine($"некорректные даныне");
                    Console.WriteLine($"необходимо число в диапазоне от 1 до {maxCards - 1}");
                }
            }

            HandOver(number);
            ShowMesage(fourth);
            _player.ShowArm();
        }

        private void ShowMesage(int number)
        {
            switch (number)
            {
                case 1:
                    Console.WriteLine($"Добро пожаловать за Игровой стол.");
                    Console.WriteLine($"Специально для Вас мы распечатали новую колоду:");
                    break;
                case 2:
                    Console.WriteLine($"\nи тщательно перемешали ее тасованием Ханафуда,");
                    Console.WriteLine($"- это когда несколько карт вынимают из любой части колоды и перемещают наверх.");
                    Console.WriteLine($"Поэтому алгоритм тасования колоды будет отличаться от привычного Вам.\n");
                    break;
                case 3:
                    Console.WriteLine($"\nСколько карт желаете взять?:");
                    break;
                case 4:
                    Console.WriteLine($"Ваши карты:");
                    break;
                default:
                    Console.WriteLine($"внештатная ситуация, обратитесь к разработчику");
                    break;
            }
        }

        private void HandOver(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Card card = _dealer.GetCard();
                _dealer.RemoveCard(card);
                _player.TakeCard(card);
            }            
        }
    }

    class Dealer
    {
        private static Random _random;

        private Deck _deck;

        static Dealer()
        {
            _random = new Random();
        }

        public void CreateNewDeck()
        {
            _deck = new Deck();

            string[] ranks = new string[] { "6", "7", "8", "9", "10", "В", "К", "Д", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < ranks.Length; i++)
            {
                for (int j = 0; j < suits.Length; j++)
                {                    
                    _deck.AddNewCard(new Card(ranks[i], suits[j]));
                }
            }
        }
        public void Shuffle(int quantity)
        {
            List<Card> deck = (List<Card>)_deck.Cards;

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

        public void ShowDeck()
        {
            _deck.ShowCards();
            Console.WriteLine();
        }

        public Card GetCard()
        {
            return _deck.GetCard();
        }

        public void RemoveCard(Card card)
        {
            _deck.RemoveCard(card);
        }
    }

    class Player
    {
        private Deck _arm;

        public Player()
        {
            _arm = new Deck();
        }

        public void ShowArm()
        {
            _arm.ShowCards();
            Console.WriteLine();
        }

        public void TakeCard(Card card)
        {
            _arm.AddNewCard(card);
        }
    }

    class Deck
    {
        private int firstIndex = 0;

        private List<Card> _cards;
        public IReadOnlyList<Card> Cards => _cards;

        public Deck()
        {
            _cards = new List<Card>();
        }

        public void ShowCards()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ToDisplay();
            }
        }

        public void AddNewCard(Card card)
        {
            _cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        public Card GetCard()
        {            
            Card card = null;

            if (_cards.Count > 0)
                card = _cards[firstIndex];

            return card;
        }
    }

    class Card
    {
        public string Rank { get; private set; }
        public string Suite { get; private set; }

        public Card(string rank, string suite)
        {
            Rank = rank;
            Suite = suite;
        }

        public void ToDisplay()
        {
            Console.ForegroundColor = SetTextColor(Suite);
            Console.Write($"{Rank}{Suite} ");            
            Console.ForegroundColor = ConsoleColor.White;
        }

        private ConsoleColor SetTextColor(string suite)
        {
            return (suite.Contains("♥") || suite.Contains("♦")) ? ConsoleColor.Red : ConsoleColor.DarkGray;
        }
    }
}
