using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestEcoSphera
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = new List<string>(File.ReadAllLines("../../Database/File1.txt"));
            string name = lines[0];
            lines.RemoveAt(0);
            List<Bar> Bars = new List<Bar>();
            foreach (string line in lines)
            {
                string[] vs = line.Split(',');
                Bars.Add(new Bar()
                {
                    Symbol = vs[0],
                    Description = vs[1],
                    Date = DateTime.Parse(vs[2]),
                    Time = TimeSpan.Parse(vs[3]),
                    Open = decimal.Parse(vs[4].Replace('.', ',')),
                    High = decimal.Parse(vs[5].Replace('.', ',')),
                    Low = decimal.Parse(vs[6].Replace('.', ',')),
                    Close = decimal.Parse(vs[7].Replace('.', ',')),
                    TotalVolume = int.Parse(vs[8])
                });
            }

            #region Задание 1

            GetMaxMinForDayFile(Bars, name);

            #endregion

            #region Задание 2

            GetFileFromSource2(Bars, name);

            #endregion

            #region Задание 3

            List<string> lines2 = new List<string>(File.ReadAllLines("../../Database/File2.txt"));
            lines2.RemoveAt(0);
            using (StreamWriter sw = new StreamWriter("../../NewFiles/Новые Строки.txt", false))
            {
                sw.WriteLine(name);
                List<Bar> Bars2 = new List<Bar>();

                foreach (string line in lines2)
                {
                    string[] vs = line.Split(',');
                    Bars2.Add(new Bar()
                    {
                        Symbol = vs[0],
                        Description = vs[1],
                        Date = DateTime.Parse(vs[2]),
                        Time = TimeSpan.Parse(vs[3]),
                        Open = decimal.Parse(vs[4].Replace('.',',')),
                        High = decimal.Parse(vs[5].Replace('.', ',')),
                        Low = decimal.Parse(vs[6].Replace('.', ',')),
                        Close = decimal.Parse(vs[7].Replace('.', ',')),
                        TotalVolume = int.Parse(vs[8])
                    });

                }

                while (Bars.Count != 0)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        if (Bars[0].Date == Bars2[j].Date && Bars[0].Time == Bars2[j].Time)
                        {
                            Bars.Remove(Bars[0]);
                            Bars2.Remove(Bars2[j]);
                            break;
                        }
                    }
                }

                foreach (var bar2 in Bars2)
                {
                    sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", bar2.Symbol, bar2.Description, bar2.Date.ToString("d"), bar2.Time, bar2.Open, bar2.High, bar2.Low, bar2.Close, bar2.TotalVolume));
                }

            }
            
            #endregion

        }


        static void GetMaxMinForDayFile(List<Bar> Bars,string name)
        {
            using (StreamWriter sw = new StreamWriter("../../NewFiles/MaxMinLines.txt", false))
            {
                sw.WriteLine(name);
                Bar Maxbar = new Bar();
                Bar Minbar = new Bar();
                for (int i = 0; i < Bars.Count - 1; i++)
                {
                    if (Bars[i].Date == Bars[i + 1].Date)
                    {
                        if (Bars[i].High > Bars[i + 1].High)
                        {
                            Maxbar = Bars[i];
                        }

                        if (Bars[i].Low < Bars[i + 1].Low)
                        {
                            Minbar = Bars[i];
                        }
                    }
                    else
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Minbar.Symbol, Minbar.Description, Minbar.Date.ToString("d"), Minbar.Time, Minbar.Open, Minbar.High, Minbar.Low, Minbar.Close, Minbar.TotalVolume));
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Maxbar.Symbol, Maxbar.Description, Maxbar.Date.ToString("d"), Maxbar.Time, Maxbar.Open, Maxbar.High, Maxbar.Low, Maxbar.Close, Maxbar.TotalVolume));

                    }
                }
            }
        }

        static void GetFileFromSource2(List<Bar> Bars, string name)
        {
            using (StreamWriter sw = new StreamWriter("../../NewFiles/Задание2.txt", false))
            {
                sw.WriteLine(name);
                Bar newBar = new Bar();
                newBar.TotalVolume = 0;
                for (int i = 0; i < Bars.Count - 1; i++)
                {
                    newBar.TotalVolume += Bars[i].TotalVolume;
                    newBar.Date = Bars[i].Date;
                    if (newBar.High == 0 && newBar.Low == 0)
                    {
                        newBar.High = Bars[i].High;
                        newBar.Low = Bars[i].Low;
                    }
                    if (Bars[i].High > newBar.High)
                    {
                        newBar.High = Bars[i].High;
                    }
                    if (Bars[i].Low < newBar.Low)
                    {
                        newBar.Low = Bars[i].Low;
                    }
                    if (Bars[i].Time.Minutes == 0 || (Bars[i].Time.Hours == 8 && Bars[i].Time.Minutes == 1))
                    {
                        newBar.Symbol = Bars[i].Symbol;
                        newBar.Description = Bars[i].Description;
                        newBar.Time = Bars[i].Time;
                        newBar.Open = Bars[i].Open;
                    }
                    else if (Bars[i].Time.Minutes == 59)
                    {
                        newBar.Close = Bars[i].Close;
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", newBar.Symbol, newBar.Description, newBar.Date.ToString("d"), newBar.Time, newBar.Open, newBar.High, newBar.Low, newBar.Close, newBar.TotalVolume));
                        newBar = new Bar();
                        newBar.TotalVolume = 0;
                    }
                }
            }
        }
    }
}
