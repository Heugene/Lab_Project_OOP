using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_OOP_Heoka_program
{
    public static class Validator
    {
        // функція перевірки валідності імен
        public static bool IsNameValid(string name)
        {
            string alphabet = "abcdefeghjiklmnopqrstuvwxyz";
            var Chars = name.ToLower().AsEnumerable().Where(X => char.IsLetter(X)).ToArray();
            if ((name != null) && (name.Length != 0) && char.IsLetter(name[0]) && Array.TrueForAll(Chars, x => alphabet.Contains(x)) && (name.Length >= 3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // функція перевірки валідності числового значення для еnum заданого типу
        public static bool IsEnumValid(Type enumType, int value)
        {
            if (((int[])Enum.GetValues(enumType)).Contains(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // функція перевірки валідності числового значення у заданих межах
        // Параметр bool? conditions потрібен для того, щоб визначити межі для перевірки
        // null = перевірка на входження у діапазон [min; max]
        // true = перевірка на входження у діапазон [min; int.MaxValue]
        // false = перевірка на входження у діапазон [int.MinValue; max]
        public static bool IsIntValid(int value, int min, int max, bool? conditions)
        {
            bool output = false;
            switch (conditions)
            {
                case null:
                    {
                        if ((value >= min) && (value <= max))
                        {
                            output = true;
                        }
                    }
                    break;
                case true:
                    {
                        if (value >= min)
                        {
                            output = true;
                        }
                    }
                    break;
                case false:
                    {
                        if (value <= max)
                        {
                            output = true;
                        }
                    }
                    break;
            }
            return output;
        }

        // функція перевірки валідності імені файлу
        public static bool IsFileNameValid(string fileName)
        {
            string bannedFileNameCharacters = new string(Path.GetInvalidFileNameChars());
            if (IsNameValid(fileName) && Array.TrueForAll(fileName.ToCharArray(), x => !bannedFileNameCharacters.Contains(x)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
