using System;
using System.Collections.Generic;

namespace HomeWork2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Battlefield battlefield = new Battlefield();

            battlefield.Field();

            Console.ReadKey();
        }
    }

    public static class RandomGenerator
    {
        private static Random s_random = new Random();

        public static int Next(int minimum, int maximum) => s_random.Next(minimum, maximum);
    }

    class Battlefield
    {
        private Squad _squadOne = new Squad("Country One");
        private Squad _squadTwo = new Squad("Country Two");

        private Soldier _firstCountry;
        private Soldier _secondCountry;

        public void Field()
        {
            while (_squadOne.IsDefeated() == false && _squadTwo.IsDefeated() == false)
            {
                _firstCountry = _squadOne.GetRandomSoldiers();
                _secondCountry = _squadTwo.GetRandomSoldiers();

                _squadOne.ShowSoldiers();
                _squadTwo.ShowSoldiers();

                _firstCountry.TakeDamage(_secondCountry.Damage);
                _secondCountry.TakeDamage(_firstCountry.Damage);

                _firstCountry.UsesSpecialAttack(_secondCountry);
                _secondCountry.UsesSpecialAttack(_firstCountry);

                _squadOne.RemoveDefeatedSoldiers();
                _squadTwo.RemoveDefeatedSoldiers();
            }

            ShowBattleResult();
        }

        private void ShowBattleResult()
        {
            if (_squadOne.IsDefeated() && _squadTwo.IsDefeated())
            {
                Console.WriteLine($"The battle ended in a draw.");
            }
            else if (_squadOne.IsDefeated())
            {
                Console.WriteLine($"{_squadOne.NameCountry} -- The first country has lost.");
            }
            else if (_squadTwo.IsDefeated())
            {
                Console.WriteLine($"{_squadTwo.NameCountry} -- The second country has lost.");
            }
        }
    }

    class Squad
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Squad(string nameCountry)
        {
            NameCountry = nameCountry;
            _soldiers.AddRange(CreateSoldiers());
        }

        public string NameCountry { get; private set; }

        public bool IsDefeated()
        {
            foreach (Soldier soldier in _soldiers)
            {
                if (soldier.isAlive)
                {
                    return false;
                }
            }

            return true;
        }

        public Soldier GetRandomSoldiers()
        {
            int randomIndex = RandomGenerator.Next(0, _soldiers.Count);

            Soldier randomSoldiers = _soldiers[randomIndex];

            return randomSoldiers;
        }

        public void RemoveDefeatedSoldiers()
        {
            List<Soldier> aliveSoldiers = new List<Soldier>();

            foreach (Soldier soldier in _soldiers)
            {
                if (soldier.isAlive)
                {
                    aliveSoldiers.Add(soldier);
                }
            }

            _soldiers = aliveSoldiers;
        }

        public void ShowSoldiers()
        {
            Console.WriteLine($"{NameCountry} Squad:");

            foreach (Soldier soldier in _soldiers)
            {
                Console.WriteLine($"  {soldier.Name} - Health: {soldier.Health}, Damage: {soldier.Damage}");
            }
        }

        private List<Soldier> CreateSoldiers()
        {
            List<Soldier> soldiers = new List<Soldier>();

            int minHealth = 0;
            int maxHealth = 100;

            int minDamage = 40;
            int maxDamage = 60;

            int health = RandomGenerator.Next(minHealth, maxHealth);
            int damage = RandomGenerator.Next(minDamage, maxDamage);

            soldiers.Add(new Archer(health, damage, "Archer"));
            soldiers.Add(new Swordsman(health, damage, "Swordsman"));

            return soldiers;
        }
    }

    class Soldier
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Soldier(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public string Name { get; protected set; }
        public bool isAlive => Health > 0;

        public virtual int TakeDamage(int damage)
        {
            if (isAlive)
                Health -= damage;

            return Damage;
        }

        public virtual int UsesSpecialAttack(Soldier enemy)
        {
            int totalDamage = 0;

            foreach (Soldier soldiers in _soldiers)
            {
                int damage = UsesSpecialAttack(enemy);
                totalDamage += damage;
                enemy.TakeDamage(damage);
            }

            return totalDamage;
        }
    }

    class Swordsman : Soldier
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public Swordsman(int health, int damage, string name) : base(health, damage, name)
        {
            GhostOfMoon = 49;
        }

        public int GhostOfMoon { get; protected set; }

        public override int UsesSpecialAttack(Soldier enemy)
        {
            int totalDamage = 0;

            foreach (Soldier soldiers in _soldiers)
            {
                GhostOfMoon = UsesSpecialAttack(enemy);
                totalDamage += GhostOfMoon;
                enemy.TakeDamage(GhostOfMoon);
            }

            Console.WriteLine($"Swordsman uses attack - {GhostOfMoon}.");
            return totalDamage;
        }
    }

    class Archer : Soldier
    {
        private List<Soldier> _soldires = new List<Soldier>();

        public Archer(int health, int damage, string name) : base(health, damage, name)
        {
            FlamingArrow = 59;
        }

        public int FlamingArrow { get; protected set; }

        public override int UsesSpecialAttack(Soldier enemy)
        {
            int totalDamage = 0;

            foreach (Soldier soldiers in _soldires)
            {
                FlamingArrow = UsesSpecialAttack(enemy);
                totalDamage += FlamingArrow;
                enemy.TakeDamage(FlamingArrow);
            }

            Console.WriteLine($"Archer uses attack - {FlamingArrow}.");
            return totalDamage;
        }
    }
}