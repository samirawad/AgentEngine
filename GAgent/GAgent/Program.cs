using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            GameWorld WorldEngine = new GameWorld();
            while (!exit)
            {
                if (!exit)
                {
                    // Show the possible actions
                    Console.WriteLine(WorldEngine.LastOutcome);
                    string validEvents = WorldEngine.GetValidEvents();

                    // If there is only one possible valid event, it's detail and potential outcomes are displayed instead.
                    // We just need the user to press a key to continue.

                    if(WorldEngine.CurrentValidEvents.Count > 1)
                    {
                        Console.WriteLine(validEvents);
                        var currCommand = Console.ReadKey(true);
                        exit = currCommand.Key == ConsoleKey.Escape ? true : false;
                        if (exit) break;
                        var commandChar = currCommand.KeyChar;
                        if (WorldEngine.IsEventValid(commandChar))
                        {
                            Console.WriteLine(WorldEngine.ListEventOutcomes(commandChar));
                            Console.WriteLine("--------------------------------------------------------------");
                            Console.WriteLine("Press enter to execute this event, any other key to abort.");
                            currCommand = Console.ReadKey(true);
                            if (currCommand.Key == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                WorldEngine.DoEvent(commandChar);
                            }
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }
                    else // only one valid event.  Show description, detail, possible outcomes, then just do it.
                    {
                        Console.WriteLine(WorldEngine.CurrentValidEvents.First().Value.Description(WorldEngine));
                        Console.WriteLine(WorldEngine.ListEventOutcomes(WorldEngine.CurrentValidEvents.First().Key));
                        Console.WriteLine("--------------------------------------------------------------");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey(true);
                        WorldEngine.DoEvent(WorldEngine.CurrentValidEvents.First().Key);
                    }
                    
                }
            }
        }
    }
}
