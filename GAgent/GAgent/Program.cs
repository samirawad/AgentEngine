﻿using System;
using System.Collections.Generic;
using System.Dynamic;
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
                    Console.WriteLine(WorldEngine.LastOutcomeLog);
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
                                WorldEngine.DoGameAction(commandChar);
                            }
                        }
                        else
                        {
                            Console.Clear();
                        }
                    }
                    // only one valid event.  Show description, detail, possible outcomes, then just do it.
                    else if(WorldEngine.CurrentValidEvents.Count == 1)
                    {
                        Console.WriteLine(WorldEngine.CurrentValidEvents.First().Value.Description(WorldEngine));
                        Console.WriteLine(WorldEngine.ListEventOutcomes(WorldEngine.CurrentValidEvents.First().Key));
                        Console.WriteLine("--------------------------------------------------------------");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey(true);
                        WorldEngine.DoGameAction(WorldEngine.CurrentValidEvents.First().Key);
                    
                    }  
                    else // There is probably something wrong.
                    {
                        Console.WriteLine("There are no valid events!");
                        Console.ReadKey(true);
                    }
                    
                }
            }
        }
    }
}
