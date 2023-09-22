using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;

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
        public void Field()
        {
            int firstCounty = 0;
            int secondCounty = 0;

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

        public virtual List<SoldierFactory> GetSoldier() 
        {
            SoldierFactory soldier = new SoldierFactory();
            soldier.CreateMedic();
            return _soldiers;
        }
    }
     
    class Medic : Soldier
    {
        private List<SoldierFactory> _soldiers = new List<SoldierFactory>();

        public Medic(int health, int damage) : base(health, damage) { }

        public int Healing { get; private set; } = 100;

        public override int UseSpecialAttack()
        {
            return Damage *= Healing;
        }

        public override List<SoldierFactory> GetSoldier()
        {
            return base.GetSoldier();
        }
    }

    class Sniper : Soldier
    {
        private List<SoldierFactory> _soldires = new List<SoldierFactory>();

        public Sniper(int health, int damage) : base(health, damage) { }

        public int LongRangeShot { get; private set; } = 140;

        public override int UseSpecialAttack()
        {
            return LongRangeShot;
        }

        public override List<SoldierFactory> GetSoldier()
        {
            return base.GetSoldier();
        }
    }

    class SoldierFactory
    {
        private List<Soldier> _soldiers = new List<Soldier>();

        public List<Soldier> CreateMedic()
        {
            int minMedic = 20;
            int maxMedic = 100; 

            for (int i = 0; i < _soldiers.Count; i++)
            {
                int medicSkill = RandomGenerator.Next(minMedic, maxMedic);
                Soldier soldier = new Soldier(0, medicSkill);
                _soldiers.Add(soldier);
            }

            return _soldiers;
        }

        public List<Soldier> CreateSoldier()
        {
            int minSniper = 20;
            int maxSniper = 100;

            for (int i = 0; i < _soldiers.Count; i++)
            {
                int sniperSkill = RandomGenerator.Next(minSniper, maxSniper);
                Soldier soldier = new Soldier(0, sniperSkill);
                _soldiers.Add(soldier);
            }

            return _soldiers;
        }
    }
}
