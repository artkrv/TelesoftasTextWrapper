using NLog;
using System;
using Telesoftas.Wrapper;
using Telesoftas.Wrapper.Models;

namespace Telesoftas
{
    class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                Logger.Debug("Wrapper strating");
                if (args == null)
                {
                    Logger.Warn("No arguments were passed. Algorithm must accept 2 arguments.");
                }
                Console.Title = "Telesoftas wrapper";
                Console.WriteLine("Wrapper is starting\n");

                TextWrapper wrapper = new TextWrapper();
                WrapperModel model = wrapper.PrepareWrapperModel(args);

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Line length:    {model.Length}");
                Console.WriteLine($"Original text:  {model.Text}");
                Console.ResetColor();

                var wrapped = wrapper.WrapText(model);

                Console.WriteLine("\nWrapped:");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                foreach (var l in wrapped)
                {
                    Console.WriteLine(l);
                }
                Console.ResetColor();

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Unexpected error fired.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"\nERROR: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Logger.Info("Application terminated.");
                Console.WriteLine("\nApplication terminated. Press <enter> to exit...");
                Console.ReadLine();
            }
        }
    }
}
