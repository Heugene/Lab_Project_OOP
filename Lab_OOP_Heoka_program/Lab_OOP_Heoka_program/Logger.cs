using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_OOP_Heoka_program
{
    internal static class Logger
    {
        // Debug ( Debug.WriteLine($"Ship {Name} of type {Type} was destroyed.");)
        public static void General(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[General] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[Info] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Action(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Action] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Damage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Damage] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Repair(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Repair] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Stun(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Stun] - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
