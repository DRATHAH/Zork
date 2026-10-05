using System;
using System.Collections.Generic;

namespace Zork
{
    class Program
    {
        private static readonly Dictionary<string, Room> roomMap;
        static Program()
        {
            roomMap = new Dictionary<string, Room>();
            foreach (Room room in Rooms)
            {
                roomMap[room.Name] = room;
            }
        }

        private static int locationRow = 1;
        private static int locationCol = 1;

        static void Main(string[] args)
        {
            string roomsFileName = "Rooms.txt";
            InitializeRoomDescriptions(roomsFileName);
            Console.WriteLine("Welcome to Zork!");

            Room previousRoom = null;
            Commands.CommandWords command = Commands.CommandWords.UNKNOWN;
            while (command != Commands.CommandWords.QUIT)
            {
                Console.WriteLine(Rooms[locationCol, locationRow]);
                if (previousRoom != Rooms[locationCol, locationRow])
                {
                    Console.WriteLine(Rooms[locationCol, locationRow].Description);
                    previousRoom = Rooms[locationCol, locationRow];
                }
                Console.Write("> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString = "";
                switch (command)
                {
                    case Commands.CommandWords.LOOK:
                        outputString = Rooms[locationCol, locationRow].Description;
                        break;

                    case Commands.CommandWords.QUIT:
                        outputString = "Thanks for playing!";
                        break;

                    case Commands.CommandWords.NORTH:
                    case Commands.CommandWords.SOUTH:
                    case Commands.CommandWords.EAST:
                    case Commands.CommandWords.WEST:
                        if (Move(command))
                        {
                            outputString = "You moved " + command.ToString();
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                            break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        static Commands.CommandWords ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands.CommandWords result) ? result : Commands.CommandWords.UNKNOWN;

        static bool Move(Commands.CommandWords direction)
        {
            switch (direction)
            {
                case Commands.CommandWords.WEST:
                    if (locationRow - 1 < 0)
                    {
                        return false;
                    }
                    locationRow--;
                    break;

                case Commands.CommandWords.EAST:
                    if (locationRow + 1 >= Rooms.GetLength(1))
                    {
                        return false;
                    }
                    locationRow++;
                    break;

                case Commands.CommandWords.NORTH:
                    if (locationCol + 1 >= Rooms.GetLength(0))
                    {
                        return false;
                    }
                    locationCol++;
                    break;

                case Commands.CommandWords.SOUTH:
                    if (locationCol - 1 < 0)
                    {
                        return false;
                    }
                    locationCol--;
                    break;

                default:
                    return false;
            }

            return true;
        }

        private enum Fields
        {
            Name = 0,
            Description
        }

        static void InitializeRoomDescriptions(string roomsFileName)
        {
            const string fieldDelimiter = "##";
            const int expectedFieldCount = 2;

            string[] lines = File.ReadAllLines(roomsFileName);
            foreach (string line in lines)
            {
                string[] fields = line.Split(fieldDelimiter);
                if (fields.Length != expectedFieldCount)
                {
                    throw new InvalidDataException("Invalid Record");
                }

                string name = fields[(int)Fields.Name];
                string description = fields[(int)Fields.Description];

                roomMap[name].Description = description;
            }
        }

        public static readonly Room[,] Rooms =
        {
            {new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View")},
            {new Room("Forest"), new Room("West of House"), new Room("Behind House")},
            {new Room("Dense Woods"), new Room("North of House"), new Room("Clearing")},
        };
    }
}
