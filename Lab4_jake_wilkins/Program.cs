//**************************************************************************************************************
//Program:      Array of Marks 
//Descriptions: Creates an array with a size dictated by user input up to 10000, fills the array with random 
//              grade values from 0 - 100, then gives the user several options to play around with the array
//              from sorting, displaying it as a bar graph/histograph and even saving it to a text file
//Lab:          4
//Date:         November 21st, 2016
//Author:       Jake Wilkins
//Class:        CMPE1300 A01
//Instructor:   JD Silver
//**************************************************************************************************************
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
            string choice = "";                     //input from user on what action they'd like to take
            int[] grades = new int[0];              //the array that will hold all the randomly generated grades
            CDrawer gdi;                            //the drawing window for displaying the histogram   
            bool error = false;                     //used to confirm if an array has been created or not

            //repeats program and menu options until letter q is pressed
            do
            {
                //title
                Console.WriteLine("\t\tLab4 - Array of Marks\n");
                Console.WriteLine("Actions available...\n");

                //options and input from user
                Console.Write("1. Create random array.\n2. Array stats.\n3. Draw histogram.\n4. Save array to file.");
                Console.Write("\n5. Load array from file.\nq. Exit the program.\n\nYour selection: ");
                choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    //creates array with as large as the user desires, fills with random grade values then displays it
                    case ("1"):
                        grades = GetArray();
                        DisplayArray(grades);
                        Refresh();
                        break;

                    //sorts array then displays some simple stats about it
                    case ("2"):
                        if (error = ArrayCheck(grades))
                            break;
                        Array.Sort(grades);
                        DisplayArray(grades);
                        Console.WriteLine("\nThe minimum value is {0}", grades.Min());
                        Console.WriteLine("The average value is {0}", grades.Average());
                        Console.WriteLine("The maximum value is {0}", grades.Max());
                        Refresh();
                        break;

                    //creates histogram based on the array
                    case ("3"):
                        if (error = ArrayCheck(grades))
                            break;
                        gdi = new CDrawer(800, 600, false);
                        DrawHistogram(gdi, grades);
                        Refresh();
                        gdi.Close();                        
                        break;

                    //stores the array as a file
                    case ("4"):
                        if (error = ArrayCheck(grades))
                            break;
                        SaveFile(grades);
                        Refresh();                        
                        break;

                    //loads a file, designed only to work with the file save formatting of the SafeFile() method
                    case ("5"):
                        LoadFile();
                        Refresh();
                        break;

                    case ("q"):
                        continue;

                    default:
                        Console.Clear();
                        Console.WriteLine("\tYou have entered an invalid option\n");
                        break;
                }

            } while (choice != "q");

        }
        //checks program to see if an array has been created or not, prevents errors from occuring within menu
        static public bool ArrayCheck(int[] array)
        {
            bool error = false;             //assumes an array was made, but will change should if statement be entered

            if (array.GetLength(0) == 0)
            {
                Console.Clear();
                Console.WriteLine("\tYou have not created an array yet\n");
                Console.Beep();
                error = true;
            }
            return error;

        }
        //used to pause then clean up the screen between option prompts
        static public void Refresh()
        {
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        static public int[] GetArray()
        {
            int size = 0;                                                                       //proves the index position for array
            Random rng = new Random();                                                          //used to produce a random grade
            int[] array = new int[Utilities.GetInt("Enter the size of the array: ", 1, 10000)]; //takes input to determine size of array

            //fills each spot in array with random values from 0-100
            for (size = 0; size < array.GetLength(0); size++)
                array[size] = rng.Next(101);

            return array;
        }
        static public void DisplayArray(int[] array)
        {
            int rowSize = 0;        //used to ensure each row has only 20 number in it

            //displays the contents of the array in neat columns and rows regardless of how big or small the rng numbers are
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
            int width = 0;                              //starting position of the x-axis for each bar
            int height = 0;                             //starting position of the y-axis for each bar
            int textHeight = 0;                         //determines where the text representing how big each bar is will go
            double increments = 0.0;                    //used to simplify the graphing math, MUST be double to minimize rounding errors
            int lengthBar = 0;                          //determines how long each bar is based on its groups size
            int count = 0;                              //incrimental counter used to cycle through the full array
            int[] groupedGrades = new int[11];          //each value in the array is the size of a grouping of numbers
            int[] array = (int[])grades.Clone();        //cloned the parameter to prevent changing the original array
            Color[] barColour;                          //each group will have an individual colour assosiated with it
            string[] group;                             //the groups that the paramter array will be sorted into
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
            
            //using the created parallel arrays does the actual drawing of the histogram
            for (count = 0; count < groupedGrades.GetLength(0); count++)
            {

                //any variables that were too long to define above or needed the array values != 0 to function
                group = new string[11] { "0 to 9", "10 to 19", "20 to 29", "30 to 39",
                    "40 to 49", "50 to 59", "60 to 69", "70 to 79", "80 to 89", "90 to 99", "100" };
                barColour = new Color[11] { Color.Magenta, Color.LightYellow, Color.LimeGreen, Color.LightSteelBlue,
                    Color.Sienna, Color.DarkOrchid, Color.DodgerBlue, Color.Goldenrod, Color.Maroon, Color.Indigo, Color.Khaki};
                width = count * 72;
                increments = 600.0 / groupedGrades.Max();
                lengthBar = (groupedGrades[count] * (int)(increments));

                //creates a special exception for groups that have no numbers in them
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
            StreamWriter save;          //saves the array to a file
            string fileName = "";       //input from the user what to save the file as
            bool error = false;         //used to check if an error occured

            //loops so long as no error was incurred
            do
            {

                try
                {
                    Console.Write("\nEnter the name of the save file: ");
                    fileName = Console.ReadLine() + ".txt";

                    //saves each value of the array on a new line in the txt file
                    save = new StreamWriter(fileName);
                    for (int i = 0; i < array.GetLength(0); i++)
                        save.WriteLine(array[i]);                    

                    Console.WriteLine("\n{0} values saved to file {1}.", array.GetLength(0), fileName);
                    save.Close();
                    error = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("I AM ERROR: {0}\n\nTry again.", ex.Message);
                    error = true;
                    fileName = "";                    
                }
            } while (error);
        }
        static public void LoadFile()
        {
            StreamReader load;              //loads previously saved array 
            bool running = true;            //used to check if error occured
            string fileName = "";           //input from user for file to load
            int counter = 0;                //used to count up during loops
            int[] array = new int[0];       //the array to be used to store the values found in the file
            string number = "";             //allows storage of what the ReadLine finds then allows conversion to int

            do
            {
                counter = 0;
                try
                {
                    Console.Write("\nEnter the name of the file to be loaded: ");
                    fileName = Console.ReadLine() + ".txt";

                    //finds number of numbers in file
                    load = new StreamReader(fileName);                    
                    while ((load.ReadLine()) != null)
                        counter++;
                    array = new int[counter];
                    counter = 0;

                    //assigns numbers in sequential order to the array
                    load = new StreamReader(fileName);
                    while ((number = load.ReadLine()) != null)
                    {
                        array[counter] = int.Parse(number);
                        counter++;
                    }                    

                    Console.WriteLine("\n{0} values were loaded from file {1}", array.GetLength(0), fileName);
                    DisplayArray(array);
                    running = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("I AM ERROR: {0}\n\nTry again.", ex.Message);                    
                }

            } while (running);
            
        }
    }
}
