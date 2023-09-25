using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork2
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public static class RandomGenerator
    {
        private static Random s_random = new Random();

        public static int Next(int minimum, int maximum) => s_random.Next(minimum, maximum);
    }

    class Battlefield
    {
        private Squard _squardOne = new Squard();
        private Squard _squardTwo = new Squard();
        private Soldier _firstSoldier;
        private Soldier _secondSoldier;

        public void Field()
        {
            int firstCounty = _squardOne.GetRandomSoldier();
            int secondCounty = _squardTwo.GetRandomSoldier();

            while (firstCounty > 0 && secondCounty > 0)
            {

            }
        }
    }

    class Squard
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public void RemoveSoldiers(Soldier soldier)
        {
            _soldiers.Remove(soldier);
        }

        public void ShowSoldiers()
        {
            foreach (Soldier soldier in _soldiers)
            {
                Console.WriteLine($"{soldier.Health} {soldier.SpecialAttack}");
            }
        }

        public int GetRandomSoldier()
        {
            int index = RandomGenerator.Next(0, _soldiers.Count);
            Soldier soldier = _soldiers[index];
            _soldiers.Add(soldier);
            return _soldiers.Count;
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

        public int Health { get; private set; }
        public int Damage { get; protected set; }
        public int SpecialAttack => Damage;
        public bool isAlive => Health > 0;

        public virtual void TakeDamage()
        {
            if (isAlive)
                Health -= Damage;
        }

        public virtual void Attack(Soldier opponent)
        {
            opponent.TakeDamage();
        }

        public virtual int UseSpecialAttack()
        {
            return SpecialAttack;
        }
    }

    class Medic : Soldier
    {
        private SoldierFactory _soldierFactory = new SoldierFactory();

        public Medic(int health, int damage,SoldierFactory soldiers) : base(health, damage)
        {
            _soldierFactory = soldiers;
        }

        public int Healing { get; private set; } = 100;

        public override int UseSpecialAttack()
        {
            return Damage *= Healing;
        }

        public override void Attack(Soldier opponent)
        {
            base.Attack(opponent);
            opponent.TakeDamage();
        }

        public void RecruitsMedics()
        {
            List<Soldier> _soldierFactories = _soldierFactory.CreateMedic();
            foreach (Soldier factory in _soldierFactories)
            _soldierFactories.Add(factory); 
        }
    }

    class Archer : Soldier
    {
        private SoldierFactory _soldierFactory = new SoldierFactory();

        public Archer(int health, int damage, SoldierFactory factory) : base(health, damage) 
        {
            _soldierFactory = factory;
        }

        public int LongRangeShot { get; private set; } = 140;

        public override int UseSpecialAttack()
        {
            return LongRangeShot;
        }

        public override void Attack(Soldier opponent)
        {
            base.Attack(opponent);
            opponent.TakeDamage(); 
        }

        public void RecruitsArchers()
        {
            _soldierFactory.CreateSoldier();
        }
    }

    class SoldierFactory
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public List<Soldier> CreateMedic()
        {
            int minMedic = 5;
            int maxMedic = 50;

            for (int i = 0; i < _soldiers.Count; i++)
            {
                int medicSkill = RandomGenerator.Next(minMedic, maxMedic);
                _soldiers.Add(new Medic(0, medicSkill, this));
            }

            return _soldiers;
        }

        public List<Soldier> CreateSoldier()
        {
            int minArcher = 10;
            int maxArcher = 90;

            for (int i = 0; i < _soldiers.Count; i++)
            {
                int archerSkill = RandomGenerator.Next(minArcher, maxArcher);
                _soldiers.Add(new Archer(0, archerSkill, this));
            }

            return _soldiers;
        }
    }
}
