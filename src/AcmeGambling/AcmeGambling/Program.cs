using System;

namespace AcmeGambling
{
    class Program
    {
        static void Main(string[] args)
        {
            var balance = ReadBalance();
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
    }
}
