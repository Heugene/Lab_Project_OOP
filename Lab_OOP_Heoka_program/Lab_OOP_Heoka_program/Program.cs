using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text.Json;

namespace Lab_OOP_Heoka_program
{
    public class Program
    {
        static List<SpaceShip> ships = new List<SpaceShip>();

        static int maxShipCount = 0; // максимальна кількість кораблів в колекції

        static List<string> shipNames;

        // Пул зброї для випадкової генерації, без додаткового хардкоду
        readonly static List<Weapon> presetWeapons = new List<Weapon>()
        {
            new Weapon("Plasmer", 20),
            new Weapon("Multicannon", 10),
            new Weapon("Missile launcher", 60),
            new Weapon("Railgun", 80),
            new Weapon("Beam cannon", 45),
            new Weapon("Cannon", 30)
        };

        // Для відновлення пулу назв для випадкової генерації, без додатквого хардкоду
        static void ShipNamesRefill()
        {
            shipNames = new List<string>()
            {
            "Aboba",
            "Amogus",
            "Catship",
            "Chayka",
            "Mriya",
            "Molfar",
            "Reni",
            "Brashovan",
            "Alice",
            "Kris S"
            };
        }


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.WriteLine("Вітаю! Це ЛР1 студента групи 612пст Геока Євгенія");
            ShipNamesRefill();
            ShipCountSetup(ref maxShipCount);
            MainMenuShow();
        }

