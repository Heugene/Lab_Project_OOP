using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_OOP_Heoka_program
{
    public class Weapon
    {
        public readonly static int maxDamage = 80;
        public readonly static int minDamage = 0;

        public string Name { get; private set; }
        public int Damage { get; private set; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        public static bool TryParse(string weapon, out Weapon result)
        {
            string[] weapon_parts = weapon.Split('_');
            string name = weapon_parts[0];
            int damage = 0;
            if ((weapon_parts.Length == 2) && Validator.IsNameValid(name) && int.TryParse(weapon_parts[1], out damage) && Validator.IsIntValid(damage, minDamage, maxDamage, null))
            {
                result = new Weapon(name, damage);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Name}_{Damage}";
        }

        //jkjkjkl
    }
}
