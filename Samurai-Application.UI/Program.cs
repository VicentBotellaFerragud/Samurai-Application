﻿using Microsoft.EntityFrameworkCore;
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
            //GetBattles();
            //AddBattlesByName("Battle of Nagashino", "Battle of Anegawa");
            //QueryAndUpdateBattles_Disconnected();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiNotTracked(9);
            //Simpler_AddQuoteToExistingSamuraiNotTracked(8);
            //EagerLoadSamuraiWithQuotes();
            ProjectSamuraiWithQuotes();
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

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Vincent",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I am the last samurai!" }
                }
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Tamagochi",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I could never be stronger than Vincent..." },
                    new Quote { Text = "Vincent is the greatest samurai that ever walked this earth" }
                }
            };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "We are nothing but dust in the wind"
            });
            context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "All hope is gone..."
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);
                newContext.SaveChanges();   
            }    
        }

        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "I am really hungry", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samurais = context.Samurais.Include(s => s.Quotes).ToList();
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
                foreach (var quote in samurai.Quotes)
                {
                    Console.WriteLine(quote.Text);
                }
                Console.WriteLine("");
            }

            /*
             * MORE POSSIBILITIES.
             * 
             * var splitQuery = context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
             * var filteredInclude = context.Samurais.Include(s => s.Quotes.Where(q => q.Text.Contains("whatever"))).ToList();
             * var filterPrimaryEntityWithInclude = 
             *      context.Samurais.Where(s => s.Name.Contains("whatever")).Include(s => s.Quotes).FirstOrDefault();                                               
             */
        }

        private static void ProjectSamuraiWithQuotes()
        {
            var somePropsWithQuotes = context.Samurais.Select(s => new { s.Id, s.Name, numberOfQuotes = s.Quotes.Count}).ToList();
            Console.WriteLine("");
            Console.WriteLine("");
            foreach (var samuraiWithQuotes in somePropsWithQuotes)
            {
                Console.WriteLine(samuraiWithQuotes.Id);
                Console.WriteLine(samuraiWithQuotes.Name);
                Console.WriteLine(samuraiWithQuotes.numberOfQuotes);
                Console.WriteLine("");
            }
        }
    }
}
