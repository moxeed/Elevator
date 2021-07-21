using System;
using System.Collections.Generic;

namespace Elevator
{
    class Program
    {
        static void Main()
        {
            var elevator = new Domain.Elevator(4);
            elevator.AddChangeListner(DrawElevator);
            elevator.Start();

            var levels = elevator.Levels;
            while(true)
            {
                DrawUI();
                var input = Console.ReadLine();
                if (input == "q")
                    break;

                if(int.TryParse(input, out int level))
                {
                    try {
                        levels[level - 1].RequestCar();
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Level");
                    }
                }
            }
            elevator.Stop();
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static void DrawElevator(int currentLevel, int? destination, IList<int> requests)
        {
            var (left, top) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 0);

            ClearCurrentConsoleLine();
            Console.WriteLine($"Current Level: {currentLevel}");

            ClearCurrentConsoleLine();
            Console.Write($"Requests: ");
            foreach(var request in requests)
            {
                Console.Write($"{request} ");
            }
            Console.WriteLine();

            ClearCurrentConsoleLine();
            Console.Write($"Destination: {destination}");
            Console.SetCursorPosition(left, top);
        }

        static void DrawUI()
        {
            Console.SetCursorPosition(0, 4);
            ClearCurrentConsoleLine();
            Console.Write("Enter Request Level :");
        }
    }
}
