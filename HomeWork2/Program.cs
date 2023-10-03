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

            Console.ReadLine();
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

        public void Field()
        {
            while (_squadOne.IsDefeated() == false && _squadTwo.IsDefeated() == false)
            {
                _squadOne.ShowSoldiers();
                _squadTwo.ShowSoldiers();

                int damageByFirstSquad = _squadOne.Attack(_squadTwo);
                int damageBySecondSquad = _squadTwo.Attack(_squadOne);

                Console.WriteLine($"First Squad dealt {damageByFirstSquad} damage.");
                Console.WriteLine($"Second Squad dealt {damageBySecondSquad} damage.");

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

        private List<Soldier> CreateSoldiers()
        {
            SoldierFactory factory = new SoldierFactory();
            List<Soldier> soldiers = new List<Soldier>();

            int squadSize = 5;

            for (int i = 0; i < squadSize; i++)
            {
                soldiers.Add(factory.CreateRandomSoldier());
            }

            return soldiers;
        }

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

        public int Attack(Squad enemySquad)
        {
            int totalDamage = 0;

            foreach (Soldier soldier in _soldiers)
            {
                if (soldier.isAlive)
                {
                    int damage = RandomGenerator.Next(1, soldier.Damage);
                    enemySquad.TakeDamage(damage);
                    totalDamage += damage;
                }
            }

            return totalDamage;
        }

        public void TakeDamage(int damage)
        {
            foreach (Soldier soldier in _soldiers)
            {
                if (soldier.isAlive)
                {
                    soldier.TakeDamage(damage);
                }
            }
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
    }

    class Soldier
    {
        public Soldier(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }
        public bool isAlive => Health > 0;

        public virtual int TakeDamage(int damage)
        {
            if (isAlive)
            {
                Health -= damage;
                return damage;
            }

            return 0;
        }
    }

    class SoldierFactory
    {
        private static int _nextSoldierId = 1;

        public Soldier CreateRandomSoldier()
        {
            int minHealth = 500;
            int maxHealth = 1000;

            int minDamage = 50;
            int maxDamage = 100;

            int health = RandomGenerator.Next(minHealth, maxHealth);
            int damage = RandomGenerator.Next(minDamage, maxDamage);
            string name = $"Soldier{_nextSoldierId}";
            _nextSoldierId++;
            return new Soldier(health, damage, name);
        }
    }
}
