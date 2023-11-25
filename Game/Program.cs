using System.Buffers.Text;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Character> characters = new List<Character>();

            Warrior warrior = new Warrior()
            {
                Name = "Kratos",
                Health = 100,
                Strength = 20,
                AvailablePoints = 20,
                Armor = 50,
                Mana = 100
            };
            characters.Add(warrior);

            List<Character> character = new List<Character>();

            Warrior warrior1 = new Warrior()
            {
                Name = "Zeus",
                Health = 100,
                Strength = 10,
                AvailablePoints = 20,
                Armor = 70,
                Mana = 100
            };
            MonitorStats(characters);


            Character attack = warrior;
            Character targett = warrior1;

            while (attack.Health > 0 && targett.Health > 0)
            {
                attack.Attack(targett);
                Swap(ref attack, ref targett);
            }

            MonitorStats(characters);


            foreach (Character attacker in characters)
            {
                foreach (Character target in characters)
                {
                    if (attacker != target && attacker.Health > 0 && target.Health > 0)
                    {
                        attacker.Attack(target);
                    }
                }
            }

            List<ISkill> skillList = new List<ISkill>();
            skillList.Add((ISkill)warrior);
            skillList.Add((ISkill)warrior1);


            foreach (ISkill skill in skillList)

            {
                skill.UseSkill();
            }

            MonitorStats(characters);

        }
        static void MonitorStats(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                Console.WriteLine($"Character: {character.Name}");
                Console.WriteLine($"Health: {character.Health}");
                Console.WriteLine($"Available Points: {character.AvailablePoints}");
                Console.WriteLine();
            }
        }
        static void Swap(ref Character a, ref Character b)
        {
            Character temp = a;
            a = b;
            b = temp;
        }
        abstract class Character : IDisposable
        {
            public string Name { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int AvailablePoints { get; set; }
            public string Weapon { get; set; }
            public int IDisposable { get; set; }
            public Weapon EquippedWeapon { get; set; }


            public Character()
            {
                Name = "Unknown";
                Health = 100;
                Strength = 10;
                AvailablePoints = 0;
                Weapon = "M4A1";
                IDisposable = 1;

            }

            public Character(string name, int health, int strength, int availablePoints, string weapon)
            {
                Name = name;
                Health = health;
                Strength = strength;
                AvailablePoints = availablePoints;
                Weapon = weapon;
            }


            public abstract void Attack(Character target);

            public interface ISkill
            {
                void UseSkill();

            }
            public void FinishedWeapon()
            {
                if (Convert.ToInt32(Weapon) < 1)
                {
                    Console.WriteLine("Weapon is out of ammo");
                }
            }
            public void EqEquipWeapon(Weapon weapon)
            {
                using (EquippedWeapon = weapon)
                {

                }
            }
            public void Dispose()
            {

                EquippedWeapon?.Dispose();


                GC.SuppressFinalize(this);
            }
        }

        abstract class Weapon : Character, IDisposable
        {
            public int IDisposable;


            public Weapon(int idIsPosable)
            {
                IDisposable = idIsPosable;
            }
            public Weapon()
            {
                if (Health is 0)
                {
                    Weapon = null;
                }
            }
            public void Dispose()
            {

            }
        }

        class Warrior : Character
        {
            public int Armor { get; set; }
            public int Mana { get; internal set; }

            public Warrior()
            {
                Armor = 0;
            }
            public Warrior(string name, int health, int strength, int availablePoints, int armor, string weapon)
        : base(name, health, strength, availablePoints, weapon)
            {
                Armor = armor;
            }
            public override void Attack(Character target)
            {
                int bonusDamage = Armor > 50 ? 10 : 0;
                int totalDamage = Strength + bonusDamage;
                target.Health -= totalDamage;
                Console.WriteLine($"The warrior {Name} attacks with {totalDamage} damage.");
            }
            public void UseSkill()
            {
                if (AvailablePoints >= 5)
                {
                    Armor = +10;
                    AvailablePoints -= 5;
                    Console.WriteLine($"{Name} uses a skill to increase Armor by 10. AvailablePoints: {AvailablePoints}");
                }
                else
                {
                    Console.WriteLine($"Insufficient AvailablePoints to use the skill.");
                }
            }
        }
        abstract class Mage : Character

        {
            public int Mana { get; set; }
            public int Spell { get; set; }


            public Mage() : base()
            {
                Mana = 100;
                Spell = 40;
            }

            public Mage(string name, int health, int strength, int availablePoints, int mana, int spell, string weapon) : base(name, health, strength, availablePoints, weapon)
            {
                Mana = mana;
                Spell = spell;
            }
            public void UseSkill()
            {
                if (AvailablePoints >= 10)
                {
                    int Healing = Spell + Health;
                    int Attack = Spell + Strength;
                    AvailablePoints -= 10;

                    Console.WriteLine($"Spell can heal {Healing} Character and increase {Attack}");
                }
            }
        }
    }

    internal interface ISkill
    {
        void UseSkill();
    }
}