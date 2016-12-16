using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace ConsoleApplication1
{
    class Program
    {
        class ValuesPerStratum
        {
            public string Stratum;
            public string Effort;
            public string samples;
            public string width;
            public string n;
            public string D;
            public string SE;
            public string CV;
            public string LCL;
            public string UCL;
            public string df;

            public ValuesPerStratum()
            {
                Stratum = Effort = samples = width = n = D = SE = CV = LCL = UCL = df = "-";
            }
            public string OutputLine
            {
                get
                {
                    return Stratum + "\t" + Effort + "\t" + samples + "\t" + width + "\t" + n + "\t" + D + "\t" + SE + "\t" + CV + "\t" + LCL + "\t" + UCL + "\t" + df;
                }
            }
        }
        private static Dictionary<string, ValuesPerStratum> valuesPerStratum = new Dictionary<string, ValuesPerStratum>();

        private static List<string> output = new List<string>();

        private static void WriteLine(string line) {
            Console.WriteLine(line);
            output.Add(line);
        }
         
        static void Main(string[] args)
        {
            string[] inputFiles = System.IO.Directory.GetFiles("input");

            foreach (string inputFile in inputFiles)
            {
                Console.WriteLine(inputFile);

                

                string[] lines = System.IO.File.ReadAllLines(inputFile);

                bool InDensitySection = false;

                bool InTable = false;
                bool InDensityAndAbundance = false;

                string Stratum = string.Empty;

                int lineNr=0;
                foreach (string line in lines)
                { 
                    
                    if (line.Contains("Density Estimates/"))
                    {
                        InDensitySection = true;
                    }
                    if (InDensitySection == true)
                    {
                        if (line.Contains("Density: Numbers/Sq. kilometers"))
                        {
                            InDensitySection = false;
                        }
                         
                        if (line.Contains("Stratum"))
                        {
                            Stratum  = line.Split(':')[1].Trim();
                            if (valuesPerStratum.ContainsKey(Stratum) == false)
                            {
                                valuesPerStratum.Add(Stratum, new ValuesPerStratum());
                                valuesPerStratum[Stratum].Stratum = Stratum;
                            }
                            

                        }
                        if (line.Contains("Effort"))
                        {
                            valuesPerStratum[Stratum].Effort= line.Split(':')[1].Trim();
                        }
                        if (line.Contains("# samples"))
                        {
                            valuesPerStratum[Stratum].samples = line.Split(':')[1].Trim();
                        }
                        if (line.Contains("Width"))
                        {
                            valuesPerStratum[Stratum].width = line.Split(':')[1].Trim();
                        }
                        if (line.Contains("# observations"))
                        {
                            valuesPerStratum[Stratum].n = line.Split(':')[1].Trim();
                        }
                        if(line.Contains("---------  -----------  -----------  --------------  ----------------------"))
                        {
                            if(InTable==true)InTable=false;
                            else InTable= true;
                        }
                        if(InTable==true)
                        {
                            if(line.Split(' ').Contains("D"))
                            {
                                List<string> terms = line.Split(' ').Where(t=> t.Trim()!= string.Empty).ToList();
                                valuesPerStratum[Stratum].D = terms[1];

                                double dD = double.MinValue;

                                if (double.TryParse(valuesPerStratum[Stratum].D, out dD) == true && dD != 0)
                                {
                                    valuesPerStratum[Stratum].SE = terms[2];
                                    valuesPerStratum[Stratum].CV = terms[3];
                                    valuesPerStratum[Stratum].LCL = terms[4];
                                    valuesPerStratum[Stratum].UCL = terms[5];
                                }

                                
                            }
                        }
                         
                        

                    }
                    if (line.Contains("Density&Abundance"))InDensityAndAbundance = true;

                    if (InDensityAndAbundance)
                    {
                        if (line.Contains("Stratum") == true)
                        {
                            Stratum = line.Split(':')[1].Trim();
                        }

                        if (int.Parse(valuesPerStratum[Stratum].n) > 1)
                        {
                            string[] terms = line.Split(' ').Where(t => t.Trim() != string.Empty).ToArray();
                            if (line.Split(' ').Contains("D") == true)
                            {
                                valuesPerStratum[Stratum].df = terms[3];

                            }
                        }
                        
                       
                    }

                    lineNr++;
                     
                }
                WriteLine("");
                WriteLine(inputFile);
                WriteLine("");
                WriteLine("Stratum\tEffort\t#samples\tWidth\tn\tD\tSE\tCV\t90%LCL\t90%UCL\tdf");
                foreach (string line in valuesPerStratum.Select(v => v.Value.OutputLine).ToList())
                {
                    WriteLine(line);
                }

                
                Console.WriteLine(); Console.WriteLine();


            }


            System.IO.File.WriteAllLines("output/filesummary.txt", output.ToArray());

            Console.WriteLine("PRESS any key to continue");
            Console.ReadKey();
        }
    }
}
