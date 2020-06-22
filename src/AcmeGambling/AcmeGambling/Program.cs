using System;

namespace AcmeGambling
{
    class Program
    {
        static void Main(string[] args)
        {
            var balance = ReadBalance();
            Console.WriteLine(balance);

            while (balance >= 0)
            {
                // TODO: input steak till its valid
                var steak = ReadSteak(balance);
                
                // TODO: Play
                Console.WriteLine(steak);
            }

            Console.WriteLine(balance);
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

    }
}
