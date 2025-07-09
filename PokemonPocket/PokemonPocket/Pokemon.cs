//Wong Shao Yang 242948F

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
namespace PokemonPocket
{
    public class PokemonMaster
    {
        public const int MaxLevel = 100;
        public int MaxHP { get; set; }
        public string Name { get; set; }
        public int NoToEvolve { get; set; }
        public string EvolveTo { get; set; }
        public int HP { get; set; }
        public int Experience { get; set; }
        public string SkillName { get; set; }
        public int SkillDamage { get; set; }
        public string Rarity { get; set; }
        public int Level { get; set; } = 1;

        public PokemonMaster(string name, int noToEvolve, string evolveTo, int hp, int experience, string skillName, int skillDamage, string rarity)
        {
            this.Name = name;
            this.NoToEvolve = noToEvolve;
            this.EvolveTo = evolveTo;
            this.HP = hp;
            this.MaxHP = hp;
            this.Experience = experience;
            this.SkillName = skillName;
            this.SkillDamage = skillDamage;
            this.Rarity = rarity;
        }

        private static readonly Random random = new Random();
        public static PokemonMaster GetRandomFoe()
        {


            var rarityWeights = new Dictionary<string, int> {
                { "common", 50000 },
                { "uncommon", 30000 },
                { "rare", 15000 },
                { "super rare", 4900 },
                { "legendary", 990},
                { "???", 10}
            };

            var allPokemon = new List<PokemonMaster> {
                // Common Pokémon
                new Pokemon.Caterpie(),
                new Pokemon.Pidgey(),
                new Pokemon.Rattata(),
                new Pokemon.Spearow(),
                new Pokemon.Magikarp(),
                // Uncommon Pokémon
                new Pokemon.Metapod(),
                new Pokemon.Pidgeotto(),
                new Pokemon.Charmander(),
                new Pokemon.Pikachu(),
                new Pokemon.Eevee(),
                new Pokemon.Raticate(),
                new Pokemon.Fearow(),
                new Pokemon.Dratini(),
                // Rare Pokémon
                new Pokemon.Charmeleon(),
                new Pokemon.Raichu(),
                new Pokemon.Pidgeot(),
                new Pokemon.Druddigon(),
                new Pokemon.Dragonair(),
                // Super Rare Pokémon
                new Pokemon.Charizard(),
                new Pokemon.Flareon(),
                new Pokemon.Gyarados(),
                new Pokemon.Pinsir(),
                new Pokemon.Lapras(),
                new Pokemon.Lucario(),
                new Pokemon.Spiritomb(),
                new Pokemon.Dragonite(),
                new Pokemon.Gardevoir(),
                // Legendary Pokémon
                new Pokemon.Mewtwo(),
                new Pokemon.Zamazenta(),
                new Pokemon.Regigigas(),
                new Pokemon.Reshiram(),
                new Pokemon.Zekrom(),
                new Pokemon.Kyurem(),
                // ??? rarity Pokémon
                new Pokemon.InsaneStreamer(),
                new Pokemon.Heathcliff(),
                new Pokemon.TheUnspeakable(),
                new Pokemon.TheTetoPlush(),
                new Pokemon.BugCat(),
                new Pokemon.Monsoon()
            };

            var weightedList = new List<PokemonMaster>();
            foreach (var pokemon in allPokemon)
            {
                if (rarityWeights.TryGetValue(pokemon.Rarity, out int weight))
                {
                    for (int i = 0; i < weight; i++)
                    {
                        weightedList.Add(pokemon);
                    }
                }
            }

            if (!weightedList.Any())
            {
                throw new InvalidOperationException("No Pokémon match the rarity weights. Cannot select a random foe.");
            }

            Random random = new Random();
            int randomIndex = random.Next(weightedList.Count);
            return weightedList[randomIndex];
        }


