using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telesoftas.Wrapper.Models;

namespace Telesoftas.Wrapper
{
    public class TextWrapper : ITextWrapper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void AppendOutputFile(IEnumerable<string> output)
        {
            Logger.Debug("Going to append output file with wrapped text");
            try
            {
                string currentDir = Path.Combine(Directory.GetCurrentDirectory(), "docs");
                using (StreamWriter sw = File.AppendText($"{currentDir}\\output.txt"))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("g")} wrapped text:");
                    foreach (var t in output)
                    {
                        sw.WriteLine(t);
                    }
                    sw.WriteLine($"{new string('-', output.FirstOrDefault().Length)}\n\n");
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Failed to append to output file");
                throw;
            }
        }

        public WrapperModel PrepareWrapperModel(string[] args)
        {
            WrapperModel model = new WrapperModel();

            string argText = string.Empty;
            string argLength = string.Empty;

            int indexL = Array.IndexOf(args, args.Where(x => x.Equals("-l"))?.FirstOrDefault());
            int indexT = Array.IndexOf(args, args.Where(x => x.Equals("-t"))?.FirstOrDefault());
            if (indexT > -1)
            {
                argText = args.ElementAtOrDefault(indexT + 1);
            }
            if (indexL > -1)
            {
                argLength = args.ElementAtOrDefault(indexL + 1);
            }

            model.Text = !string.IsNullOrEmpty(argText) ? argText : ReadInputFile();

            int dummy;
            if (int.TryParse(argLength, out dummy) && dummy > 0)
            {
                model.Length = dummy;
            }
            else
            {
                throw new IndexOutOfRangeException("Line length is incorrect");
            }

            if (string.IsNullOrEmpty(model.Text))
            {
                throw new IndexOutOfRangeException("Text to wrap was not set");
            }

            Logger.Debug("Prepared model:");
            Logger.Debug($"Line length:     {model.Length}");
            Logger.Debug($"Text:            {model.Text}");

            return model;
        }

        public string ReadInputFile()
        {
            Logger.Debug("Going to read text from input file");
            try
            {
                string currentDir = Path.Combine(Directory.GetCurrentDirectory(), "docs");
                return File.ReadAllText($"{currentDir}\\input.txt");

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Failed to read text from input file");
                throw;
            }
        }

        public IEnumerable<string> WrapText(WrapperModel model)
        {
            List<string> wrapped = new List<string>();


            int currentIndex = 0;
            int lastIndex = 0;
            char[] spaces = new[] { ' ', '\r', '\n', '\t' };
            char[] newLines = new[] { '\r', '\n' };

            if (model.Length > model.Text.Trim(newLines).Length)
            {
                wrapped = model.Text.Split(newLines).Where(x => x != string.Empty).ToList();
            }
            else
            {
                while (currentIndex < model.Text.Length)
                {
                    currentIndex = lastIndex + model.Length > model.Text.Length ? model.Text.Length : model.Text.LastIndexOfAny(spaces, Math.Min(model.Text.Length - 1, lastIndex + model.Length)) + 1;
                    if (currentIndex <= lastIndex)
                    {
                        currentIndex = Math.Min(lastIndex + model.Length, model.Text.Length);
                    }

                    wrapped.Add(model.Text.Substring(lastIndex, currentIndex - lastIndex).Trim(spaces));
                    lastIndex = currentIndex;
                }
            }

            AppendOutputFile(wrapped);
            return wrapped;
        }
    }
}
