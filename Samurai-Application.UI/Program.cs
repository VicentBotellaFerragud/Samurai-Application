using Microsoft.EntityFrameworkCore;
using Samurai_Application.Data;
using Samurai_Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Samurai_Application.UI
{
    class Program
    {
        private static SamuraiContext context = new SamuraiContext();
        private static SamuraiContextNoTracking contextNT = new SamuraiContextNoTracking(); //Not used yet.

        private static void Main(string[] args)
        {
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
            //GetSamurais();
            //GetSamuraiByName("Takashi");
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //RetrieveAndDeleteSamurai(2);
            //AddBattlesByName("Battle of Nagashino", "Battle of Anegawa");
            QueryAndUpdateBattles_Disconnected();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddSamuraisByName(params string[] names)
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
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"Samurai count is --> {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void GetSamuraiByName(string name)
        {
            var samurai = context.Samurais.FirstOrDefault(s => s.Name == name);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(samurai.Name);
        }

        private static void QueryFilters()
        {
            var samurais = context.Samurais.Where(s => s.Name == "Okamoto").ToList();
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }

            /*
             * ANOTHER WAY:
             * 
             * var filter = "Oka%";
             * var samurais = context.Samurais.Where(s => EF.Functions.Like(s.Name, filter)).ToList();
             */
        }

        private static void QueryAggregates()
        {
            var name = "Shimada";
            var samurai = context.Samurais.FirstOrDefault(s => s.Name == name);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(samurai.Name);

            /*
             * ANOTHER WAY:
             * 
             * var samurai = context.Samurais.Find(2);
             */
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Name += "El ";
            context.SaveChanges();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(samurai.Name);
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += " the greatest");
            context.SaveChanges();
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void RetrieveAndDeleteSamurai(int id)
        {
            var samurai = context.Samurais.Find(id);
            context.Samurais.Remove(samurai);
            context.SaveChanges();
            GetSamurais();
        }

        private static void AddBattlesByName(params string[] names)
        {
            foreach (var name in names)
            {
                context.Add(new Battle { Name = name });
            }
            context.SaveChanges();
            GetBattles();
        }

        private static void GetBattles()
        {
            var battles = context.Battles.TagWith("ConsoleApp.Program.GetBattles method").ToList();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"Battles count is --> {battles.Count}");
            foreach (var battle in battles)
            {
                Console.WriteLine(battle.Name);
            }
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconectedBattles = context.Battles.ToList();
            } //Context1 is disposed.

            disconectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1572, 01, 01);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconectedBattles);
                context2.SaveChanges();
            }
        }
    }
}
