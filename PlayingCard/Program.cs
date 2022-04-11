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

        private int _shuffleQuantity = 100;
        private int _number;        
        private bool _isCorrectNumber = false;

        public void PlayToCard()
        {
            Console.WriteLine($"Добро пожаловать за Игровой стол.");
            Console.WriteLine($"Специально для Вас мы распечатали новую колоду:");

            _dealer.CreateNewDeck();
            _dealer.ShowDeck();

            Console.WriteLine($"\nи тщательно перемешали ее тасованием Ханафуда,");
            Console.WriteLine($"- это когда несколько карт вынимают из любой части колоды и перемещают наверх.");
            Console.WriteLine($"Поэтому алгоритм тасования колоды будет отличаться от привычного Вам.\n");

            _dealer.Shuffle(_shuffleQuantity);
            _dealer.ShowDeck();

            while (_isCorrectNumber == false)
            {
                int _maxCards = _dealer.GetCount();

                Console.WriteLine($"\nСколько карт желаете взять?:");

                int.TryParse(Console.ReadLine(), out _number);

                _isCorrectNumber = (_number > 0 && _number < _maxCards);

                if (_isCorrectNumber == false)
                {
                    Console.WriteLine($"некорректные данные");
                    Console.WriteLine($"необходимо число в диапазоне от 1 до {_maxCards - 1}");
                }
            }

            HandOver(_number);

            Console.WriteLine($"Ваши карты:");
            _player.ShowArm();
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
        private Deck _deck;

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
            for (int i = 0; i < quantity; i++)
            {
                _deck.Shuffle();
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

        public int GetCount()
        {
            return _deck.Cards.Count;
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
        private static Random _random;

        private int firstIndex = 0;

        private List<Card> _cards;
        public IReadOnlyList<Card> Cards => _cards;

        static Deck()
        {
            _random = new Random();
        }

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

        public void Shuffle()
        {
            int depth = _random.Next(_cards.Count);
            int range = _random.Next(1, _cards.Count - depth);

            Card[] tempSlice = new Card[range];

            _cards.CopyTo(depth, tempSlice, 0, range);
            _cards.RemoveRange(depth, range);
            _cards.InsertRange(0, tempSlice);
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
