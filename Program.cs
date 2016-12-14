using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        private static List<string> output = new List<string>();

        private static void WriteLine(string line) {
            Console.WriteLine(line);
            output.Add(line);
        }
        static void Main(string[] args)
        {
            foreach (string fn in System.IO.Directory.GetFiles("input"))
            {
                WriteLine("FILE " + fn);
                 
                if (System.IO.File.Exists(fn) == false)
                {
                    WriteLine("FILE " + fn + " does not exist");
                }

                string[] content = System.IO.File.ReadAllLines(fn);

                for (int line = 0; line < content.Length; line++)
                {
                    List<string> terms = new List<string>(content[line].Split(' ').Select(s => s.Trim()));

                    if (terms.Contains("n") && terms.Contains("|") == false)
                    {
                        WriteLine(content[line - 1].Trim() + "\t" + content[line]);

                    }

                }
                Console.WriteLine();
            }

            System.IO.File.WriteAllLines("output/filesummary.txt", output.ToArray());

            Console.WriteLine("PRESS any key to continue");
            Console.ReadKey();
        }
    }
}
