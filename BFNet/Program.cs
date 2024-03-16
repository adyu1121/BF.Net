using BrainFuck;
using System;
using System.IO;
using static BrainFuck.BF;


namespace BFNet
{
    internal class Program
    {
        private const string version = "1.0";
        private const string help =
@"usage: BFNet [option] <FileName (=FolderName.bf)>

option list
new : Create a new .bf file
run : Run the .bf file to the end
debug : Start debug
    - step [count:int] : Proceed with steps equal to the [count].
    - memory [numder:int] : Prints the value of [numder]memory
    - end : Exit debug
    - run : Run the .bf file to the end
h/help : Print this help message
v/version : Print this app version";
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine(help);
                return;
            }
            if(args.Length > 2 )
            {
                Console.WriteLine("Too many arguments");
                return;
            }
            
            StreamIO standIO = new StreamIO(Console.In,Console.Out);
            BF bf = new BF(standIO.Input, standIO.Output);

            string path = Directory.GetCurrentDirectory();

            string fileName;
            if (args.Length == 1)
            {
                DirectoryInfo folderInfo = new DirectoryInfo(path);
                fileName = folderInfo.Name + ".bf";
            }
            else
            {
                fileName = args[1];
            }

            switch (args[0])
            {
                case "":
                    Console.WriteLine("Input option");
                    break;
                case "h": 
                case "help":
                    Console.WriteLine(help);
                    break;
                case "v":
                case "version":
                    Console.WriteLine(version);
                    break;

                case "new":
                    if(File.Exists(path + "\\" + fileName))
                    {
                        Console.WriteLine($"\"{path + "\\" + fileName}\" already exists");
                    }
                    else
                    {
                        File.Create(path + "\\" + fileName);
                        Console.WriteLine($"Create new file \"{path + "\\" + fileName}\"");
                    }
                    break;
                case "run":
                    if (File.Exists(path + "\\" + fileName))
                    {
                        bf.Code = File.ReadAllText(path + "\\" + fileName);
                        bf.Stap(-1);

                        BF.Error error = bf.GetLastError();
                        if (error.error != BF.ErrorrCode.CodeEnd)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"RunTime error : {error.error},Code Index : {error.index}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"File \"{fileName}\" is not found");
                    }
                    break;
                case "debug":
                    if(File.Exists(path + "\\" + fileName))
                    {
                        bf.Code = File.ReadAllText(path + "\\" + fileName);

                        string option;
                        bool result = true;

                        do
                        {
                            string[] options = Console.ReadLine().Split(' ');
                            int arg;

                            option = options[0];
                            if (options.Length <= 1)
                            {
                                arg = -1;
                            }
                            else
                            {
                                arg = int.Parse(options[1]);
                            }

                            switch (option)
                            {
                                case "":
                                    Console.WriteLine("Input option");
                                    break;
                                case "step":
                                    if(arg == -1)
                                    {
                                        Console.WriteLine("No argument for \"count\"");
                                        break;
                                    }
                                    result = bf.Stap(arg);
                                    break;
                                case "memory":
                                    if (arg == -1)
                                    {
                                        Console.WriteLine("No argument for \"number\"");
                                        break;
                                    }
                                    Console.WriteLine($"Memory {arg} : {bf[arg]}({(char)bf[arg]})");
                                    break;
                                case "run":
                                    result = bf.Stap(-1);
                                    break;
                                case "end": break;
                                default:
                                    Console.WriteLine($"\"{option}\" is not valid option, Type -h to see valid options");
                                    break;
                            }
                            if (!result)
                            {
                                Error error = bf.GetLastError();
                                if(error.error != ErrorrCode.CodeEnd)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"RunTime error : {error.error},Code Index : {error.index}");
                                }
                                break;
                            }
                        } while (option != "end");
                    }
                    else
                    {
                        Console.WriteLine($"File \"{fileName}\" is not found");
                    }
                    break;

                default:
                    Console.WriteLine($"\"{args[0]}\" is not valid option, Type -h to see valid options");
                    break;
            }
        }
    }
}