        public void UpdateHP(int newHP)
        {

            if (newHP < 25)
            {
                Console.WriteLine($"HP is too low ({newHP}), setting it to 25 by default.");
                newHP = 25;
            }



            this.HP = newHP;

        }


        public void UpdateEXP(int newXP)
        {
            if (newXP < 0)
            {
                Console.WriteLine("Experience cannot be negative. Setting EXP to 0.");
                newXP = 0;
            }

            while (Experience >= GetEXPToNextLevel())
            {
                LevelUp();
            }

            this.Experience += newXP;
            Console.WriteLine($"{Name}'s EXP has increased to {this.Experience}!");
        }

        private int GetEXPToNextLevel()
        {
            return (int)((Math.Pow(Level, 3) * (162 - Level)) / 100);
        }

        private void LevelUp()
        {

            if (Level >= MaxLevel)
            {
                Console.WriteLine($"{Name} has reached the maximum level of {MaxLevel}.");
                return;
            }
            Level++;
            if (Level > MaxLevel)
            {
                Level = MaxLevel;
            }
            MaxHP += (int)2.5;
            HP = MaxHP;
            SkillDamage += (int)1.25m;
            Experience = 0;

            Console.WriteLine($"{Name} leveled up to level {Level}! Max HP is now {MaxHP}.");

            if (Level == MaxLevel)
            {
                Console.WriteLine($"{Name} has reached the maximum level of {MaxLevel}.");
            }
        }

        public PokemonMaster Evolve()
        {
            if (string.IsNullOrEmpty(EvolveTo))
            {
                Console.WriteLine($"{Name} cannot evolve further.");
                return this;
            }

            Console.WriteLine($"{Name} is evolving into {EvolveTo}!");

            return EvolveTo switch
            {
                "Charmeleon" => new Pokemon.Charmeleon(),
                "Raichu" => new Pokemon.Raichu(),
                "Flareon" => new Pokemon.Flareon(),
                "Charizard" => new Pokemon.Charizard(),
                _ => throw new InvalidOperationException($"No subclass found for evolution: {EvolveTo}")
            };
        }
        public virtual void calculateDamage(int damage)
        {
            this.HP -= damage;
            if (this.HP <= 0)
            {
                this.HP = 0;
                Console.WriteLine($"{Name} has fainted.");
            }
            Console.WriteLine($"{Name} took {damage} damage. Remaining HP: {this.HP}");

        }

