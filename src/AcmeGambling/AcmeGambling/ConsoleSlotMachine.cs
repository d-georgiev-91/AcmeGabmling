using System;
using System.Threading.Tasks;
using AcmeGambling.Services;

namespace AcmeGambling
{
    public class ConsoleSlotMachine
    {
        private readonly ISlotMachine _slotMachine;

        public ConsoleSlotMachine(ISlotMachine slotMachine)
        {
            _slotMachine = slotMachine;
        }

        public async Task Run(string[] args)
        {
            var deposit = ReadDeposit();
            _slotMachine.Deposit(deposit);

            while (_slotMachine.Balance > 0)
            {
                var steak = ReadSteak();

                var spinResult = _slotMachine.Spin(steak);
                PrintSpinResult(spinResult);
            }

            await Task.CompletedTask;
        }

        private decimal ReadDeposit()
        {
            Console.Write("Please deposit money you would like to play with: ");
            var deposit = decimal.Parse(Console.ReadLine());

            return deposit;
        }

        private decimal ReadSteak()
        {
            Console.Write("Enter steak amount: ");
            var deposit = decimal.Parse(Console.ReadLine());

            return deposit;
        }

        private void PrintSpinResult(SlotMachineSpinResult spinResult)
        {
            for (var reelIndex = 0; reelIndex < spinResult.Reels.GetLength(0); reelIndex++)
            {
                for (var symbolReelIndex = 0; symbolReelIndex < spinResult.Reels.GetLength(1); symbolReelIndex++)
                {
                    Console.Write(spinResult.Reels[reelIndex, symbolReelIndex].Character);
                }

                Console.WriteLine();
            }

            if (spinResult.AmountWon > 0)
            {
                Console.WriteLine($"You won {spinResult.AmountWon}");
            }

            Console.WriteLine($"Current balance is {_slotMachine.Balance}");
        }
    }
}