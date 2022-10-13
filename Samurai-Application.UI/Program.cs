using Microsoft.EntityFrameworkCore;
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
            AddSamurai("Julie", "Sampson");
            GetSamurais();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamurai(params string[] names)
        {
            foreach (var name in names)
            {
                context.Add(new Samurai { Name = name });
            }
            context.SaveChanges();
        }

        private static void GetSamurais()
        {
            var samurais = context.Samurais.TagWith("ConsoleApp.Program.GetSamurais method").ToList();
            Console.WriteLine($"Samurai count is --> {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
