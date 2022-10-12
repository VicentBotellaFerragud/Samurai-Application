using Samurai_Application.Data;
using Samurai_Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Samurai_Application.UI
{
    class Program
    {
        private static SamuraiContext context = new SamuraiContext();  

        private static void Main(string[] args)
        {
            context.Database.EnsureCreated();
            GetSamurais("Before adding Samurai");
            AddSamurai();
            GetSamurais("After adding Samurai");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamurai()
        {
            var newSamurai = new Samurai { Name = "ElJuli" };
            context.Samurais.Add(newSamurai);
            context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is --> {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
