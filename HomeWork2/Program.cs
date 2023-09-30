using System;
using System.Collections.Generic;
using System.Linq;

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

        public static int Next(int maximum)
        {
            return s_random.Next(maximum);
        }
    }

    class Battlefield
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        private Squad _squadOne = new Squad();
        private Squad _squadTwo = new Squad();

        public void Field()
        {
            Soldier _firstSoldier = _squadOne.GetRandomSoldier();
            Soldier _secondSoldier = _squadTwo.GetRandomSoldier();

            while (_firstSoldier.isAlive && _secondSoldier.isAlive)
            {
                _squadOne.ShowSoldiers();
                _squadTwo.ShowSoldiers();

                int damageByFirstSoldier = _firstSoldier.TakeDamage(_secondSoldier.Damage);
                int damageBySecondSoldier = _secondSoldier.TakeDamage(_firstSoldier.Damage);

                Console.WriteLine($"First Soldier dealt {damageByFirstSoldier} damage.");
                Console.WriteLine($"Second Soldier dealt {damageBySecondSoldier} damage.");

                _squadOne.RemoveSoldiers();
                _squadTwo.RemoveSoldiers();
            }

            ShowBattleResult();
        }

        private void ShowBattleResult()
        {
            string _countryOne = "CountyOne";
            string _countryTwo = "CountryTwo";

            if (_squadOne.GetTotalHealth() > _squadTwo.GetTotalHealth())
            {
                Console.WriteLine($"{_countryOne} -- The first country has won.");
            }
            else if (_squadOne.GetTotalHealth() < _squadTwo.GetTotalHealth())
            {

                Console.WriteLine($"{_countryTwo} -- The second country has won.");
            }
            else
            {
                Console.WriteLine("fff");
            }
        }
    }

    class Soldier
    {
        private List<SoldierFactory> _soldiers = new List<SoldierFactory>();

        public Soldier(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public int Health { get; set; }
        public int Damage { get; protected set; }
        public string Name { get; protected set; }
        public bool isAlive => Health > 0;

        public virtual int TakeDamage(int damage)
        {
            if (isAlive)
            {
                Health -= damage;
                return Damage;
            }

            return 0;
        }
    }

    class SoldierFactory
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public List<Soldier> CreateMedic()
        {
            int swordsmanCount = 5;

            int minDamage = 5;
            int maxDamage = 80;

            int maxHealth = 800;

            string name = "Swordsman";

            int damage = RandomGenerator.Next(minDamage, maxDamage);
            int health = RandomGenerator.Next(maxHealth);
            int SwordsmanSkill = RandomGenerator.Next(swordsmanCount);

            _soldiers.Add(new Swordsman(health, damage, swordsmanCount, name));

            return _soldiers;
        }

        public List<Soldier> CreateArcher()
        {
            int archerCount = 5;

            int minDamage = 10;
            int maxDamage = 90;

            int maxHealth = 900;

            string name = "Archer";

            int damage = RandomGenerator.Next(minDamage, maxDamage);
            int health = RandomGenerator.Next(maxHealth);
            int archerSkill = RandomGenerator.Next(archerCount);

            _soldiers.Add(new Archer(health, damage, archerCount, name));

            return _soldiers;
        }
    }

    class Squad
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        private SoldierFactory _soldierFactory = new SoldierFactory();

        private int _soldiersOutCoint = 0;

        public Squad()
        {
            _soldiers.AddRange(_soldierFactory.CreateArcher());
            _soldiers.AddRange(_soldierFactory.CreateMedic());
        }

        public int GetTotalHealth()
        {
            int totalHealth = 0;

            foreach (Soldier soldier in _soldiers)
                totalHealth += soldier.Health;

            return totalHealth;
        }

        public void RemoveSoldiers()
        {
            List<Soldier> removeToSoldiers = new List<Soldier>();

            foreach (Soldier soldiers in _soldiers)
            {
                if (!soldiers.isAlive)
                {
                    removeToSoldiers.Add(soldiers);
                }
            }

            _soldiersOutCoint = removeToSoldiers.Count;

            foreach (Soldier soldier in removeToSoldiers)
            {
                _soldiers.Remove(soldier);
            }

            Console.WriteLine($"Fallen soldiers: {removeToSoldiers.Count}. Remaining soldiers: {_soldiers.Count}.");

        }

        public void ShowSoldiers()
        {
            int soldierCount = 0;

            foreach (Soldier soldier in _soldiers)
            {
                soldierCount++;
                Console.WriteLine($"Health -- {soldier.Health}.\t" +
                                  $"Attack -- {soldier.Damage}.\t" +
                                  $"Name -- {soldier.Name}.\t");
            }
        }

        public Soldier GetRandomSoldier()
        {
            if (_soldiers.Count == 0)
            {
                return null;
            }

            int index = RandomGenerator.Next(0, _soldiers.Count);
            Soldier soldier = _soldiers[index];
            _soldiers.RemoveAt(index);
            return soldier;
        }
    }

    class Swordsman : Soldier
    {
        public Swordsman(int health, int damage, int quanity, string name) : base(health, damage, name) { }

        public override int TakeDamage(int damage)
        {
            return base.TakeDamage(damage);
        }
    }

    class Archer : Soldier
    {
        public Archer(int health, int damage, int quanity, string name) : base(health, damage, name) { }

        public override int TakeDamage(int damage)
        {
            return base.TakeDamage(damage);
        }
    }
}