        // показ головного меню. Рефакторинг не потрібен на даний момент
        static void MainMenuShow()
        {
            int menuChoice = 0;

            Console.Clear();
            Console.WriteLine($"Кораблів створено: {SpaceShip.ShipsSpawned}\n\n" +
                "ГОЛОВНЕ МЕНЮ\n" +
                "1 - Додати корабель вручну\n" +
                "2 - Додати автозгенеровані кораблі з випадковою конфігурацією\n" +
                "3 - Вивести на екран всі кораблі\n" +
                "4 - Знайти корабель\n" +
                "5 - Видалити корабель\n" +
                "6 - Демонстрація поведінки (Дуельний режим)\n" +
                "7 - Зберегти колекцію об'єктів у файлі\n" +
                "8 - Зчитати колекцію об'єктів з файлу\n" +
                "9 - Очистити колекцію об'єктів\n" +
                "0 - Вийти з програми\n" +
                "\nВведіть номер пункту меню для вибору: "
                );

            if (int.TryParse(Console.ReadLine(), out menuChoice))
            {
                switch (menuChoice)
                {
                    case 0:
                        { Environment.Exit(0); }
                        break;
                    case 1:
                        {
                            MenuShipAddManual();
                            MainMenuShow();
                        }
                        break;
                    case 2:
                        {
                            MenuShipAddAuto();
                            MainMenuShow();
                        }
                        break;
                    case 3:
                        {
                            MenuPrintShips();
                            MainMenuShow();
                        }
                        break;
                    case 4:
                        {
                            MenuShipSearch();
                            MainMenuShow();
                        }
                        break;
                    case 5:
                        {
                            MenuShipRemove();
                            MainMenuShow();
                        }
                        break;
                    case 6:
                        {
                            MenuDuelStart();
                            MainMenuShow();
                        }
                        break;
                    case 7:
                        {
                            MenuFileSave();
                            MainMenuShow();
                        }
                        break;
                    case 8:
                        {
                            MenuFileRead();
                            MainMenuShow();
                        }
                        break;
                    case 9:
                        {
                            MenuCollectionClear();
                            MainMenuShow();
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Введіть корректний номер пункту!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                            MainMenuShow();
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("Введіть корректний номер пункту!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                MainMenuShow();
            }
        }

        // встановлення максимальної кількості кораблів. Рефакторінг на даний момент не потрібен
        static void ShipCountSetup(ref int shipCount)
        {
            int SpecialSlots = 2; // Коли не треба, встановити значення 0.

            Console.WriteLine($"Задайте кількість кораблів (не менше 2 і не більше {shipNames.Count}. Додатково буде додано {SpecialSlots} спеціальні кораблі для демонстрації роботи перевантажених конструкторів):");

            if (int.TryParse(Console.ReadLine(), out shipCount))
            {
                if (shipCount < 2)
                {
                    shipCount = 2;
                    Console.WriteLine("Некоректна кількість кораблів! За замовчуванням встановлено 2.");
                    Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                }
                else if (shipCount > shipNames.Count)
                {
                    shipCount = shipNames.Count;
                    Console.WriteLine($"Некоректна кількість кораблів! За замовчуванням встановлено {shipNames.Count}.");
                    Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                }
                shipCount = shipCount;
                SpecialShipsAdd();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Введіть число!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                ShipCountSetup(ref shipCount);
            }
        }

        // ніде більше не використовувати! видалити за потреби. Додає два особливих корабля для демонстрації різних конструкторів.
        static void SpecialShipsAdd()
        {
            ShipType twinType = ShipType.Frigate;
            int twinHull = 10 * (int)(SpaceShip.maxHealth / 10 * 0.66);
            int twinRegen = (int)(SpaceShip.maxHealRate * 0.66);
            int tornadoDMG = (int)(Weapon.maxDamage * 0.66);
            List<Weapon> twinWeapons = new List<Weapon>() {
                new Weapon("Tornado", tornadoDMG),
                new Weapon("Tornado", tornadoDMG),
                new Weapon("Tornado", tornadoDMG),
            };

            Logger.General("Демонстрація роботи конструктора 1 без параметрів");
            SpaceShip Twin_A = new SpaceShip() { Name = "Twin A", Type = twinType, MaxHull = twinHull, Hull = twinHull, HealRate = twinRegen, State = ShipState.Alive };
            Twin_A.WeaponAdd(new Weapon("Tornado", tornadoDMG));
            Twin_A.WeaponAdd(new Weapon("Tornado", tornadoDMG));
            Twin_A.WeaponAdd(new Weapon("Tornado", tornadoDMG));
            Console.WriteLine($"Конфігурація новоствореного корабля: 1");
            ships.Add(Twin_A);
            Twin_A.Print();
            Console.WriteLine();

            Logger.General("Демонстрація роботи конструктора 3, який викликає 2, а той 1");
            SpaceShip Twin_B = new SpaceShip("Twin B", twinType, twinHull, twinRegen, twinWeapons);
            Console.WriteLine($"Конфігурація новоствореного корабля: 2");
            ships.Add(Twin_B);
            Twin_B.Print();
            Console.WriteLine();
        }

        // метод додавання корабля покроковий
        static SpaceShip ShipAddStepByStep()
        {
            // тимчасові змінні
            string shipName;
            int shipType;
            int shipHull;
            int shipHealRate;

            Console.WriteLine("ДОДАВАННЯ НОВОГО КОРАБЛЯ");

            while (true)
            {
                Console.WriteLine("Введіть назву (не менше 3 символів, тільки латиниця, не може починатися із цифри):");
                shipName = Console.ReadLine();
                if (Validator.IsNameValid(shipName))
                {
                    break;
                }
            }

            while (true)
            {
                StringBuilder info = new StringBuilder("Оберіть тип: (");
                ShipType[] shipTypes = (ShipType[])Enum.GetValues(typeof(ShipType));
                foreach (ShipType type in shipTypes)
                {
                    info.Append($"{(int)type} - {type}; ");
                }
                info.Append(")");

                Console.WriteLine(info);
                if (int.TryParse(Console.ReadLine(), out shipType))
                {
                    if (Validator.IsEnumValid(typeof(ShipType), shipType))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Введіть корректний номер пункту!");
                        Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                        Console.ReadKey();
                    }
                }
            }

            while (true)
            {
                Console.WriteLine($"Введіть кількісно міцність корпусу (не менше {SpaceShip.minHealth} і не більше {SpaceShip.maxHealth})");
                if (int.TryParse(Console.ReadLine(), out shipHull) && Validator.IsIntValid(shipHull, SpaceShip.minHealth, SpaceShip.maxHealth, null))
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine($"Введіть кількісно швидкість ремонту корпусу (не менше 0 і не більше {SpaceShip.maxHealRate} очок на хід)");
                if (int.TryParse(Console.ReadLine(), out shipHealRate) && Validator.IsIntValid(shipHealRate, 0, SpaceShip.maxHealRate, null))
                {
                    break;
                }
            }

            SpaceShip newShip = new SpaceShip(shipName, (ShipType)(shipType), shipHull, shipHealRate);

            Console.WriteLine("ДОДАВАННЯ ОЗБРОЄННЯ У СЛОТИ");
            Console.WriteLine($"Введіть латиницею інформацію про зброю у форматі <НАЗВА>_<ПОШКОДЖЕННЯ>, де назва не може починатися з цифри та містити менше 3 символів, а пошкодження не можуть перевищувати {Weapon.maxDamage}");
            Console.WriteLine("Наприклад, Plasmer_20");

            Weapon weapon; //тимчасова змінна
            for (int i = 0; i < newShip.WeaponHardpointCount; i++)
            {
                while (true)
                {
                    Console.WriteLine($"СЛОТ {i + 1}:");
                    string line = Console.ReadLine();
                    if (Weapon.TryParse(line, out weapon))
                    {
                        newShip.WeaponAdd(weapon);
                        break;
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Конфігурація новоствореного корабля:");
            newShip.Print();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();
            return newShip;
        }

        // метод додаванння корабля з рядка
        static SpaceShip ShipAddFromString()
        {
            StringBuilder prompt = new StringBuilder(
                               "ДОДАВАННЯ НОВОГО КОРАБЛЯ" +
                              "\n\nВведіть рядок формату НазваКорабля,Тип,МаксимальнаМіцністьКорпусу,ПоточнаМіцністьКорпусу,РемонтЗаХід,Стан,НазваЗброї1_Пошкодження/...НазваЗброїN_Пошкодження" +
                                "\nНаприклад, Parser,3,600,350,40,2,Test1_20/Test1_20/Test2_20/Test3_20" +
                              "\n\nПричому є такі правила:" +
                                "\nНазви не менше 3 символів, тільки латиниця, не можуть починатися з цифри;" +
                                $"\nМаксимальна міцність корпусу не менше {SpaceShip.minHealth} і не більше {SpaceShip.maxHealth};" +
                                "\nПоточна міцність корпусу не менше 0 і не більше максимальної для даного корабля;" +
                                $"\nРемонт за хід в діапазоні від 0 до {SpaceShip.maxHealRate}" +
                                $"\nПошкодження зброї від {Weapon.minDamage} до {Weapon.maxDamage}" +
                              "\n\nЕлементи enum такі:\n");
            prompt.AppendLine("Типи кораблів:");
            ShipType[] shipTypes = (ShipType[])Enum.GetValues(typeof(ShipType));
            foreach (ShipType type in shipTypes)
            {
                prompt.Append($"{(int)type} - {type}; ");
            }
            prompt.AppendLine(")");
            prompt.AppendLine("Стани корабля:");
            ShipState[] shipStates = (ShipState[])Enum.GetValues(typeof(ShipState));
            foreach (ShipState state in shipStates)
            {
                prompt.Append($"{(int)state} - {state}; ");
            }
            prompt.AppendLine(")\n\n");
            Console.WriteLine(prompt.ToString());

            SpaceShip newShip;
            while (!SpaceShip.TryParse(Console.ReadLine(), out newShip)) { }


            Console.Clear();
            Console.WriteLine("Конфігурація новоствореного корабля:");
            newShip.Print();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();
            return newShip;
        }

        // метод ручного додавання корабля, який викликається з головного меню та надає вибір між покроковим додаванням та додаванням з рядка
        static void MenuShipAddManual()
        {
            Console.Clear();
            if (ships.Count < maxShipCount)
            {
                Console.WriteLine("Додавання корабля в ручному режимі");
                Console.WriteLine("Ви бажаєте додати корабель покроково чи зчитати з рядка? 0 - Покроково, 1 - З рядка.");
                switch (Console.ReadLine())
                {
                    case "0":
                        { ships.Add(ShipAddStepByStep()); }
                        break;
                    case "1":
                        { ships.Add(ShipAddFromString()); }
                        break;
                    default:
                        {
                            Console.WriteLine("Введіть корректний номер пункту!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                        }
                        break;
                }
                MainMenuShow();
            }
            else
            {
                Console.WriteLine("Ви досягли встановленого ліміту на створення кораблів!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                MainMenuShow();
            }
        }

        // автоматична генерація рандомних кораблів у вільні слоти, рефакторінг був проведений.
        static void MenuShipAddAuto()
        {
            if (ships.Count < maxShipCount)
            {
                Console.Clear();
                Console.WriteLine($"Автоматична генерація кораблів у вільні слоти");
                Console.WriteLine();

                ShipNamesRefill();

                int N = maxShipCount - ships.Count;
                List<SpaceShip> Generated = SpaceShip.GenerateRandomShips(shipNames, presetWeapons, N);
                ships.AddRange(Generated);
                foreach (SpaceShip ship in Generated)
                {
                    ship.Print();
                    Console.WriteLine();
                }

                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ви досягли встановленого ліміту на кількість кораблів!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        // виведення у консоль всіх кораблів. Рефакторінг не треба
        static void MenuPrintShips()
        {
            Console.Clear();
            Console.WriteLine($"Кількість кораблів: {ships.Count}/{maxShipCount}");
            int i = 0;
            foreach (SpaceShip ship in ships)
            {
                Console.WriteLine($"Поточний індекс: {i}");
                i++;
                ship.Print();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        // метод пункту меню для пошуку кораблів за індексом чи назвою
        static List<SpaceShip> MenuShipSearch()
        {
            Console.Clear();
            Console.WriteLine("Введіть назву або індекс у списку корабля для пошуку");
            List<SpaceShip> results;

            string input = Console.ReadLine();
            results = ShipSearch(input, ships);
            if (results.Count > 0)
            {
                Console.WriteLine("Знайдені результати: ");
                foreach (SpaceShip ship in results)
                {
                    Console.WriteLine($"Поточний індекс: {ships.IndexOf(ship)}");
                    ship.Print();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("За даним пошуковим запитом результатів не знайдено!");
            }

            Console.WriteLine();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();

            return results;
        }

        // метод пошуку кораблів за індексом і назвою, повертає ліст з результатами. Був оптимізований під повторне використання в коді.
        public static List<SpaceShip> ShipSearch(string input, List<SpaceShip> spaceShips)
        {
            List<SpaceShip> results = new List<SpaceShip>();
            string name = input;
            int index;
            if (int.TryParse(name, out index))
            {
                // якщо введене число
                if ((index >= 0) && (index <= spaceShips.Count - 1))
                {
                    results.Add(spaceShips[index]);
                }
            }
            else
            {
                // якщо введений текст
                IEnumerable<SpaceShip> result = (spaceShips.FindAll(x => x.Name == name));
                if (result.Any())
                {
                    results = result.ToList();
                }
            }
            return results;
        }

        // метод видалення корабля за індексом або назвою, був перероблений із повторним використанням вже написаного функціоналу 
        static bool MenuShipRemove()
        {
            bool success = false;
            List<SpaceShip> foundShips;
            List<int> foundShipsIndexes = new List<int>();

            Console.Clear();
            Console.WriteLine("ВИДАЛЕННЯ");

            foundShips = MenuShipSearch();
            Console.Clear();

            if (foundShips.Count == 0)
            {
                // якщо нічого не знайшли
                return success;
            }
            else
            {
                // якщо щось знайшли
                Console.WriteLine("Знайдені результати для видалення:");

                foreach (SpaceShip ship in foundShips)
                {
                    foundShipsIndexes.Add(ships.IndexOf(ship));
                    Console.WriteLine($"Індекс у списку: {ships.IndexOf(ship)}");
                    ship.Print();
                    Console.WriteLine();
                }

                // якщо результатів кілька, вибираємо один
                if (foundShips.Count > 1)
                {
                    int index;
                    while (true)
                    {
                        Console.WriteLine("Введіть індекс одного з результатів на екрані, для видалення");

                        if (int.TryParse(Console.ReadLine(), out index) && foundShipsIndexes.Contains(index))
                        {
                            foundShips.Clear();
                            foundShips.Add(ships[index]);
                            break;
                        }
                    }
                }

                Console.WriteLine("Ви дійсно бажаєте видалити даний корабель? 1 - ТАК, будь-яка інша клавіша - НІ");
                if (Console.ReadLine() == "1")
                {
                    success = true;
                    ships.Remove(foundShips[0]);


                    Console.WriteLine();
                    Console.WriteLine("Обраний корабель було видалено...");
                    Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }

                return success;
            }
        }

        // метод запуску дуелі, повертає переможця
        static SpaceShip MenuDuelStart()
        {
            if (ships.Count < 2)
            {
                Console.WriteLine("Не можна почати дуель, так як було додано менше двох кораблів!");
                Console.WriteLine();
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                return null;
            }

            SpaceShip winner;

            Console.Clear();
            Console.WriteLine("ДУЕЛЬНИЙ РЕЖИМ");

            // виводимо список кораблів для ознайомлення чи ні
            int choice;
            Console.WriteLine("Вивести список наявних кораблів? 1 - ТАК");
            if (int.TryParse(Console.ReadLine(), out choice) && choice == 1)
            {
                Console.WriteLine($"Кількість кораблів: {ships.Count}/{maxShipCount}");
                foreach (SpaceShip ship in ships)
                {
                    Console.WriteLine($"Поточний індекс: {ships.IndexOf(ship)}");
                    ship.Print();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            // Вибираємо учасників дуелі
            Console.WriteLine("Оберіть введіть індекси двох кораблів для проведення дуелі.");
            int shipIndex1;
            int shipIndex2;

            SpaceShip[] duelists = new SpaceShip[2];

            while (true)
            {
                Console.WriteLine("Корабель 1:");
                if (int.TryParse(Console.ReadLine(), out shipIndex1) && (shipIndex1 >= 0) && (shipIndex1 <= ships.Count - 1))
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Корабель 2:");
                if (int.TryParse(Console.ReadLine(), out shipIndex2) && (shipIndex2 >= 0) && (shipIndex2 <= ships.Count - 1) && (shipIndex2 != shipIndex1))
                {
                    break;
                }
            }

            duelists[0] = ships[shipIndex1];
            duelists[1] = ships[shipIndex2];

            // Випадковим чином обираємо кожен хід, хто ходить першим, та симулюємо бій
            Random rand = new Random();
            int turn = 0;
            while (Array.TrueForAll(duelists, duelist => duelist.State != ShipState.Destroyed))
            {
                turn++;
                Logger.Info($"TURN {turn}");
                ships[shipIndex1].DeStun();
                ships[shipIndex2].DeStun();

                if (rand.Next(0, 2) == 1)
                {
                    Array.Reverse(duelists);
                }

                foreach (SpaceShip ship in duelists)
                {
                    if (ship.State != ShipState.Stunned)
                    {
                        SpaceShip opponent = duelists.Except(new List<SpaceShip>() { ship }).First();
                        opponent.DeStun();
                        ship.Attack(opponent);
                        ship.Heal(ship.HealRate);
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Thread.Sleep(1000);
            }

            winner = ships[shipIndex1].State == ShipState.Alive ? ships[shipIndex1] : ships[shipIndex2];

            winner.Heal();

            Console.WriteLine($"Дуель завершена. Переможець: {(winner.Name)}");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();

            return winner;
        }

        static void MenuFileSave()
        {
            Console.Clear();
            Console.WriteLine("ЗБЕРЕЖЕННЯ КОЛЕКЦІЇ У ФАЙЛ");

            Console.WriteLine("Введіть неповне ім'я файлу (тільки назву, буде доступний в одному каталозі з .ехе додатку)");
            Console.WriteLine("Введіть назву (не менше 3 символів, тільки латиниця, не може починатися із цифри):");
            Console.WriteLine($"Не може містити наступні заборонені символи: < > : {'"'} / {@"\"} | ? *");

            string fileName = Console.ReadLine();

            if (!Validator.IsFileNameValid(fileName))
            {
                Console.WriteLine("Введіть коректну назву файлу!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                MenuFileSave();
            }

            Console.WriteLine("Введіть 1 для збереження у формат .json, 2 - для збереження у формат .txt, 0 - для виходу");
            switch (Console.ReadLine())
            {
                case "0":
                    {
                        return;
                    }
                case "1":
                    {
                        if (CollectionSaveJSON(ships, fileName))
                        {
                            Console.WriteLine("Колекція успішно збережена!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Помилка збереження!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                        }
                    }
                    break;
                case "2":
                    {
                        if (CollectionSaveTXT(ships, fileName))
                        {
                            Console.WriteLine("Колекція успішно збережена!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Помилка збереження!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                        }
                    }
                    break;
                default:
                    {
                        Console.WriteLine("Введіть корректний номер пункту!");
                        Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                        Console.ReadKey();
                        MenuFileSave();
                    }
                    break;
            }
        }

        public static bool CollectionSaveJSON(List<SpaceShip> spaceShips, string fileName)
        {
            bool success = true;
            try
            {
                StringBuilder jsonstring = new StringBuilder();
                foreach (SpaceShip ship in spaceShips)
                {
                    jsonstring.AppendLine(JsonSerializer.Serialize<SpaceShip>(ship));
                }
                //jsonstring.AppendLine(JsonSerializer.Serialize<List<SpaceShip>>(spaceShips));

                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\{fileName}.json", jsonstring.ToString());
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public static bool CollectionSaveTXT(List<SpaceShip> spaceShips, string fileName)
        {
            bool success = true;
            try
            {
                File.WriteAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\{fileName}.txt", spaceShips.Select(x => x.ToString()));
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public static List<SpaceShip> CollectionReadJSON(string fileName)
        {
            List<SpaceShip> result = new List<SpaceShip>();
            string[] strings = File.ReadAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\{fileName}.json");

            foreach (string s in strings)
            {
                try
                {
                    result.Add(JsonSerializer.Deserialize<SpaceShip>(s));
                }
                catch
                {
                    // whatever
                }
            }

            return result;
        }

        public static List<SpaceShip> CollectionReadTXT(string fileName)
        {
            List<SpaceShip> result = new List<SpaceShip>();
            string[] strings = File.ReadAllLines($@"{AppDomain.CurrentDomain.BaseDirectory}\{fileName}.txt");
            SpaceShip current;
            foreach (string ship in strings)
            {
                if (SpaceShip.TryParse(ship, out current))
                {
                    result.Add(current);
                }
            }
            return result;
        }

        static void MenuFileRead()
        {
            Console.Clear();
            Console.WriteLine("ЗЧИТУВАННЯ КОЛЕКЦІЇ З ФАЙЛУ");

            Console.WriteLine("Введіть неповне ім'я файлу (тільки назву, має бути в одному каталозі з .ехе додатку)");
            Console.WriteLine("Введіть назву (не менше 3 символів, тільки латиниця, не може починатися із цифри):");
            Console.WriteLine($"Не може містити наступні заборонені символи: < > : {'"'} / {@"\"} | ? *");

            string fileName = Console.ReadLine();

            if (!Validator.IsFileNameValid(fileName))
            {
                Console.WriteLine("Введіть коректну назву файлу!");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
                MenuFileRead();
            }

            Console.WriteLine("Введіть 1 для зчитування з формату .json, 2 - для зчитування з формату .txt, 0 - для виходу");
            List<SpaceShip> shipsFromFile = new List<SpaceShip>();
            switch (Console.ReadLine())
            {
                case "0":
                    {
                        MainMenuShow();
                    }
                    break;
                case "1":
                    {
                        if (maxShipCount - ships.Count > 1)
                        {
                            try
                            {
                                shipsFromFile = CollectionReadJSON(fileName);
                            }
                            catch(Exception X)
                            {
                                Console.WriteLine($"Помилка зчитування з файлу! \n{X.Message}");
                                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                                Console.ReadKey();
                                MainMenuShow();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ви досягли встановленого ліміту на створення кораблів!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                            MainMenuShow();
                        }
                    }
                    break;
                case "2":
                    {
                        if (maxShipCount - ships.Count > 1)
                        {
                            try
                            {
                                shipsFromFile = CollectionReadTXT(fileName);
                            }
                            catch
                            {
                                Console.WriteLine("Помилка зчитування з файлу!");
                                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                                Console.ReadKey();
                                MainMenuShow();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ви досягли встановленого ліміту на створення кораблів!");
                            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                            Console.ReadKey();
                            MainMenuShow();
                        }
                    }
                    break;
                default:
                    {
                        Console.WriteLine("Введіть корректний номер пункту!");
                        Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                        Console.ReadKey();
                        MenuFileRead();
                    }
                    break;
            }

            if (maxShipCount - ships.Count < shipsFromFile.Count)
            {
                Console.WriteLine("Не всі кораблі були додані через брак місця в колекції!");
            }

            Console.WriteLine("ДОДАНІ КОРАБЛІ");
            int i = 0;
            while ((maxShipCount - ships.Count > 0) && (shipsFromFile.Count > i))
            { 
                ships.Add(shipsFromFile[i]);
                shipsFromFile[i].Print();
                Console.WriteLine("\n");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine("Натисність будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        static void MenuCollectionClear()
        {
            Console.Clear();
            Console.WriteLine("Ви дійсно бажаєте очистити колекцію кораблів? 1 - ТАК, будь-яка інша клавіша - НІ");
            if (Console.ReadLine() == "1")
            {
                ships.Clear();
                Console.WriteLine();
                Console.WriteLine("Колекцію кораблів було очищено...");
                Console.WriteLine("Натисність будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }
    }
}
