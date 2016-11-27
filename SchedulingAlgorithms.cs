﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication13
{
    public class SJFModel
    {
        public SJFModel(string processName, int ArriveTime, int BurstTime)
        {
            this.ProcessName = processName;
            this.ArriveTime = ArriveTime;
            this.BurstTime = BurstTime;
        }

        public string ProcessName { get; set; }
        public int ArriveTime { get; set; }
        public int BurstTime { get; set; }
        public int WT { get; set; } = 0;
        public int LastEnd { get; set; } = 0;
    }

    public class HashModel
    {
        public HashModel(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class SchedulingAlgorithms
    {
        public static void SJFPreemptive(IList<SJFModel> sjfModels)
        {
            string tempElementName = string.Empty;
            SJFModel tempElement;
            for (int i = 0;; i++)
            {

                tempElement =
                    sjfModels.Where(e => e.ArriveTime <= i && e.BurstTime > 0)
                        .OrderBy(e => e.BurstTime)
                        .FirstOrDefault();
                if (tempElement == null)
                {
                    Console.WriteLine($"|{0}|\t {i}");
                    break;
                }

                tempElement.BurstTime -= 1;
                //if (item.ArriveTime < counter) continue;
                if (tempElementName == tempElement.ProcessName) continue;

                if (i != 0)
                {
                    Console.WriteLine(
                        $"|{sjfModels.FirstOrDefault(x => x.ProcessName == tempElementName).BurstTime}|\t {i}");
                    sjfModels.FirstOrDefault(x => x.ProcessName == tempElementName).LastEnd = i;
                }

                Console.Write($"{i}\t({tempElement.ProcessName})");

                tempElementName = tempElement.ProcessName;

                tempElement.WT = tempElement.LastEnd == 0
                    ? i - tempElement.ArriveTime
                    : i - tempElement.LastEnd;
            }
            int sum = 0;
            sum = sjfModels.Sum(x => x.WT);
            //foreach (var item in sjfModels)
            //{
            //    sum += item.WT;
            //}
            Console.WriteLine($"AWT is = {(double) sum/sjfModels.Count} ");
        }

        public static void RoundRubin(List<HashModel> testHashModels, int quantum)
        {
            int tempbegin = 0;
            string highest = "";
            bool outx = false;
            highest = testHashModels.FirstOrDefault(x => x.Value == testHashModels.Max(p => p.Value)).Name;

            for (;;)
            {
                foreach (var item in testHashModels)
                {
                    if (item.Value < 1)
                        continue;
                    Console.Write(tempbegin + "\t");
                    Console.Write($"{item.Name} ({item.Value})");
                    if (item.Value > 0)
                        if (item.Value - quantum < 0)
                        {
                            tempbegin += item.Value;
                            item.Value -= item.Value;
                        }

                        else
                        {
                            item.Value -= quantum;
                            tempbegin += quantum;
                        }

                    Console.Write($"||{item.Value}\t");
                    Console.WriteLine(tempbegin);
                    if (item.Name == highest && item.Value < 1) outx = true;
                }
                if (outx) break;

            }
        }
    }

}
