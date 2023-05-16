using System.Reflection;
using System.Text.RegularExpressions;

namespace MrJumpscareApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal roundedConfigGrade = 0;
            decimal grade = 0;
            bool canUseConfig = true;
            List<decimal> values = new List<decimal>();
            try
            {
                string numberoutput = String.Empty;
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                string folderPath = Path.GetDirectoryName(myAssembly.Location);
                StreamReader sr = new StreamReader(folderPath + "/config.txt");
                string[] output = File.ReadAllLines(folderPath + "/config.txt");
                Regex rgx = new Regex(@"\d+");


                foreach (string str in output)
                {
                    string[] splittedoutput = Regex.Split(str, @"\D+");
                    foreach (var c in splittedoutput)
                    {
                        if (!string.IsNullOrEmpty(c))
                        {
                            decimal i = decimal.Parse(c);
                            values.Add(i);
                        }
                    }
                }
                values.ToArray();
                decimal configGrade = (values[1] / values[0]) * 9 + values[2];
                roundedConfigGrade = Math.Round(configGrade, 1);
                sr.Close();
            }
            catch (Exception e) {  canUseConfig = false; }

            bool confirmed = false;
            bool configChosen = false;
            bool secondTime = false;
            var currentTime = DateTime.Now;
            string Username = Environment.UserName;
            Console.WriteLine($"\nWelcome, {Username}, on {currentTime:d} at {currentTime:t}.");
            Console.WriteLine("This is a grade calculator, used to calculate your grade given the right inputs.\nIt can either use the config.txt file located in the folder or manual input.");
            ConsoleKey configChoice;
            Console.WriteLine("Do you want to use the config.txt input? If no, manual input will be selected. [y/n] ");
            configChoice = Console.ReadKey(false).Key;
            if (configChoice != ConsoleKey.Enter) Console.WriteLine();
            configChosen = configChoice == ConsoleKey.Y;

            do
            {
                if (!configChosen)
                {
                    do
                    {
                        Console.WriteLine("Please state your total points");
                        var totalPoints = Console.ReadLine();
                        Console.WriteLine("Please state your obtained points");
                        var obtPoints = Console.ReadLine();
                        Console.WriteLine("Please state your N-term.");
                        var nTerm = Console.ReadLine();
                        try
                        {
                            grade = Convert.ToDecimal(obtPoints) / Convert.ToDecimal(totalPoints) * 9 + Convert.ToDecimal(nTerm);
                            decimal roundedGrade = Math.Round(grade, 1);
                            Console.WriteLine($"You got a {roundedGrade}");
                        }
                        catch(Exception e) { Console.WriteLine($"{e.Message} Please return proper numbers!"); }
                        ConsoleKey response;
                        do
                        {
                            Console.Write("Do you want to calculate another grade in this console? [y/n] ");
                            response = Console.ReadKey(false).Key;
                            if (response != ConsoleKey.Enter)
                                Console.WriteLine();

                        } while (response != ConsoleKey.Y && response != ConsoleKey.N);
                        confirmed = response == ConsoleKey.Y;
                    } while (confirmed);
                    Exit();
                }
                else if (configChosen)
                {
                    if (canUseConfig)
                    {
                        Console.WriteLine($"You got a {roundedConfigGrade}");
                        ConsoleKey secondTimeKey;
                        Console.WriteLine("Do you want to calculate a different grade without config.txt? [y/n] ");
                        secondTimeKey = Console.ReadKey(false).Key;
                        if (secondTimeKey != ConsoleKey.Enter)
                            Console.WriteLine();
                        secondTime = secondTimeKey == ConsoleKey.Y;
                        if (!secondTime)
                        {
                            Exit();
                        }
                        else configChosen = false;
                    }
                    else 
                    {
                        Console.WriteLine("[ERROR] : Config not detected/complete. Returning to console input method. \nIf this is not wanted please fix config.txt and reopen the program.\n");
                        Thread.Sleep(1000);
                        secondTime = true;
                        configChosen = false;
                    }
                }
            } while (secondTime);
            void Exit()
            {
                Console.WriteLine("Exiting...");
                Thread.Sleep(1000);
                Environment.Exit(0);
            }
        }
    }
}

