using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace DateBook
{
    public class Program
    {
        #region Lists
        private static List<Note> Notes = new List<Note>() { };
        private static List<string> Dates = new List<string>() { };
        private static List<int> Numbers = new List<int>() { };
        #endregion
        #region GlobalVar
        private static DateTime date = DateTime.Now;
        private static Note note = new Note();
        private static int position = 0;
        private static int size = 0;
        private static int Pos = 1;
        #endregion
        #region Methods
        private static void colorYellow()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        private static void colorGreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        private static void colorRed()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        private static string userInput()
        {
            return Console.ReadLine(); ;
        }
        private static ConsoleKeyInfo userInputKey()
        {
            return Console.ReadKey(true);
        }
        private static void leftArrow()
        {
            date = date.AddDays(-1);
            Numbers.Clear();
            position = 0;
            size = 0;
            noteOut();
        }
        private static void rightArrow()
        {
            date = date.AddDays(1);
            Numbers.Clear();
            position = 0;
            size = 0;
            noteOut();
        }
        private static void upArrow()
        {
            if (size >= 1 && position > 1)
            {

                position--;
                Console.Clear();
                noteOut();
                Console.SetCursorPosition(0, position);
                Console.WriteLine("->");
            }
        }
        private static void downArrow()
        {
            if (size >= 1 && position < size)
            {
                position++;
                noteOut();
                Console.SetCursorPosition(0, position);
                Console.WriteLine("->");
            }
        }
        private static void makeNote()
        {
            Console.Clear();
            Numbers.Clear();
            Console.WriteLine($"Создание заметки на: {Date()}");
            Console.WriteLine("==================================");
            Dates.Add(Date());
            colorYellow();
            Console.WriteLine("Введите название заметки: ");
            Console.ResetColor();
            string Name = userInput();
            colorGreen();
            Console.WriteLine("Введите описание:");
            Console.ResetColor();
            string Description = userInput();
            colorRed();
            Console.WriteLine("Введите дату выполнения: ");
            Console.ResetColor();
            string DateComp = userInput();
            Notes.Add(new Note(Name, Description, DateComp, DateTime.Now));
            Console.Clear();
            noteOut();
        }
        private static string Date()
        {
            return $"{date.Day}.{date.Month}.{date.Year}";
        }
        private static void noteOut()
        {
            Console.Clear();
            Console.WriteLine($"Выбранная дата: {Date()}");
            Numbers.Clear();
            for (int k = 0; k < Dates.Count; k++)
            {
                if (Dates[k] == Date())
                {
                    Numbers.Add(k);
                    Console.WriteLine("  " + Pos + "." + Notes[k].name);
                    size = Pos;
                    Pos++;

                }
            }
            Pos = 1;

        }
        private static void saves()
        {
            var xmlFormater = new XmlSerializer(typeof(List<Note>));
            var xmlFormater2 = new XmlSerializer(typeof(List<string>));
            using (var file = new FileStream("saveNote.xml", FileMode.OpenOrCreate))
            {
                file.SetLength(0);
                xmlFormater.Serialize(file, Notes);
            }
            using (var file = new FileStream("saveDate.xml", FileMode.OpenOrCreate))
            {
                file.SetLength(0);
                xmlFormater2.Serialize(file, Dates);
            }
        }
        private static void savesLoad()
        {
            Console.Clear();
            var xmlFormater = new XmlSerializer(typeof(List<Note>));
            var xmlFormater2 = new XmlSerializer(typeof(List<string>));
            using (var file = new FileStream("saveNote.xml", FileMode.OpenOrCreate))
            {
                var desNote = new List<Note> { };
                try
                {
                    desNote = xmlFormater.Deserialize(file) as List<Note>;
                }
                catch { Console.WriteLine("Сохранения не найдены!"); desNote = null; }
                if (desNote != null)
                {
                    Notes.Clear();
                    for (int i = 0; i < desNote.Count; i++)
                    {
                        Notes.Add(desNote[i]);
                    }
                    Console.WriteLine("Сохранения загружены!");
                }
            }
            using (var file = new FileStream("saveDate.xml", FileMode.OpenOrCreate))
            {
                var desNote = new List<string> { };
                try
                {
                    desNote = xmlFormater2.Deserialize(file) as List<string>;
                }
                catch { desNote = null; }
                if (desNote != null)
                {
                    Dates.Clear();
                    for (int i = 0; i < desNote.Count; i++)
                    {
                        Dates.Add(desNote[i]);
                    }
                }
            }
        }
        #endregion
        static void Main(string[] args)

        {
            noteOut();
            while (true)
            {
                ConsoleKeyInfo userKey = userInputKey();
                if (userKey.Key == ConsoleKey.RightArrow) { rightArrow(); }
                else if (userKey.Key == ConsoleKey.LeftArrow) { leftArrow(); }
                else if (userKey.Key == ConsoleKey.UpArrow) { upArrow(); }
                else if (userKey.Key == ConsoleKey.DownArrow) { downArrow(); }
                else if (userKey.Key == ConsoleKey.Enter)
                {

                    if (position > 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Название: {Notes[Numbers[position - 1]].name}");
                        Console.ResetColor();
                        Console.WriteLine("=======================================");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Описание: {Notes[Numbers[position - 1]].description}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Дата выполнения: {Notes[Numbers[position - 1]].dateComp}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Дата создания заметки: {Notes[Numbers[position - 1]].dateNow}");
                        Console.ResetColor();

                        while (true)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.Backspace)
                            {
                                Console.Clear();
                                noteOut();
                                break;
                            }
                        }
                    }


                }
                else if (userKey.Key == ConsoleKey.F1) { makeNote(); }
                else if (userKey.Key == ConsoleKey.F2)
                {
                    if (position > 0)
                    {
                        Notes.RemoveAt(Numbers[position - 1]);
                        Dates.RemoveAt(Numbers[position - 1]);
                        position = 0;
                        size = 0;
                        noteOut();
                    }
                }
                else if (userKey.Key == ConsoleKey.F3) { saves(); }
                else if (userKey.Key == ConsoleKey.F4)
                {
                    savesLoad();
                    Console.WriteLine("Для продолжения нажмите любую клавишу");
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F5)
                {
                    Console.WriteLine("Сохранения зашифрованы!");
                    File.Encrypt("saveNote.xml");
                    File.Encrypt("saveDate.xml");
                    colorYellow();
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.ResetColor();
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F6)
                {
                    Console.WriteLine("Сохранения дешифрованы!");
                    File.Decrypt("saveNote.xml");
                    File.Decrypt("saveDate.xml");
                    colorYellow();
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.ResetColor();
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F9)
                {
                    link1: Console.WriteLine("Вы действительно хотите удалить сохранения? (Да/Нет):");
                    string validation = userInput();
                    if(validation.ToLower() == "да")
                    {
                        File.Delete("saveDate.xml");
                        File.Delete("saveNote.xml");
                        Console.WriteLine("Сохранения удалены!\nНажмите любую клавишу для продолжения");
                        userInput();
                        noteOut();
                        
                    }
                    else if (validation.ToLower() == "нет") { noteOut(); }
                    else { Console.WriteLine("Введите либо \"Да\", либо \"Нет\""); goto link1; }
                }
                else if (userKey.Key == ConsoleKey.F10)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    Console.Write("Всего хорошего!");
                    Console.ResetColor();
                    break;
                }
                else
                {
                    continue;
                }
            }


        }

    }
}