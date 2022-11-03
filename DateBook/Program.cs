using System.Runtime.Serialization.Formatters.Binary;

namespace DateBook
{
    public class Program
    {
        #region Lists
        private static List<Note> Notes = new List<Note>() { };
        private static List<int> Numbers = new List<int>() { };
        #endregion
        #region GlobalVar
        private static DateTime date = DateTime.Now;
        private static int arrowPosition = 0;
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
            arrowPosition = 0;
            size = 0;
            noteOut();
        }
        private static void rightArrow()
        {
            date = date.AddDays(1);
            arrowPosition = 0;
            size = 0;
            noteOut();
        }
        private static void upArrow()
        {
            if (size >= 1 && arrowPosition > 1)
            {

                arrowPosition--;
                Console.Clear();
                noteOut();
                Console.SetCursorPosition(0, arrowPosition);
                Console.WriteLine("->");
            }
        }
        private static void downArrow()
        {
            if (size >= 1 && arrowPosition < size)
            {
                arrowPosition++;
                noteOut();
                Console.SetCursorPosition(0, arrowPosition);
                Console.WriteLine("->");
            }
        }
        private static void makeNote()
        {
            Console.Clear();
            Console.WriteLine($"Создание заметки на: {_date()}");
            Console.WriteLine("==================================");
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
            Notes.Add(new Note(Name, Description, DateComp, DateTime.Now, _date()));
            Console.Clear();
            noteOut();
        }
        private static string _date()
        {
            return $"{date.Day}.{date.Month}.{date.Year}";
        }
        private static void noteOut()
        {
            Console.Clear();
            Console.WriteLine($"Выбранная дата: {_date()}");
            Numbers.Clear();
            for (int k = 0; k < Notes.Count; k++)
            {
                if (Notes[k].date == _date())
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
            var binFormater = new BinaryFormatter();
            using (var file = new FileStream("saveNote.bin", FileMode.OpenOrCreate))
            {
                file.SetLength(0);
                binFormater.Serialize(file, Notes);
            }
            Console.Clear();
            Console.WriteLine("Заметки сохранены!\nНажмите любую клавишу для продолжения");
            userInputKey();
            noteOut();
        }
        private static void savesLoad()
        {
            if (File.Exists("saveNote.bin"))
            {
                Console.Clear();
                var binFormater = new BinaryFormatter();
                using (var file = new FileStream("saveNote.bin", FileMode.Open))
                {
                    var desNote = new List<Note> { };
                    try
                    {
                        desNote = binFormater.Deserialize(file) as List<Note>;
                    }
                    catch {desNote = null;}
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
            }
            else { Console.Clear(); Console.WriteLine("Сохранения не найдены!");}
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

                    if (arrowPosition > 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Название: {Notes[Numbers[arrowPosition - 1]].name}");
                        Console.ResetColor();
                        Console.WriteLine("=======================================");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Описание: {Notes[Numbers[arrowPosition - 1]].description}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Дата выполнения: {Notes[Numbers[arrowPosition - 1]].dateComp}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Дата создания заметки: {Notes[Numbers[arrowPosition - 1]].dateNow}");
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
                    if (arrowPosition > 0)
                    {
                        Notes.RemoveAt(Numbers[arrowPosition - 1]);
                        arrowPosition = 0;
                        size = 0;
                        noteOut();
                    }
                }
                else if (userKey.Key == ConsoleKey.F3) { saves(); }
                else if (userKey.Key == ConsoleKey.F4)
                {
                    savesLoad();
                    colorYellow();
                    Console.WriteLine("Для продолжения нажмите любую клавишу");
                    Console.ResetColor();
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F5)
                {
                    Console.Clear();
                    try
                    {
                        File.Encrypt("saveNote.bin");
                        Console.WriteLine("Сохранения зашифрованы!");
                    }
                    catch { Console.WriteLine("Сохранения не найдены!"); }
                    
                    colorYellow();
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.ResetColor();
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F6)
                {
                    Console.Clear();
                    try
                    {
                        File.Decrypt("saveNote.bin");
                        Console.WriteLine("Сохранения дешифрованы!");
                    }
                    catch { Console.WriteLine("Сохранения не найдены!");}
                    colorYellow();
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.ResetColor();
                    userInputKey();
                    noteOut();
                }
                else if (userKey.Key == ConsoleKey.F9)
                {
                    Console.Clear();
                    while (true)
                    {
                        Console.WriteLine("Вы действительно хотите удалить сохранения? (Да/Нет):");
                        string validation = userInput();
                        if (validation.ToLower() == "да")
                        {
                            File.Delete("saveNote.xml");
                            colorYellow();
                            Console.WriteLine("Сохранения удалены!\nНажмите любую клавишу для продолжения");
                            Console.ResetColor();
                            userInput();
                            noteOut();
                            break;

                        }
                        else if (validation.ToLower() == "нет") { noteOut(); break; }
                        else { Console.WriteLine("Введите либо \"Да\", либо \"Нет\""); break; }
                    }
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