        public static List<PokemonMaster> GetAllPokemonByRarity(string rarity)
        {
            // Use the existing Pokémon classes from Pokemon.cs
            var allPokemon = new List<PokemonMaster>
    {
        // Common Pokémon
        new Pokemon.Caterpie(),
        new Pokemon.Pidgey(),
        new Pokemon.Rattata(),
        new Pokemon.Spearow(),
        new Pokemon.Magikarp(),
        // Uncommon Pokémon
        new Pokemon.Metapod(),
        new Pokemon.Pidgeotto(),
        new Pokemon.Charmander(),
        new Pokemon.Pikachu(),
        new Pokemon.Eevee(),
        new Pokemon.Raticate(),
        new Pokemon.Fearow(),
        new Pokemon.Dratini(),
        // Rare Pokémon
        new Pokemon.Charmeleon(),
        new Pokemon.Raichu(),
        new Pokemon.Pidgeot(),
        new Pokemon.Druddigon(),
        new Pokemon.Dragonair(),
        // Super Rare Pokémon
        new Pokemon.Charizard(),
        new Pokemon.Flareon(),
        new Pokemon.Gyarados(),
        new Pokemon.Pinsir(),
        new Pokemon.Lapras(),
        new Pokemon.Lucario(),
        new Pokemon.Spiritomb(),
        new Pokemon.Dragonite(),
        new Pokemon.Gardevoir(),
        // Legendary Pokémon
        new Pokemon.Mewtwo(),
        new Pokemon.Zamazenta(),
        new Pokemon.Regigigas(),
        new Pokemon.Reshiram(),
        new Pokemon.Zekrom(),
        new Pokemon.Kyurem(),
        // ??? rarity Pokémon
        new Pokemon.InsaneStreamer(),
        new Pokemon.Heathcliff(),
        new Pokemon.TheUnspeakable(),
        new Pokemon.TheTetoPlush(),
        new Pokemon.BugCat(),
        new Pokemon.Monsoon()
    };

            return allPokemon.Where(p => p.Rarity.Equals(rarity, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static string GetRandomRarity(Dictionary<string, int> rarityWeights)
        {
            int totalWeight = rarityWeights.Values.Sum();
            Random random = new Random();
            int randomValue = random.Next(1, totalWeight + 1);

            int cumulativeWeight = 0;
            foreach (var rarity in rarityWeights)
            {
                cumulativeWeight += rarity.Value;
                if (randomValue <= cumulativeWeight)
                {
                    return rarity.Key;
                }
            }

            return "common";
        }

    }


    public class Pokemon
    {
        // The following classes are for the common Pokémon

        public class Caterpie : PokemonMaster
        {
            public Caterpie() : base("Caterpie", 3, "Metapod", 25, 0, "String Shot", 5, "common") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }
        public class Pidgey : PokemonMaster
        {
            public Pidgey() : base("Pidgey", 3, "Pidgeotto", 25, 0, "Quick Attack", 10, "common") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Rattata : PokemonMaster
        {
            public Rattata() : base("Rattata", 3, "Raticate", 30, 0, "Bite", 15, "common") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }
        public class Spearow : PokemonMaster
        {
            public Spearow() : base("Spearow", 3, "Fearow", 35, 0, "Peck", 20, "common") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Magikarp : PokemonMaster
        {
            public Magikarp() : base("Magikarp", 5, "Gyarados", 25, 0, "Splash", 5, "common") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }


        // The following classes are for the uncommon Pokémon
        public class Charmander : PokemonMaster
        {
            public Charmander() : base("Charmander", 1, "Charmeleon", 30, 0, "Solar Power", 10, "uncommon")
            {

            }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }

        }

        public class Pikachu : PokemonMaster
        {
            public Pikachu() : base("Pikachu", 2, "Raichu", 50, 0, "Lightning Bolt", 30, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 3);
                Console.WriteLine($"{Name} takes an increased {increasedDamage} amount of damage!");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Eevee : PokemonMaster
        {
            public Eevee() : base("Eevee", 3, "Flareon", 30, 0, "Run Away", 25, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                int increasedDamage = damage * 2;
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}!");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Metapod : PokemonMaster
        {
            public Metapod() : base("Metapod", 2, "Butterfree", 30, 0, "Tackle", 15, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.5);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Pidgeotto : PokemonMaster
        {
            public Pidgeotto() : base("Pidgeotto", 2, "Pidgeot", 30, 0, "Wing Attack", 25, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Raticate : PokemonMaster
        {
            public Raticate() : base("Raticate", 0, "", 50, 0, "Hyper Fang", 35, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Fearow : PokemonMaster
        {
            public Fearow() : base("Fearow", 0, "", 40, 0, "Double-Edge", 25, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Dratini : PokemonMaster
        {
            public Dratini() : base("Dratini", 4, "Dragonair", 30, 0, "Wrap", 20, "uncommon")
            {


            }
            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        // The following classes are for the rare Pokémon
        public class Charmeleon : PokemonMaster
        {
            public Charmeleon() : base("Charmeleon", 2, "Charizard", 100, 0, "Flame Cannon", 10, "rare") { }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Butterfree : PokemonMaster
        {
            public Butterfree() : base("Butterfree", 0, "", 60, 0, "Bug Buzz", 50, "rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.95);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Raichu : PokemonMaster
        {
            public Raichu() : base("Raichu", 0, "", 100, 0, "Thunder Strike", 30, "rare") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = damage * 3;
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}!");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Pidgeot : PokemonMaster
        {
            public Pidgeot() : base("Pidgeot", 0, "", 80, 0, "Hurricane", 45, "rare") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.3);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Druddigon : PokemonMaster
        {
            public Druddigon() : base("Druddigon", 0, "", 90, 0, "Dragon Claw", 45, "rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.8);
                Console.WriteLine($"{Name} takes flat damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Dragonair : PokemonMaster
        {
            public Dragonair() : base("Dragonair", 3, "Dragonite", 50, 0, "Dragon Tail", 40, "rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.9);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }
        // The following classes are for the super rare Pokémon
        public class Charizard : PokemonMaster
        {
            public Charizard() : base("Charizard", 2, "", 60, 0, "Flame Devastation", 55, "super rare") { }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }


        public class Flareon : PokemonMaster
        {
            public Flareon() : base("Flareon", 0, "", 100, 0, "Run Away", 25, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int IncreasedDamage = (int)(damage * 2);
                Console.WriteLine($"{Name} takes increased damage of {IncreasedDamage}.");
                base.calculateDamage(damage);
            }
        }

        public class Gyarados : PokemonMaster
        {
            public Gyarados() : base("Gyarados", 0, "", 100, 0, "Hydro Beam", 65, "super rare") { }

            public override void calculateDamage(int damage)
            {

                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }
        public class Pinsir : PokemonMaster
        {
            public Pinsir() : base("Pinsir", 0, "", 100, 0, "X-Scissor", 55, "super rare") { }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Lapras : PokemonMaster
        {
            public Lapras() : base("Lapras", 0, "", 100, 0, "Ice Beam", 60, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.85);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(damage);
            }
        }

        public class Lucario : PokemonMaster
        {
            public Lucario() : base("Lucario", 0, "", 100, 0, "Aura Sphere", 75, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage}.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Spiritomb : PokemonMaster
        {
            public Spiritomb() : base("Spiritomb", 0, "", 100, 0, "Shadow Ball", 70, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.8);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Dragonite : PokemonMaster
        {
            public Dragonite() : base("Dragonite", 0, "", 100, 0, "Hyper Beam", 80, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.9);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }
        public class Gardevoir : PokemonMaster
        {
            public Gardevoir() : base("Gardevoir", 0, "", 100, 0, "Psychic", 70, "super rare") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.85);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }
        // The following classes are for the legendary Pokémon

        public class Mewtwo : PokemonMaster
        {
            public Mewtwo() : base("Mewtwo", 0, "", 100, 0, "Psycho Cut", 85, "legendary") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.9);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Zamazenta : PokemonMaster
        {
            public Zamazenta() : base("ZamaZenta", 0, "", 100, 0, "Beast Roar", 80, "legendary") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.85);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Regigigas : PokemonMaster
        {
            public Regigigas() : base("Regigigas", 0, "", 100, 0, "Giga Impact", 90, "legendary") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.8);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Reshiram : PokemonMaster
        {
            public Reshiram() : base("Reshiram", 0, "", 100, 0, "Blue Flare", 95, "legendary") { }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }

        public class Zekrom : PokemonMaster
        {
            public Zekrom() : base("Zekrom", 0, "", 100, 0, "Fusion Flare", 95, "legendary") { }

            public override void calculateDamage(int damage)
            {
                Console.WriteLine($"{Name} takes flat damage of {damage}.");
                base.calculateDamage(damage);
            }
        }
        public class Kyurem : PokemonMaster
        {
            public Kyurem() : base("Kyurem", 0, "", 100, 0, "Glaciate", 100, "legendary") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.9);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage}.");
                base.calculateDamage(damage);
            }
        }

        // The following classes are for the ??? rarity Pokémon
        public class InsaneStreamer : PokemonMaster
        {
            public InsaneStreamer() : base("An Insane Streamer", 0, "", 100, 0, "ONLY I REMAIN AS A PERFORMANCE ARTIST", 150, "???") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.25);
                Console.WriteLine($"{Name} takes increased damage of {increasedDamage} due to his nature as a performance artist.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class Heathcliff : PokemonMaster
        {
            public Heathcliff() : base("Behead All Heathcliffs???", 0, "", 100, 0, "Lament, Mourn and Despair", 180, "???") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.8);
                Console.WriteLine($"{Name} takes an increased damage of {increasedDamage} due to its nature as depression itself.");
                base.calculateDamage(increasedDamage);
            }
        }

        public class TheUnspeakable : PokemonMaster
        {
            public TheUnspeakable() : base("Tung Tung Tung Tung Tung Tung Sahur", 0, "", 100, 0, "Tung Tung Tung", 90, "???") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.75);
                Console.WriteLine($"{Name} takes decreased damage of {decreasedDamage} due to its extreme resistance.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class BugCat : PokemonMaster
        {
            public BugCat() : base("Weird Cat???", 0, "", 100, 0, "Awful Commission", 100, "???") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.5);
                Console.WriteLine($"{Name} takes a decreased damage of {decreasedDamage} due to the bold nature of its requester.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class TheTetoPlush : PokemonMaster
        {
            public TheTetoPlush() : base("The Giant Teto Plush", 0, "", 100, 0, "どうしてすぐ知ってしまうの", 144, "???") { }

            public override void calculateDamage(int damage)
            {
                int decreasedDamage = (int)(damage * 0.8);
                Console.WriteLine($"{Name} takes a decreased damage of {decreasedDamage}  due to the size of its sheer dumptruck.");
                base.calculateDamage(decreasedDamage);
            }
        }

        public class Monsoon : PokemonMaster
        {
            public Monsoon() : base("Monsoon", 0, "", 100, 0, "MEMES", 125, "???") { }

            public override void calculateDamage(int damage)
            {
                int increasedDamage = (int)(damage * 1.2);
                Console.WriteLine($"{Name} takes a decreased damage of {increasedDamage} due to the MEMES");
                base.calculateDamage(increasedDamage);
            }
        }


    }

    public class Player
    {
        public string Username { get; set; }
        [JsonInclude]
        public List<PokemonMaster> PokemonCollection = new List<PokemonMaster>();
        [JsonInclude]
        public int ShardCount = 1000;
        public int MaxPokemonCount { get; private set; } = 8;
        public int PocketUpgradeCost { get; private set; } = 500;

        public int PokemonCatchChance { get; private set; } // Base catch chance, increases with rebirths

        public int CatchChancePurchaseCount { get; private set; } = 0;

        public int RebirthCount { get; private set; } = 0;
        public int MaxRebirthCount { get; private set; } = 3;
        public bool HasAddedStarter { get; set; } = false;

        public Player()
        {
            PokemonCatchChance = 50 + (RebirthCount * 5);
        }

        public void AddPokemon(PokemonMaster pokemon)
        {
            if (pokemon is Pokemon.Charmander || pokemon is Pokemon.Pikachu || pokemon is Pokemon.Eevee)
            {
                PokemonCollection.Add(pokemon);
                Console.WriteLine($"{pokemon.Name} has been added to your collection.");
            }
            else
            {
                Console.WriteLine("This Pokemon cannot be added to your collection.");
            }
        }
        public void RemovePokemon(PokemonMaster pokemon)
        {
            PokemonCollection.Remove(pokemon);
        }

        public void AddEvolvedPokemon(PokemonMaster evolvedPokemon)
        {

            PokemonCollection.Add(evolvedPokemon);
            Console.WriteLine($"{evolvedPokemon.Name} has been added to your Pokémon Pocket!");
        }

        public void SaveToFile()
        {
            string fileName = $"{Username}.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(fileName, JsonSerializer.Serialize(this, options));
        }
        public void Rebirth()
        {
            if (this.RebirthCount >= this.MaxRebirthCount)
            {
                Console.WriteLine("You have reached the maximum rebirth count. No further rebirths are allowed.");
                return;
            }
            // Save current shards and rebirth count
            int savedShards = this.ShardCount;
            int savedRebirths = this.RebirthCount + 1;

            // Reset all data except shards and rebirth count
            this.PokemonCollection.Clear();
            this.HasAddedStarter = false;
            this.MaxPokemonCount = 8;
            this.PocketUpgradeCost = 500;
            this.PokemonCatchChance = 50;
            this.CatchChancePurchaseCount = 0;

            // Restore shards and increment rebirth count
            this.ShardCount = savedShards;
            this.RebirthCount = savedRebirths;

            Console.WriteLine("Rebirth complete! All Pokémon and upgrades have been reset, but your shards remain. Rebirth count increased by 1! Current Rebirth Count: " + this.RebirthCount);
        }

        public static Player LoadFromFile(string username)
        {
            string fileName = $"{username}.json";
            if (File.Exists(fileName))
            {
                return JsonSerializer.Deserialize<Player>(File.ReadAllText(fileName));
            }
            return null;
        }

        public void AddShard(int amount)
        {
            ShardCount += amount;
        }

        public int GetShardCount()
        {
            return ShardCount;
        }

        public int GetCatchChance()
        {
            return PokemonCatchChance + (RebirthCount * 5);
        }

        public void UpgradeCatchChance()
        {
            if (ShardCount < 250)
            {
                Console.WriteLine($"You do not have enough shards to upgrade your catch chance. You need 250 Shards.");
                return;
            }

            AddShard(-250);
            CatchChancePurchaseCount++;
            PokemonCatchChance += 5;
            if (CatchChancePurchaseCount >= 5)
            {
                Console.WriteLine("You have reached the maximum catch chance upgrade.");
                return;
            }
            Console.WriteLine($"Your catch chance has been increased to {PokemonCatchChance}%. The next upgrade will cost {250 + CatchChancePurchaseCount * 250} Shards.");
        }

        public void UpgradePocketSize()
        {
            if (ShardCount < PocketUpgradeCost)
            {
                Console.WriteLine($"You do not have enough shards to upgrade your pocket size. You need {PocketUpgradeCost} Shards.");
                return;
            }

            AddShard(-PocketUpgradeCost);
            MaxPokemonCount++;
            PocketUpgradeCost += 250;
            Console.WriteLine($"Your pocket size has been increased to {MaxPokemonCount}. The next upgrade will cost {PocketUpgradeCost} Shards.");
        }

        public void RollForPokemon()
        {
            const int shardCost = 50;
            if (ShardCount < shardCost)
            {
                Console.WriteLine($"You do not have enough shards to roll. You need {shardCost} Shards.");
                return;
            }

            if (PokemonCollection.Count >= MaxPokemonCount)
            {
                Console.WriteLine("Your pocket is full! You cannot roll for a Pokémon.");
                return;
            }

            AddShard(-shardCost);

            var rarityWeights = new Dictionary<string, int>
    {
        { "common", 45000 },
        { "uncommon", 35000 },
        { "rare", 15000 },
        { "super rare", 3000 },
        { "legendary", 1900 },
        { "???", 100 }
    };

            string selectedRarity = PokemonMaster.GetRandomRarity(rarityWeights);

            var allPokemon = PokemonMaster.GetAllPokemonByRarity(selectedRarity);
            if (!allPokemon.Any())
            {
                Console.WriteLine($"No Pokémon available for the {selectedRarity} rarity.");
                return;
            }

            Random random = new Random();
            PokemonMaster obtainedPokemon = allPokemon[random.Next(allPokemon.Count)];


            Console.WriteLine($"{shardCost} Shards have been deducted from your account. Current Shard Count: {ShardCount}.");
            Console.WriteLine($"Congratulations! You obtained {obtainedPokemon.Name} ({selectedRarity})!");
            AddEvolvedPokemon(obtainedPokemon);
        }



        public List<PokemonMaster> GetPokemonCollection()
        {
            return PokemonCollection;
        }
    }

    public class RaidBoss : PokemonMaster
    {

        public int AttemptsLeft { get; set; } = 3;

        public RaidBoss(string name, int maxHP, int damage, string rarity)
            : base(name, 0, "", maxHP, 0, "Raid Skill", damage, rarity)
        {
            MaxHP = maxHP;
            HP = maxHP;
            SkillDamage = damage;
            Rarity = rarity;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public bool IsDefeated()
        {
            return HP <= 0;
        }
        public static RaidBoss GenerateRaidBoss()
        {
            Random random = new Random();
            var rarities = new Dictionary<string, int>
    {
        { "common", 35000 },
        { "uncommon", 30000 },
        { "rare", 25000 },
        { "super rare", 6000 },
        { "legendary", 3000 },
        { "???", 1000 }
    };

            // Select a random rarity based on weights
            string selectedRarity = PokemonMaster.GetRandomRarity(rarities);

            // Get all Pokémon from PokemonMaster
            var allPokemon = new List<PokemonMaster>
    {
        new Pokemon.Caterpie(),
        new Pokemon.Pidgey(),
        new Pokemon.Rattata(),
        new Pokemon.Spearow(),
        new Pokemon.Magikarp(),
        new Pokemon.Metapod(),
        new Pokemon.Pidgeotto(),
        new Pokemon.Charmander(),
        new Pokemon.Pikachu(),
        new Pokemon.Eevee(),
        new Pokemon.Raticate(),
        new Pokemon.Fearow(),
        new Pokemon.Dratini(),
        new Pokemon.Charmeleon(),
        new Pokemon.Raichu(),
        new Pokemon.Pidgeot(),
        new Pokemon.Druddigon(),
        new Pokemon.Dragonair(),
        new Pokemon.Charizard(),
        new Pokemon.Flareon(),
        new Pokemon.Gyarados(),
        new Pokemon.Pinsir(),
        new Pokemon.Lapras(),
        new Pokemon.Lucario(),
        new Pokemon.Spiritomb(),
        new Pokemon.Dragonite(),
        new Pokemon.Gardevoir(),
        new Pokemon.Mewtwo(),
        new Pokemon.Zamazenta(),
        new Pokemon.Regigigas(),
        new Pokemon.Reshiram(),
        new Pokemon.Zekrom(),
        new Pokemon.Kyurem(),
        new Pokemon.InsaneStreamer(),
        new Pokemon.Heathcliff(),
        new Pokemon.TheUnspeakable(),
        new Pokemon.TheTetoPlush(),
        new Pokemon.BugCat(),
        new Pokemon.Monsoon()
    };

            var filteredPokemon = allPokemon.Where(p => p.Rarity == selectedRarity).ToList();

            if (!filteredPokemon.Any())
            {
                throw new InvalidOperationException($"No Pokémon found for rarity: {selectedRarity}");
            }

            var selectedPokemon = filteredPokemon[random.Next(filteredPokemon.Count)];

            int maxHP = selectedPokemon.MaxHP * 20;
            int damage = selectedPokemon.SkillDamage * 6;

            return new RaidBoss(selectedPokemon.Name, maxHP, damage, selectedRarity)
            {
                SkillName = selectedPokemon.SkillName
            };
        }
        public static void RefreshRaidBoss(ref RaidBoss raidBoss, ref DateTime raidBossGeneratedTime)
        {
            raidBoss = RaidBoss.GenerateRaidBoss();
            raidBossGeneratedTime = DateTime.Now; // Update the generation time
            Console.WriteLine($"A new Raid Boss has appeared: {raidBoss.Name} ({raidBoss.Rarity})!");
        }
    }
}













