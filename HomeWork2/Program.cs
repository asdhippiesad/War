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
                _firstSoldier.UseSpecialAttack(_secondSoldier);
                _secondSoldier.UseSpecialAttack(_firstSoldier);

                _squadOne.ShowSoldiers();
                _squadTwo.ShowSoldiers();

                _firstSoldier.TakeDamage(_secondSoldier.Damage);
                _secondSoldier.TakeDamage(_firstSoldier.Damage);

                _firstSoldier.UseSpecialAttack(_secondSoldier);
                _secondSoldier.UseSpecialAttack(_firstSoldier);

                _squadOne.RemoveSoldiers(_firstSoldier);
                _squadTwo.RemoveSoldiers(_secondSoldier);

                ShowBattleResult();
            }
        }

        public void ShowBattleResult()
        {
            string _countryOne = "CountyOne";
            string _countryTwo = "CountryTwo";

            if (_squadOne.GetTotalHealth() > _squadTwo.GetTotalHealth())
            {
                Console.WriteLine($"{_countryOne} -- Победила.");
            }
            else if (_squadOne.GetTotalHealth() < _squadTwo.GetTotalHealth())
            {

                Console.WriteLine($"{_countryTwo} -- Победила.");
            }
            else
            {
                Console.WriteLine("fff");
            }
        }
    }

    class SoldierFactory
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public List<Soldier> CreateMedic(Squad squad)
        {
            int minMedic = 5;
            int maxMedic = 50;

            int minDamage = 5;
            int maxDamage = 80;

            int maxHealth = 800;

            int damage = RandomGenerator.Next(minDamage, maxDamage);
            int health = RandomGenerator.Next(maxHealth);

            int medicSkill = RandomGenerator.Next(minMedic, maxMedic);
            _soldiers.Add(new Medic(health, damage, medicSkill, squad));

            return _soldiers;
        }

        public List<Soldier> CreateArcher()
        {
            int minArcher = 10;
            int maxArcher = 90;

            int minDamage = 10;
            int maxDamage = 90;

            int maxHealth = 900;

            int damage = RandomGenerator.Next(minDamage, maxDamage);
            int health = RandomGenerator.Next(maxHealth);

            int archerSkill = RandomGenerator.Next(minArcher, maxArcher);
            _soldiers.Add(new Archer(health, damage, archerSkill));

            return _soldiers;
        }
    }

    class Squad
    {
        private List<Soldier> _soldiers = new List<Soldier>();
        private SoldierFactory _soldierFactory = new SoldierFactory();

        public Squad()
        {
            _soldiers.AddRange(_soldierFactory.CreateArcher());
            _soldiers.AddRange(_soldierFactory.CreateMedic(this));
        }

        public int GetTotalHealth()
        {
            int totalHealth = 0;

            foreach (Soldier soldier in _soldiers)
                totalHealth += soldier.Health;

            return totalHealth;
        }

        public void RemoveSoldiers(Soldier soldiers)
        {
            _soldiers.RemoveAll(soldier => soldier.Health == 0);
            Console.WriteLine($"{_soldiers.Count} -- выбыло солдат.");
        }

        public void ShowSoldiers()
        {
            int soldierCount = 0;

            foreach (Soldier soldier in _soldiers)
            {
                soldierCount++;
                Console.WriteLine($"Здоровье -- {soldier.Health}.\n" +
                                  $"Атака -- {soldier.SpecialAttack}");
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

        public bool Contains(Soldier soldier)
        {
            return _soldiers.Contains(soldier);
        }
    }

    class Soldier
    {
        private List<SoldierFactory> _soldiers = new List<SoldierFactory>();

        public Soldier(int health, int damage)
        {
            Health = health;
            Damage = damage;
        }

        public int Health { get; set; }
        public int Damage { get; protected set; }
        public int SpecialAttack => Damage;
        public bool isAlive => Health > 0;

        public virtual int TakeDamage(int damage)
        {
            if (isAlive)
                Health -= damage;
            return Damage;
        }

        public virtual int UseSpecialAttack(Soldier opponent)
        {
            opponent.TakeDamage(Damage);
            return SpecialAttack;
        }
    }

    class Medic : Soldier
    {
        private Squad _squad;

        public Medic(int health, int damage, int quanity, Squad squad) : base(health, damage)
        {
            _squad = squad;
        }

        public int Healing { get; private set; } = 50;

        public override int UseSpecialAttack(Soldier soldier)
        {
            if (_squad.Contains(soldier))
            {
                int healAmount = Healing;
                soldier.Health += healAmount;
            }

            return Healing;
        }

        public override int TakeDamage(int damage)
        {
            return base.TakeDamage(damage);
        }
    }

    class Archer : Soldier
    {

        public Archer(int health, int damage, int quanity) : base(health, damage) { }

        public int LongRangeShot { get; private set; } = 140;

        public override int UseSpecialAttack(Soldier soldier)
        {
            return LongRangeShot;
        }

        public override int TakeDamage(int damage)
        {
            return base.TakeDamage(damage);
        }
    }
}
