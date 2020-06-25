using System;
using System.Threading.Tasks;

namespace AcmeGambling
{
    public class App
    {
        public async Task Run(string[] args)
        {
            Console.WriteLine("Hello world!");

            await Task.CompletedTask;
        }
    }
}