using NavigationAlertLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAlert
{
    // This program has some assumptions -
    // 1. The start direction is East
    // 2. The person can not go back , he can go only to straight, right , left, slight right, slight left
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var filePath = @"D:\\Projects\\CSharp\\NavigationAlert\\NavigationAlert\\TestFiles\\input.csv";
                var speed = 10; // In km/hr

                MainMethods.AlertPath(filePath, speed);
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine("Not able to Alert. Exception : " + e.StackTrace);
            }
        }

    }
}
