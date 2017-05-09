using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using EFSamurai.Data;
using EFSamurai.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFSamurai.App
{
    class Program
    {

        private static bool _keepGoing = true;
        static void Main(string[] args)
        {
            while (_keepGoing)
            {

                Menu();
            }
        }

        private static void Menu()
        {
            var menu = new Dictionary<int, Action>
            {
                { 1, AddOneSamurai },
                { 2, AddSomeSamurais },
                { 3, Quit},
                { 4, ListAllSamuraiNames },
                { 5, ListAllSamuraiNamesOrderByName },
                { 6, ListAllSamuraiNamesOrderByDescendingId },
                { 7, ClearDatabase },
                { 8, FindSamuraiWithRealName },
                { 9, ListQuotesOfType },
                { 10, ListAllBattles }
            };
            Console.WriteLine("1: Add one samurai\n2: Add some samurais\n3: Quit\n4: List all samurais" +
                              "\n5: List all samurais, order by name\n6: List all samurais, order by descending id\n7: Clear database" +
                              "\n8: Find samurai by real name\n9: List quotes of type\n10: List battles");
            menu[int.Parse(Console.ReadLine())].Invoke();
        }

        private static void FindSamuraiWithRealName()
        {
            string name = GetString("Name");
            var samuraisFound = new List<string>();
            using (var content = new SamuraiContext())
            {
                samuraisFound = content.Samurais.Where(x => x.Name == name).Select(x => x.Name).ToList();
            }
            foreach (var samurai in samuraisFound)
            {
                Console.WriteLine(samurai);
            }
            if (samuraisFound.Count == 0)
                Console.WriteLine($"No samurais by the name {name} was found");
        }

        private static void ListQuotesOfType()
        {
            var quoteType = GetQuoteType();
            var quoteList = new List<Quote>();

            using (var context = new SamuraiContext())
            {
                quoteList = context.Samurais.SelectMany(x => x.Quotes.Where(y => y.Type == quoteType)).ToList();
            }
            foreach (var quote in quoteList)
            {
                Console.WriteLine($"\"{quote.QuoteText}\"");
            }
            if (quoteList.Count == 0)
                Console.WriteLine($"No quote of type {quoteType} was found");
        }

        private static void ListAllBattles()
        {
            var startDate = GetDate("Start date");
            var endDate = GetDate("End date");
            var isBrutal = GetNullableBool("Brutal battle?");
            var battlesFound = new List<Battle>();

            using (var context = new SamuraiContext())
            {
                if (isBrutal != null)
                {
                    battlesFound = context.Battles.Where(x => x.IsBrutal == isBrutal && x.StartDate > startDate &&
                                                              x.StartDate < endDate)
                        .ToList();
                }
                else
                {
                    battlesFound = context.Battles.Where(x => x.StartDate > startDate &&
                                                              x.StartDate < endDate)
                        .ToList();
                }
            }
            foreach (var battle in battlesFound)
            {
                Console.WriteLine(battle.Name);
            }
            if(battlesFound.Count == 0)
                Console.WriteLine("No matching battles were found.");
        }

        private static void ClearDatabase()
        {
            using (var context = new SamuraiContext())
            {
                context.Database.ExecuteSqlCommand("delete from BattleEvent");
                context.Database.ExecuteSqlCommand("delete from Quote");
                context.Database.ExecuteSqlCommand("delete from Samurais");
                context.Database.ExecuteSqlCommand("delete from Battles");
                context.Database.ExecuteSqlCommand("delete from BattleLog");
                context.Database.ExecuteSqlCommand("delete from SamuraiBattles");
                context.Database.ExecuteSqlCommand("delete from SecretIdentity");

                context.SaveChanges();

                Console.WriteLine("All data deleted!");
            }
        }

        public static void ListAllSamuraiNames()
        {
            using (var context = new SamuraiContext())
            {
                foreach (var samurai in context.Samurais)
                {
                    Console.WriteLine(samurai.Name);
                }
            }
        }

        public static void ListAllSamuraiNamesOrderByName()
        {
            using (var context = new SamuraiContext())
            {
                var orderedList = context.Samurais.OrderBy(x => x.Name);
                foreach (var samurai in orderedList)
                {
                    Console.WriteLine(samurai.Name);
                }
            }
        }

        public static void ListAllSamuraiNamesOrderByDescendingId()
        {
            using (var context = new SamuraiContext())
            {
                var orderedList = context.Samurais.OrderByDescending(x => x.Id);

                foreach (var samurai in orderedList)
                {
                    Console.WriteLine(samurai.Name);
                }
            }
        }

        private static void Quit()
        {
            _keepGoing = false;
        }

        private static void AddSomeSamurais()
        {
            var samurais = new List<Samurai>();
            while (true)
            {
                string name = GetString("Name");
                SecretIdentity secretIdentity = GetSecretIdentity();
                int age = GetAge();
                bool hasKatana = GetBool("Katana");

                samurais.Add(new Samurai()
                {
                    Name = name,
                    SecretIdentity = secretIdentity,
                    Age = age,
                    HasKatana = hasKatana
                });

                Console.Write("Add another samurai? y/n: ");
                var answer = Console.ReadLine();
                if (answer.ToLower() == "n")
                {
                    Menu();
                    break;
                }
                if (answer.ToLower() == "y")
                    continue;
            }

            using (var context = new SamuraiContext())
            {
                context.AddRange(samurais);
                context.SaveChanges();
            }
        }

        private static bool GetBool(string type)
        {
            while (true)
            {
                Console.Write($"{type}? y/n: ");
                var katanaAnswer = Console.ReadLine();
                if (katanaAnswer.ToLower() == "n")
                {
                    return false;
                }

                if (katanaAnswer.ToLower() == "y")
                {
                    return true;
                }
            }
        }

        private static bool? GetNullableBool(string type)
        {
            while (true)
            {
                Console.Write($"{type}? y/n (leave blank if null): ");
                var katanaAnswer = Console.ReadLine();
                if (katanaAnswer.ToLower() == "n")
                    return false;

                if (katanaAnswer.ToLower() == "y")
                    return true;


                if (string.IsNullOrWhiteSpace(katanaAnswer))
                    return null;
            }
        }

        private static List<Quote> GetQuote()
        {
            var keepAddingQuote = true;
            var quotes = new List<Quote>();
            var quote = new Quote();
            while (keepAddingQuote)
            {
                quote.QuoteText = GetString("Quote");
                quote.Type = GetQuoteType();
                quotes.Add(quote);
                keepAddingQuote = GetBool("Add another quote");
            }
            return quotes;
        }

        private static QuoteType GetQuoteType()
        {
            while (true)
            {
                Console.WriteLine("What type of quote is it?");
                Console.WriteLine($"1: {QuoteType.Awesome}");
                Console.WriteLine($"2: {QuoteType.Cheesy}");
                Console.WriteLine($"3: {QuoteType.Lame}");

                var answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        return QuoteType.Awesome;
                    case "2":
                        return QuoteType.Cheesy;
                    case "3":
                        return QuoteType.Lame;
                    default:
                        Console.WriteLine("Invalid answer, try again");
                        continue;
                }
            }

        }

        private static int GetAge()
        {
            while (true)
            {
                Console.Write("Age: ");
                try
                {
                    var age = int.Parse(Console.ReadLine());
                    return age;
                }
                catch
                {
                    continue;
                }
            }
        }

        private static Battle GetBattle()
        {
            var battle = new Battle
            {
                Name = GetString("Battle name"),
                Description = GetString("Battle description"),
                StartDate = GetDate("Battle start date"),
                EndDate = GetDate("Battle end date"),
                IsBrutal = GetBool("Was the battle brutal"),
                BattleLog = GetBattleLog()
            };

            return battle;
        }

        private static BattleLog GetBattleLog()
        {
            var battleLog = new BattleLog
            {
                Name = GetString("Battle log name"),
                BattleEvents = GetBattleEvents()
            };

            return battleLog;
        }

        private static ICollection<BattleEvent> GetBattleEvents()
        {
            bool keepAddingEvents = true;
            var battleEvents = new List<BattleEvent>();
            var battleEvent = new BattleEvent();
            while (keepAddingEvents)
            {
                battleEvent.Description = GetString("Battle event description");
                battleEvent.Summary = GetString("Battle event summary");
                battleEvent.EventTime = GetDate("Event time");
                battleEvents.Add(battleEvent);
                keepAddingEvents = GetBool("Add another event");
            }
            return battleEvents;

        }

        private static DateTime GetDate(string type)
        {
            Console.Write($"{type}: ");

            return Convert.ToDateTime(Console.ReadLine());
        }

        private static SecretIdentity GetSecretIdentity()
        {
            Console.Write("Secret Identity: ");
            var secretIdentity = Console.ReadLine();
            return new SecretIdentity() { Alias = secretIdentity };
        }

        private static string GetString(string type)
        {
            Console.Write($"{type}: ");
            var name = Console.ReadLine();
            return name;
        }

        private static void AddOneSamurai()
        {
            var name = GetString("Name");
            var age = GetAge();
            var secretIdentity = GetSecretIdentity();
            var hasKatana = GetBool("Katana");
            var quotes = GetQuote();

            var samurai = new Samurai()
            {
                Age = age,
                Name = name,
                SecretIdentity = secretIdentity,
                HasKatana = hasKatana,
                Quotes = quotes
            };

            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);

                var samuraiBattle = new SamuraiBattle()
                {
                    Battle = GetBattle(),
                    Samurai = samurai
                };
                context.SamuraiBattles.Add(samuraiBattle);
                context.Battles.Add(samuraiBattle.Battle);
                context.SaveChanges();
            }
        }
    }
}