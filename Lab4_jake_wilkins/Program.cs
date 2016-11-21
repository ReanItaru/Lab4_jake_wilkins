//********************************************************************************************************
// Lab 4 info space
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GDIDrawer;
using Utility_Library;
using System.IO;

namespace Lab4_jake_wilkins
{
    class Program
    {
        static void Main(string[] args)
        {
            //variables
            string choice = "";                     //input from user on what action they'd like to take
            int[] grades = new int[0];              //the array that will hold all the randomly generated grades
            CDrawer gdi;                            //the drawing window for displaying the histogram
            

            do
            {
                //title
                Console.WriteLine("\t\tLab4 - Array of Marks\n");
                Console.WriteLine("Actions available...\n");

                //options and input from user
                Console.Write("1. Create random array.\n2. Array stats.\n3. Draw histogram.\n4. Save array to file.\n5. Load array from file.\nq. Exit the program.\n\nYour selection: ");
                choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case ("1"):
                        grades = GetArray();
                        DisplayArray(grades);
                        Return();
                        break;

                    case ("2"):

                        //checks to see if an array has been created yet, give an error if it hasn't
                        if (grades.GetLength(0) == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\t\tYou have not created an array yet\n");
                            Console.Beep(37, 100);
                        }
                        else
                        {
                            Array.Sort(grades);
                            DisplayArray(grades);
                            Console.WriteLine("\n\nThe minimum value is {0}", grades.Min());
                            Console.WriteLine("The average value is {0}", grades.Average());
                            Console.WriteLine("The maximum value is {0}", grades.Max());
                            Return();
                        }
                        break;

                    case ("3"):
                        if (grades.GetLength(0) == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\t\tYou have not created an array yet\n");
                            Console.Beep(37, 100);
                        }
                        else
                        {
                            gdi = new CDrawer(800, 600, false);
                            DrawHistogram(gdi, grades);
                            Return();
                            gdi.Close();
                        }
                        break;

                    case ("4"):
                        if (grades.GetLength(0) == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\t\tYou have not created an array yet\n");
                            Console.Beep(37, 100);
                        }
                        else
                        {
                            SaveFile(grades);
                            Return();
                        }
                        break;

                    case ("5"):
                        LoadFile();
                        Return();
                        break;

                    case ("q"):
                        continue;

                    default:
                        Console.Clear();
                        Console.WriteLine("You have entered an invalid option\n");
                        break;
                }

            } while (choice != "q");

        }
        static public void Return()
        {
            Console.Write("\n\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        static public int[] GetArray()
        {
            //variables and input on how big the array should be
            int size = 0;
            Random rng = new Random();
            int[] array = new int[Utilities.GetInt("Enter the size of the array: ", 1, 10000)];

            //fills array with random values from 0-100
            for (size = 0; size < array.GetLength(0); size++)
                array[size] = rng.Next(101);

            return array;
        }
        static public void DisplayArray(int[] array)
        {
            int rowSize = 0;

            //displays the unsorted contents of the array
            Console.WriteLine("\nThe current contents of the array:\n");
            for (int i = 0; i < array.GetLength(0); i++)
                if (rowSize < 19)
                {
                    if (array[i] < 10)
                        Console.Write("{0}   ", array[i]);
                    else if (array[i] == 100)
                        Console.Write("{0} ", array[i]);
                    else
                        Console.Write("{0}  ", array[i]);
                    rowSize++;
                }
                else
                {
                    Console.WriteLine("{0}\n", array[i]);
                    rowSize = 0;
                }
        }
        static public void DrawHistogram(CDrawer gdi, int[] grades)
        {
            int width = 0;
            int height = 0;
            int textHeight = 0;
            double increments = 0.0;
            int lengthBar = 0;
            int[] groupedGrades = new int[11];              //each spot in the array stores one of the grouping of numbers
            Color[] barColour = new Color[11] { Color.Red, Color.Green, Color.Blue, Color.Teal, Color.Violet, Color.Yellow, Color.Orange, Color.HotPink, Color.Maroon, Color.Purple, Color.Gray };                    //each spot will have an individual colour assosiated with it
            int[] array = (int[])grades.Clone();            //cloned the parameter to prevent changing the original array
            string[] group = new string[11] { "0 to 9", "10 to 19", "20 to 29", "30 to 39", "40 to 49", "50 to 59", "60 to 69", "70 to 79", "80 to 89", "90 to 99", "100" };
            int count = 0;                                  //incrimental counter used to cycle through the full array
            gdi.Clear();

            //counting grades per group catagory: 0-9, 10-19, 20-29, ect with 100 being stand alone in the final group
            for (count = 0; count < array.GetLength(0); count++)
            {
                //simplifies the groups
                if (array[count] < 10)
                    array[count] = 1;
                else if (array[count] > 9 && array[count] < 20)
                    array[count] = 2;
                else if (array[count] > 19 && array[count] < 30)
                    array[count] = 3;
                else if (array[count] > 29 && array[count] < 40)
                    array[count] = 4;
                else if (array[count] > 39 && array[count] < 50)
                    array[count] = 5;
                else if (array[count] > 49 && array[count] < 60)
                    array[count] = 6;
                else if (array[count] > 59 && array[count] < 70)
                    array[count] = 7;
                else if (array[count] > 69 && array[count] < 80)
                    array[count] = 8;
                else if (array[count] > 79 && array[count] < 90)
                    array[count] = 9;
                else if (array[count] > 89 && array[count] < 100)
                    array[count] = 10;
                else if (array[count] > 99)
                    array[count] = 11;

                //determines size of the groups
                switch (array[count])
                {
                    case 1:
                        groupedGrades[0]++;
                        break;
                    case 2:
                        groupedGrades[1]++;
                        break;
                    case 3:
                        groupedGrades[2]++;
                        break;
                    case 4:
                        groupedGrades[3]++;
                        break;
                    case 5:
                        groupedGrades[4]++;
                        break;
                    case 6:
                        groupedGrades[5]++;
                        break;
                    case 7:
                        groupedGrades[6]++;
                        break;
                    case 8:
                        groupedGrades[7]++;
                        break;
                    case 9:
                        groupedGrades[8]++;
                        break;
                    case 10:
                        groupedGrades[9]++;
                        break;
                    case 11:
                        groupedGrades[10]++;
                        break;
                }
            }

            for (count = 0; count < groupedGrades.GetLength(0); count++)
            {
                width = count * 72;
                increments = 600.0 / groupedGrades.Max();
                lengthBar = (groupedGrades[count] * (int)(increments));

                if (groupedGrades[count] != 0)
                {
                    height = (580 - ((int)(increments) * groupedGrades[count]));
                    textHeight = ((height) + ((lengthBar) / 2));

                    gdi.AddRectangle(width, height, 72, lengthBar, barColour[count]);
                    gdi.AddText("" + groupedGrades[count], 13, width, textHeight, 72, 20, Color.Black);
                }
                else
                {
                    textHeight = 290;
                    gdi.AddText("" + groupedGrades[count], 13, width, textHeight, 72, 20, Color.White);
                }

                gdi.AddText(group[count], 10, width, 580, 72, 20, Color.White);

            }
            gdi.Render();
        }
        static public void SaveFile(int[] array)
        {
            string fileName = "";
            bool running = true;
            StreamWriter save;

            do
            {

                try
                {
                    Console.Write("Enter the name of the save file: ");
                    fileName = Console.ReadLine();

                    save = new StreamWriter(fileName);
                    for (int i = 0; i < array.GetLength(0); i++)
                        save.WriteLine(array[i]);                    

                    Console.WriteLine("\n\n{0} values saved to file {1}.", array.GetLength(0), fileName);
                    save.Close();
                    running = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The error, {0}, occured. Please try again.", ex.Message);
                    fileName = "";                    
                }
            } while (running);
        }
        static public void LoadFile()
        {
            StreamReader load;
            bool running = true;
            string fileName = "";
            int counter = 0;
            int[] array = new int[0];
            int num = 0;
            string number = "";
            int size = 0;

            do
            {
                try
                {
                    Console.Write("Enter the name of the file to be loaded: ");
                    fileName = Console.ReadLine();

                    load = new StreamReader(fileName);
                    

                    for (counter = 0; (number = load.ReadLine()) != null; counter++)
                    {
                        array = new int[counter];
                        array[counter] = int.Parse(number);
                    }                    

                    Console.WriteLine("{0} values were loaded from file {1}\n", array.GetLength(0), fileName);
                    DisplayArray(array);
                    running = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The error, {0}, occured. Please try again.", ex.Message);                    
                }

            } while (running);
            
        }
    }
}
