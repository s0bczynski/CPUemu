using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CPUemu
{
    public static class CommandInterface
    {
        private static readonly Regex _commandRegex = new Regex(@"^/(\w+)\s+(\w+)(?:,\s+(\w+))?$", RegexOptions.IgnoreCase);

        public static void Start()
        {
            CommandsHandler.ViewWelcomeMessage();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (input == "/exit")
                {
                    break;
                }
                else if (input == "/reg")
                {
                    CommandsHandler.ViewReg();
                }
                else if (input == "/randomize")
                {
                    CommandsHandler.Randomize();
                }
                else if (input == "/help")
                {
                    CommandsHandler.Help();
                }
                else
                {
                    Match match = _commandRegex.Match(input);
                    if (match.Success)
                    {
                        string command = match.Groups[1].Value;
                        string arg1 = match.Groups[2].Value;
                        string arg2 = match.Groups[3].Value;

                        ExecuteCommand(command, arg1, arg2);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command");
                    }
                }
            }
        }

        private static void ExecuteCommand(string command, string arg1, string arg2)
        {
            arg1 = arg1.ToUpper();
            arg2 = arg2.ToUpper();
            switch (command)
            {
                case "mov":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Mov(arg1, arg2);
                    }
                    break;
                case "xchg":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Xchg(arg1, arg2);
                    }
                    break;
                case "add":
                    if(arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Add(arg1, arg2);
                    }
                    break;
                case "sub":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Sub(arg1, arg2);
                    }
                    break;
                case "or":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Or(arg1, arg2);
                    }
                    break;
                case "xor":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Xor(arg1, arg2);
                    }
                    break;
                case "and":
                    if (arg2 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.And(arg1, arg2);
                    }
                    break;
                case "not":
                    if (arg1 == null)
                    {
                        Console.WriteLine("Invalid arguments");
                    }
                    else
                    {
                        CommandsHandler.Not(arg1);
                    }
                    break;
                case "randomize":
                    CommandsHandler.Randomize();
                    break;
				case "inc":
					if (arg1 == null)
					{
						Console.WriteLine("Invalid arguments");
					}
					else
					{
						CommandsHandler.Inc(arg1);
					}
					break;
				case "dec":
					if (arg1 == null)
					{
						Console.WriteLine("Invalid arguments");
					}
					else
					{
						CommandsHandler.Dec(arg1);
					}
					break;
				default:
                    Console.WriteLine("Invalid command");
                    break;
            }
        }

    }
}