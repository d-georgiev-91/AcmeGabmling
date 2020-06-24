using System;
using System.Collections.Generic;
using System.Linq;
using AcmeGambling.Symbols;

namespace AcmeGambling
{
    class Program
    {
        static void Main(string[] args)
        {
            var balance = ReadBalance();

            while (balance > 0)
            {
                var steak = ReadSteak(balance);

                var reward = Play(steak);

                if (reward > 0)
                {
                    balance += reward;
                    Console.WriteLine($"You won: {reward}");
                }
                else
                {
                    balance -= steak;
                    Console.WriteLine($"You lost: {steak}");
                }

                Console.WriteLine($"Current balance: {balance}");
                Console.WriteLine(new string('-', Console.BufferWidth));
            }
        }

        private static decimal ReadBalance()
        {
            Console.WriteLine("Please deposit money you want to play with:");
            var balance = decimal.Parse(Console.ReadLine());

            if (balance <= 0)
            {
                throw new ArgumentException("Balance should more than 0");
            }

            return balance;
        }

        private static decimal ReadSteak(decimal balance)
        {
            Console.WriteLine("Enter steak amount:");
            var steak = decimal.Parse(Console.ReadLine());

            if (steak <= 0)
            {
                throw new ArgumentException("Steak should more than 0");
            }

            if (steak > balance)
            {
                throw new ArgumentException("Steak should less or equal to balance");
            }

            return steak;
        }
        private static decimal Play(decimal steak)
        {
            var rowsCount = 4;
            var columnsCount = 3;
            var machine = new Symbol[rowsCount, columnsCount];
            var generator = new RandomSymbolGenerator();
            var possibleSymbols = new List<Symbol> { new Apple(), new Banana(), new Pineapple(), new Wildcard() };
            var winCoefficient = 0m;
            
            Console.WriteLine(new string('-', columnsCount));

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                var matches = new Dictionary<Symbol, int>();

                for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    var symbol = generator.Generate(possibleSymbols);

                    Console.Write(symbol.Character);

                    machine[rowIndex, columnIndex] = symbol;

                    if (!matches.ContainsKey(symbol))
                    {
                        matches.Add(symbol, 0);
                    }

                    matches[symbol]++;
                }

                var mostFrequentSymbol = matches
                    .OrderByDescending(m => m.Value)
                    .First();

                if (mostFrequentSymbol.Value == columnsCount ||
                    mostFrequentSymbol.Value == columnsCount - 1 && matches.Count(s => s.Key is Wildcard) == 1 ||
                    mostFrequentSymbol.Value == 1 && matches.Count(s => s.Key is Wildcard) == columnsCount - 1
                    )
                {
                    winCoefficient += mostFrequentSymbol.Key.WinCoefficient * mostFrequentSymbol.Value;
                }

                Console.WriteLine();
            }
            
            Console.WriteLine(new string('-', columnsCount));

            return winCoefficient * steak;
        }
    }
}
