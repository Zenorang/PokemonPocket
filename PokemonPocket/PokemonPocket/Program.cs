//Wong Shao Yang 242948F

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace PokemonPocket
{
    class Program
    {

        static void Main(string[] args)
        {

            List<PokemonMaster> allPokemon = new List<PokemonMaster>()
            {
                new PokemonMaster("Pikachu", 2, "Raichu", 50, 0, "Lightning Bolt", 30, "uncommon"),
                new PokemonMaster("Eevee", 3, "Flareon", 30, 0, "Run Away", 25, "uncommon"),
                new PokemonMaster("Charmander", 1, "Charmeleon", 30, 0, "Solar Power", 10, "uncommon")
            };

            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Player player = Player.LoadFromFile(username);
            if (player == null)
            {
                player = new Player();
                player.Username = username;
                Console.WriteLine("New player profile created!");
            }
            else
            {
                Console.WriteLine($"Welcome back, {player.Username}!");
            }
            RaidBoss raidBoss = null;
            List<PokemonMaster> pokemonMasters = new List<PokemonMaster>()
            {
            };
            string choice;
            do
            {
                Console.WriteLine("******************************\nWelcome to Pokemon Pocket App!\n******************************");
                if (player.HasAddedStarter == false)
                {
                    Console.WriteLine(" (1). Add a Starter Pokemon to your pocket (Start here!)");
                }
                else
                {
                    Console.WriteLine(" (1). Add a Starter Pokemon to your pocket (Disabled)");
                }
                Console.WriteLine(" (2). Inventory \n (3). Check if I can evolve my pokemon \n (4). Evolve Pokemon \n (5). Battle a random foe \n (6). Sacrifice Pokemon \n (7). Use event shop \n (8). Battle Raid Boss (Recommended Full Team with High Lv.) \n******************************\n");
                Console.WriteLine("Please only enter the mentioned numbers or 'q' to quit:");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPokemon(player, pokemonMasters);
                        break;
                    case "2":
                        ListPokemon(player);
                        break;
                    case "3":
                        CheckEvolve(player, pokemonMasters);
                        break;
                    case "4":
                        EvolvePokemon(player, pokemonMasters);
                        break;
                    case "5":
                        BattlePokemon(player, pokemonMasters);
                        break;
                    case "6":
                        SacrificePokemon(player);
                        break;
                    case "7":
                        UseEventShop(player);
                        break;
                    case "8":
                        if (raidBoss == null || raidBoss.AttemptsLeft <= 0)
                        {
                            raidBoss = RaidBoss.GenerateRaidBoss();
                            Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
                        }
                        RaidBossBattle(player, ref raidBoss);
                        break;
                    case "q":
                        player.SaveToFile();
                        Console.WriteLine("Game saved. Exiting the program.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            } while (choice != "q");
        }


        //Use "Environment.Exit(0);" if you want to implement an exit of the console program
        //Start your assignment 1 requirements below.

        static void AddPokemon(Player player, List<PokemonMaster> pokemonMasters)
        {

            if (player.HasAddedStarter)
            {
                Console.WriteLine("You have already added a Pokemon to your pocket. You cannot add another one.");
                return;
            }

            Console.WriteLine("Enter a pokemon to be added to your pocket (can only be Charmander, Pikachu or Eevee):");
            string addPokemon = Console.ReadLine();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            addPokemon = textInfo.ToTitleCase(addPokemon);
            PokemonMaster pokemonToAdd = null;

            if (addPokemon.Equals("Charmander", StringComparison.OrdinalIgnoreCase))
            {
                pokemonToAdd = new Pokemon.Charmander();
            }
            else if (addPokemon.Equals("Pikachu", StringComparison.OrdinalIgnoreCase))
            {
                pokemonToAdd = new Pokemon.Pikachu();
            }
            else if (addPokemon.Equals("Eevee", StringComparison.OrdinalIgnoreCase))
            {
                pokemonToAdd = new Pokemon.Eevee();
            }

            if (pokemonToAdd != null)
            {
                Console.WriteLine($"Enter HP for {pokemonToAdd.Name}:");
                if (int.TryParse(Console.ReadLine(), out int newHP))
                {
                    pokemonToAdd.MaxHP = newHP;
                    pokemonToAdd.UpdateHP(newHP);
                }
                else
                {
                    Console.WriteLine("Invalid HP value. Setting HP to 30 by default.");
                    pokemonToAdd.MaxHP = 30;
                    pokemonToAdd.UpdateHP(30);
                }

                Console.WriteLine($"Enter EXP for {pokemonToAdd.Name}:");
                if (int.TryParse(Console.ReadLine(), out int newEXP))
                {
                    pokemonToAdd.UpdateEXP(newEXP);
                }
                else
                {
                    Console.WriteLine("Invalid EXP value. Setting EXP to 0 by default.");
                    pokemonToAdd.UpdateEXP(0);
                }

                player.AddPokemon(pokemonToAdd);
                player.HasAddedStarter = true;
                player.SaveToFile();
            }
            else
            {
                Console.WriteLine("Invalid Pokémon name. Please try again.");
            }

            Console.WriteLine($"You have added completed the tutorial! You have received 1000 shards as a reward!");

        }

        static void ListPokemon(Player player)
        {
            Console.WriteLine("Listing all Pokemon in your pocket: \n--------------------------");
            var sortedbyExp = player.GetPokemonCollection().OrderByDescending(pokemon => pokemon.Experience);
            foreach (var pokemon in sortedbyExp)
            {
                Console.WriteLine($"Name: {pokemon.Name} \nHP: {pokemon.HP}/{pokemon.MaxHP}\nLevel: {pokemon.Level} \nEXP: {pokemon.Experience}\nSkill: {pokemon.SkillName} \nSkill Damage: {pokemon.SkillDamage}\n--------------------------");
            }
            Console.WriteLine($"Total Pokemon: {player.GetPokemonCollection().Count}/{player.MaxPokemonCount}");
            Console.WriteLine($"Total Shards: {player.GetShardCount()}");
        }

        static void CheckEvolve(Player player, List<PokemonMaster> pokemonMasters)
        {
            Console.WriteLine("Checking for possible evolutions...");

            var groupedPokemon = player.GetPokemonCollection()
                .GroupBy(pokemon => pokemon.Name)
                .Select(group => new { Name = group.Key, Count = group.Count(), PokemonList = group.ToList() });

            foreach (var group in groupedPokemon)
            {
                var pokemon = group.PokemonList.FirstOrDefault();
                if (pokemon == null || string.IsNullOrEmpty(pokemon.EvolveTo))
                {
                    Console.WriteLine($"{group.Name} cannot evolve further.");
                    continue;
                }

                Console.WriteLine($"{pokemon.NoToEvolve} {group.Name} --> 1 {pokemon.EvolveTo}");
            }
        }

        static void EvolvePokemon(Player player, List<PokemonMaster> pokemonMasters)
        {
            Console.WriteLine("Evolving Pokémon...");

            var groupedPokemon = player.GetPokemonCollection().GroupBy(pokemon => pokemon.Name).Select(group => new { Name = group.Key, Count = group.Count(), PokemonList = group.ToList() });
            foreach (var group in groupedPokemon)
            {

                foreach (var pokemon in group.PokemonList)
                {
                    if (pokemon == null || string.IsNullOrEmpty(pokemon.EvolveTo))
                    {
                        Console.WriteLine($"{group.Name} cannot evolve further.");
                        continue;
                    }


                    if (pokemon.Experience < 1500)
                    {
                        Console.WriteLine($"{pokemon.Name} does not have enough EXP to evolve. Required: 1500, Current: {pokemon.Experience}");
                        continue;
                    }

                    if (group.Count >= pokemon.NoToEvolve)
                    {

                        var pokemonToRemove = group.PokemonList.Take(pokemon.NoToEvolve).ToList();
                        foreach (var p in pokemonToRemove)
                        {
                            player.RemovePokemon(p);
                        }

                        var evolved = pokemon.Evolve();
                        player.AddEvolvedPokemon(evolved);

                        Console.ReadLine();
                        break;
                    }

                    else
                    {
                        Console.WriteLine($"No evolution information found for {group.Name}.");
                    }
                }
            }

        }

        static void CheckForRebirth(Player player)
        {
            foreach (var pokemon in player.GetPokemonCollection())
            {
                if (pokemon.Level == 100)
                {
                    Console.WriteLine($"{pokemon.Name} has reached the pinnacle of pinnacles. It is eligible for ascension. This will remove all of your pokemon and will give you a secret stat named \"rebirth\", giving all pokemon a 10% boost in EXP gains as well as an increased catch chance. This comes at the cost of more difficult encounters. Would you like to proceed?");
                    string input = Console.ReadLine()?.ToLower();
                    if (input == "yes")
                    {
                        player.Rebirth();
                        player.SaveToFile();
                        Console.WriteLine("Rebirth complete!");
                        break;
                    }
                }
            }
        }
        static void BattlePokemon(Player player, List<PokemonMaster> pokemonMasters)
        {
            Console.WriteLine("Battling a random foe...");

            PokemonMaster foe;
            try
            {
                foe = PokemonMaster.GetRandomFoe();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            Random random = new Random();
            int minHP, maxHP;
            switch (foe.Rarity)
            {
                case "common":
                    minHP = 20;
                    maxHP = 40;
                    break;
                case "uncommon":
                    minHP = 35;
                    maxHP = 70;
                    break;
                case "rare":
                    minHP = 60;
                    maxHP = 100;
                    break;
                case "super rare":
                    minHP = 80;
                    maxHP = 140;
                    break;
                case "legendary":
                    minHP = 175;
                    maxHP = 350;
                    break;
                case "???":
                    minHP = 200;
                    maxHP = 999;
                    break;
                default:
                    minHP = 10;
                    maxHP = 30;
                    break;
            }
            foe.MaxHP = random.Next(minHP, maxHP + 1);
            foe.HP = foe.MaxHP;
            Console.WriteLine($"A(n) {foe.Rarity} encounter!\n");
            Console.WriteLine($"You are battling {foe.Name} with HP: {foe.HP} (Press Enter to go to next line)");
            Console.ReadLine();

            var playerPokemonCollection = player.GetPokemonCollection();
            if (!playerPokemonCollection.Any())
            {
                Console.WriteLine("You have no Pokémon to battle with!");
                return;
            }

            List<PokemonMaster> capturedPokemon = new List<PokemonMaster>();

            PokemonMaster currentPokemon = playerPokemonCollection.First();
            foreach (var pokemon in playerPokemonCollection)
            {

                Console.WriteLine($"Your Pokémon {pokemon.Name} with HP: {pokemon.HP} and Skill: {pokemon.SkillName} is ready to fight! \n");


                while (pokemon.HP > 0 && foe.HP > 0)
                {

                    Console.WriteLine($"{pokemon.Name} attacks {foe.Name} with {pokemon.SkillName}! \n");
                    foe.calculateDamage(pokemon.SkillDamage);
                    Console.ReadLine();

                    if (foe.HP <= 0)
                    {
                        Console.WriteLine($"You defeated {foe.Name}!");
                        Console.ReadLine();

                        int expAwarded = random.Next(40, 300) * (1 + (player.RebirthCount / 10));
                        Console.WriteLine($"Each Pokemon gains {expAwarded} EXP!");
                        foreach (var p in playerPokemonCollection)
                        {

                            p.UpdateEXP(expAwarded);
                        }
                        Console.WriteLine($"Would you like to try to capture {foe.Name}? (yes/no)");
                        string captureChoice = Console.ReadLine()?.ToLower();

                        if (captureChoice == "yes")
                        {
                            int captureChance = random.Next(1, 101);
                            if (captureChance <= player.PokemonCatchChance)
                            {
                                if (player.GetPokemonCollection().Count >= player.MaxPokemonCount)
                                {
                                    Console.WriteLine($"You cannot capture more than {player.MaxPokemonCount} Pokemon. Please sacrifice one before capturing another.");
                                    return;
                                }
                                else if (capturedPokemon.Count >= 8)
                                {
                                    Console.WriteLine("You cannot capture more than 8 Pokemon. Please sacrifice one before capturing another.");
                                    return;
                                }
                                else
                                {
                                    foe.HP = foe.MaxHP;
                                    capturedPokemon.Add(foe);
                                    Console.WriteLine($"Congratulations! You captured {foe.Name}! ({player.GetPokemonCollection().Count}/8)");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Oh no! {foe.Name} escaped!");

                                foreach (var innerPokemon in player.GetPokemonCollection())
                                {
                                    innerPokemon.UpdateHP(innerPokemon.MaxHP);
                                }

                            }
                        }

                        foreach (var captured in capturedPokemon)
                        {
                            player.AddEvolvedPokemon(captured);
                            Console.WriteLine($"You have captured {captured.Name}!");
                        }

                        foreach (var playerPokemon in player.GetPokemonCollection())
                        {
                            playerPokemon.UpdateHP(playerPokemon.MaxHP);
                        }

                        return;

                    }

                    Console.WriteLine($"{foe.Name} attacks {pokemon.Name} for {foe.SkillDamage} damage!");
                    pokemon.calculateDamage(foe.SkillDamage);
                    Console.ReadLine();

                    if (pokemon.HP <= 0)
                    {
                        Console.WriteLine($"{pokemon.Name} has fainted!");
                        currentPokemon = playerPokemonCollection.FirstOrDefault(p => p.HP > 0);
                        if (currentPokemon == null)
                        {
                            Console.WriteLine("All your Pokémon have fainted! You lost the battle.");
                            break;
                        }
                        Console.WriteLine($"Come out! {currentPokemon.Name}!");
                        Console.ReadLine();
                    }
                }

                if (foe.HP > 0)
                {
                    Console.WriteLine($"You lost the battle! {foe.Name} remains undefeated.");
                }
            }
            foreach (var pokemon in player.GetPokemonCollection())
            {
                pokemon.UpdateHP(pokemon.MaxHP);
            }
            CheckForRebirth(player);
        }

        static void SacrificePokemon(Player player)
        {
            var playerPokemonCollection = player.GetPokemonCollection();
            if (!playerPokemonCollection.Any())
            {
                Console.WriteLine("You have no Pokémon to sacrifice.");
                return;
            }

            Console.WriteLine("Choose a Pokémon to sacrifice:");
            ListPokemon(player);

            PokemonMaster pokemonToSacrifice = null;
            while (pokemonToSacrifice == null)
            {
                Console.WriteLine("Enter the name of the Pokémon you want to sacrifice:");
                string chosenName = Console.ReadLine();
                pokemonToSacrifice = playerPokemonCollection.FirstOrDefault(p => p.Name.Equals(chosenName, StringComparison.OrdinalIgnoreCase));

                if (pokemonToSacrifice == null)
                {
                    Console.WriteLine("Invalid choice. Please choose a valid Pokémon.");
                }
            }

            int shardReward = CalculateShardReward(pokemonToSacrifice);
            player.AddShard(shardReward);

            player.RemovePokemon(pokemonToSacrifice);
            Console.WriteLine($"{pokemonToSacrifice.Name} has been sacrificed. You received {shardReward} Shards.");
        }

        static int CalculateShardReward(PokemonMaster pokemon)
        {
            int baseReward = 5;
            int rarityMultiplier = pokemon.Rarity switch
            {
                "common" => 0,
                "uncommon" => 5,
                "rare" => 10,
                "super rare" => 12,
                "legendary" => 15,
                "???" => 30,
                _ => 1
            };

            return baseReward + (rarityMultiplier * pokemon.Level);
        }

        static void UseEventShop(Player player)
        {
            Console.WriteLine("Welcome to the Event Shop!");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Roll for Pokémon (50 Shards)");
            Console.WriteLine($"2. Increase Pocket Size (Current Max: {player.MaxPokemonCount}, Cost: {player.PocketUpgradeCost} Shards)");
            Console.WriteLine($"3. Upgrade PokeBalls (Current Chance: {player.PokemonCatchChance}, Cost: {250 + (player.CatchChancePurchaseCount * 250)} Shards)");
            Console.WriteLine("Enter the number of your choice or 'exit' to return to the main menu:");

            string choice = Console.ReadLine()?.ToLower();
            if (choice == "exit")
            {
                Console.WriteLine("Returning to the main menu.");
                return;
            }

            switch (choice)
            {
                case "1":
                    player.RollForPokemon();
                    break;
                case "2":
                    player.UpgradePocketSize();
                    break;
                case "3":
                    player.UpgradeCatchChance();
                    break;
                case "setlevel":
                    Console.WriteLine("Enter the name of the Pokémon to set the level for:");
                    string pokeNameSet = Console.ReadLine();
                    var pokeSet = player.GetPokemonCollection().FirstOrDefault(p => p.Name.Equals(pokeNameSet, StringComparison.OrdinalIgnoreCase));
                    if (pokeSet == null)
                    {
                        Console.WriteLine("No such Pokémon found.");
                        break;
                    }
                    Console.WriteLine("Enter the level to set (1-100):");
                    if (int.TryParse(Console.ReadLine(), out int newLevel) && newLevel >= 1 && newLevel <= 100)
                    {
                        pokeSet.Level = newLevel;
                        pokeSet.Experience = 0;
                        Console.WriteLine($"{pokeSet.Name}'s level is now {pokeSet.Level}.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid level.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice. Returning to the main menu.");
                    break;
            }
        }




        static void RaidBossBattle(Player player, ref RaidBoss raidBoss)
        {
            if (raidBoss.AttemptsLeft <= 0)
            {
                Console.WriteLine($"The Raid Boss {raidBoss.Name} has disappeared!");
                raidBoss = RaidBoss.GenerateRaidBoss();
                Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
                return;
            }

            Console.WriteLine($"You are battling the Raid Boss {raidBoss.Name}!");
            Console.WriteLine($"Rarity: {raidBoss.Rarity}");
            Console.WriteLine($"HP: {raidBoss.HP}/{raidBoss.MaxHP}");
            Console.WriteLine($"Damage: {raidBoss.SkillDamage}");
            Console.WriteLine($"Attempts Left: {raidBoss.AttemptsLeft}");

            var playerPokemonCollection = player.GetPokemonCollection();
            if (!playerPokemonCollection.Any())
            {
                Console.WriteLine("You have no Pokémon to battle with!");
                return;
            }

            PokemonMaster currentPokemon = playerPokemonCollection.FirstOrDefault(p => p.HP > 0);
            int playerAttackCounter = 0;

            while (raidBoss.HP > 0 && currentPokemon != null)
            {
                Console.WriteLine($"Your Pokémon {currentPokemon.Name} attacks the Raid Boss {raidBoss.Name}!");
                raidBoss.TakeDamage(currentPokemon.SkillDamage);
                Console.WriteLine($"{raidBoss.Name} takes {currentPokemon.SkillDamage} damage! Remaining HP: {raidBoss.HP}/{raidBoss.MaxHP}");
                playerAttackCounter++;

                if (playerAttackCounter % 2 == 0)
                {
                    currentPokemon.calculateDamage(raidBoss.SkillDamage);
                    Console.WriteLine($"{raidBoss.Name} attacks {currentPokemon.Name} for {raidBoss.SkillDamage} damage!");
                    Console.ReadLine();

                    if (currentPokemon.HP <= 0)
                    {
                        Console.WriteLine($"{currentPokemon.Name} has fainted!");
                        currentPokemon = playerPokemonCollection.FirstOrDefault(p => p.HP > 0);
                        if (currentPokemon == null)
                        {
                            Console.WriteLine("All your Pokémon have fainted! You lost this attempt.");
                            break;
                        }
                        Console.WriteLine($"Switching to {currentPokemon.Name}!");
                    }
                }

                if (raidBoss.IsDefeated())
                {
                    Console.WriteLine($"Congratulations! You defeated the Raid Boss {raidBoss.Name}!");
                    Console.WriteLine($"Would you like to try to capture {raidBoss.Name}? (yes/no)");
                    string captureChoice = Console.ReadLine()?.ToLower();

                    if (captureChoice == "yes")
                    {
                        Random random = new Random();
                        int captureChance = random.Next(1, 101);


                        int requiredChance = raidBoss.Rarity switch
                        {
                            "common" => 20 + player.PokemonCatchChance,
                            "uncommon" => 10 + player.PokemonCatchChance,
                            "rare" => 5 + player.PokemonCatchChance,
                            "super rare" => player.PokemonCatchChance - 5,
                            "legendary" => player.PokemonCatchChance - 5,
                            "???" => player.PokemonCatchChance - 10,
                            _ => 50
                        };

                        if (captureChance <= requiredChance)
                        {
                            if (player.GetPokemonCollection().Count >= player.MaxPokemonCount)
                            {
                                Console.WriteLine("You cannot capture more Pokémon. Please sacrifice one before capturing another.");
                                raidBoss = RaidBoss.GenerateRaidBoss();
                                Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
                                continue;
                            }

                            int boostedMaxHP = (int)(raidBoss.MaxHP / 20 * 1.25);
                            int boostedSkillDamage = (int)(raidBoss.SkillDamage / 6 * 1.25);


                            player.AddEvolvedPokemon(new PokemonMaster(
                                raidBoss.Name,
                                0,
                                "",
                                boostedMaxHP,
                                0,
                                raidBoss.SkillName,
                                boostedSkillDamage,
                                raidBoss.Rarity
                            ));
                            Console.WriteLine($"Congratulations! You captured the Raid Boss {raidBoss.Name}!");
                            Console.WriteLine($"Due to its superior genomes, it has attained a 25% boost in HP and Skill Damage!");
                            Console.WriteLine($"However, it has lost it's ability to evolve.)");
                        }
                        else
                        {
                            Console.WriteLine($"Oh no! {raidBoss.Name} escaped!");
                        }
                    }

                    raidBoss = RaidBoss.GenerateRaidBoss();
                    Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
                    continue;
                }
            }

            if (!raidBoss.IsDefeated())
            {
                raidBoss.AttemptsLeft--;
                Console.WriteLine($"The Raid Boss {raidBoss.Name} remains undefeated. Attempts Left: {raidBoss.AttemptsLeft}");
                if (raidBoss.AttemptsLeft <= 0)
                {
                    Console.WriteLine($"The Raid Boss {raidBoss.Name} has disappeared!");
                    raidBoss = RaidBoss.GenerateRaidBoss();
                    Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
                }

            }


            Console.WriteLine("Healing all Pokémon after the battle...");
            foreach (var pokemon in player.GetPokemonCollection())
            {
                pokemon.UpdateHP(pokemon.MaxHP);
                Console.WriteLine($"{pokemon.Name} has been healed to {pokemon.HP}/{pokemon.MaxHP} HP.");
            }
        }
    }

}



