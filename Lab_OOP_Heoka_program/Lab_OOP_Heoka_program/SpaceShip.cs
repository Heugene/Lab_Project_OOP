using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json.Serialization;

namespace Lab_OOP_Heoka_program
{
    public class SpaceShip
    {
        // обмеження та інші константи
        public readonly static int minHealth = 400;
        public readonly static int maxHealth = 1000;
        public readonly static int maxHealRate = 40;
        private readonly static int stunChance = 15; // шанс оглушення
        private static Random random = new Random(DateTime.Now.Millisecond);

        // поля
        private string name;
        private ShipType type; // enum
        private int maxHull;
        private int hull;
        private int healRate;
        private ShipState state; // enum
        private List<Weapon> weapons;

        // властивості
        public string Name
        {
            get { return this.name; }
            set
            {
                if (Validator.IsNameValid(value))
                {
                    this.name = value;
                }
                else
                {
                    throw new ArgumentException("Некоректне значення!");
                }
            }
        }

        public ShipType Type
        {
            get { return this.type; }
            set
            {
                if (Validator.IsEnumValid(typeof(ShipType), (int)value))
                {
                    this.type = value;
                    WeaponHardpointCount = GetTurretCount(value);
                }
                else
                {
                    throw new ArgumentException("Некоректне значення!");
                }
            }

        }

        public int MaxHull
        {
            get { return this.maxHull; }
            set
            {
                if (Validator.IsIntValid(value, minHealth, maxHealth, null))
                {
                    this.maxHull = value;
                }
                else
                {
                    throw new ArgumentException("Некоректне значення!");
                }
            }

        }

        public int Hull
        {
            get { return this.hull; }
            set
            {
                if (value > maxHull)
                {
                    this.hull = maxHull;
                }
                else if (value <= 0)
                {
                    this.hull = 0;
                    State = ShipState.Destroyed;
                }
                else
                {
                    this.hull = value;
                }
            }
        }

        public int HealRate
        {
            get { return this.healRate; }
            set
            {
                if (Validator.IsIntValid(value, 0, maxHealRate, null))
                {
                    this.healRate = value;
                }
                else
                {
                    throw new ArgumentException("Некоректне значення!");
                }
            }
        }

        // кількість слотів зброї на корабель
        [JsonIgnore]
        public int WeaponHardpointCount { get; private set; } = 0;

        // властивість з гетером та сетером різних рівнів доступу
        public List<Weapon> Weapons
        {
            get
            {
                return this.weapons;
            }

            private set
            {
                this.weapons = value;
            }
        }

        [JsonInclude]
        public ShipState State
        {
            get { return this.state; }
            set
            {
                if (Validator.IsEnumValid(typeof(ShipState), (int)value))
                {
                    this.state = value;
                    switch (value)
                    {
                        case ShipState.Destroyed:
                            {
                                Logger.Info($"Ship {Name} of type {Type} was destroyed.");
                            }
                            break;
                        case ShipState.Stunned:
                            {
                                Logger.Stun($"Ship {Name} of type {Type} is stunned.");
                            }
                            break;
                    }
                }
                else
                {
                    throw new ArgumentException("Некоректне значення!");
                }
            }
        }

        // Обчислювана властивість
        [JsonIgnore]
        public int DamagePerSalvo
        {
            get
            {
                int DPS = 0;
                foreach (Weapon weapon in Weapons)
                {
                    DPS = DPS + weapon.Damage;
                }
                return DPS;
            }
        }

        // Статична автовластивість для зберігання кількості створених кораблів
        public static int ShipsSpawned { get; private set; } = 0;

        // конструктори
        public SpaceShip()
        {
            this.State = ShipState.Alive;
            Weapons = new List<Weapon>();
            ShipsSpawned++;
            Logger.General("Спрацював конструктор 1");
        }

        public SpaceShip(string name, ShipType type, int maxHull, int healRate) : this()
        {
            this.Name = name;
            this.Type = type;
            this.MaxHull = maxHull;
            this.Hull = maxHull;
            this.HealRate = healRate;
            Logger.General("Спрацював конструктор 2");
        }

        public SpaceShip(string name, ShipType type, int hull, int healRate, List<Weapon> weapons) : this(name, type, hull, healRate)
        {
            this.Weapons = weapons;
            Logger.General("Спрацював конструктор 3");
        }

        // метод додавання зброї
        public bool WeaponAdd(Weapon weapon)
        {
            if (Weapons.Count < WeaponHardpointCount)
            {
                Weapons.Add(weapon);
                return true;
            }
            else
            {
                return false;
            }
        }

        // метод отримання пошкоджень
        public void TakeDamage(int damage)
        {
            Random rand = new Random();
            Hull = Hull - damage;
            Logger.Damage($"Ship {Name} of type {Type} took {damage} points of damage.");
            if (rand.Next(1, 101) <= stunChance)
            {
                InflictStun(this);
            }
        }

        // метод атаки іншого корабля
        public void Attack(SpaceShip enemy)
        {
            if (!((State == ShipState.Destroyed) || (State == ShipState.Stunned)))
            {
                foreach (Weapon weapon in Weapons)
                {
                    if (enemy.State != ShipState.Destroyed)
                    {
                        Logger.Action($"Ship {Name} of type {Type} attacked enemy ship {enemy.Name} of Type {Type}, using {weapon.Name}.");
                        enemy.TakeDamage(weapon.Damage);
                    }
                }
            }
        }

        // метод ремонту на значення
        public void Heal(int healAmount)
        {
            if (!((State == ShipState.Destroyed) || (State == ShipState.Stunned)))
            {
                Hull = Hull + healAmount;
                Logger.Repair($"Ship {Name} of type {Type} repaired by {healAmount} points.");
            }
        }

        // перевантажений метод повного ремонту і зняття всіх негативних ефектів
        public void Heal()
        {
            State = ShipState.Alive;
            Heal(MaxHull - Hull);
            Logger.Info($"Ship {Name} of type {Type} is now fully repaired and operational.");
        }

        // цей метод розроблявся для того, щоб один корабель міг глушити інший, хоча він може і "сам себе".
        public void InflictStun(SpaceShip enemy)
        {
            if (State != ShipState.Stunned && State != ShipState.Destroyed)
            {
                State = ShipState.Stunned;
            }
        }

        // метод повернення у стрій виведеного з ладу корабля
        public void DeStun()
        {
            if (State == ShipState.Stunned)
            {
                State = ShipState.Alive;
                Logger.Info($"Ship {Name} of type {Type} is not stunned anymore.");
            }
        }

        // функція виведення у рядок інформації про корабель
        public string Info()
        {
            StringBuilder result = new StringBuilder(
                $"Назва: {Name}"+
                $"\nТип: {Type}" +
                $"\nМіцність корпусу: {Hull}/{MaxHull}" +
                $"\nРемонт корпусу: {HealRate}" +
                $"\nСтатус: {State}" +
                "\nЗБРОЯ: ");
            int i = 0;
            foreach (Weapon weapon in Weapons)
            {
                i++;
                result.Append($"\nСЛОТ {i}: {weapon.Name}, {weapon.Damage} dmg");
            }
            result.Append($"\nВогневий потенціал: {DamagePerSalvo} dmg per salvo");
            result.Append($"\nРядкове представлення: \n{this}");
            return result.ToString();
        }

        // метод виведення в консоль інформації про корабель
        public void Print()
        {
            Logger.Info(this.Info());
        }

        //статичний метод, який повертає колекцію з N випадково згенерованих кораблів, приймає колекцію імен, зброї та число N.
        public static List<SpaceShip> GenerateRandomShips(List<string> names, List<Weapon> presetWeapons, int N)
        {
            List<SpaceShip> result = new List<SpaceShip>();
            if ((N > 0) && (names != null) && (presetWeapons != null) && (names.Count > 0) && (presetWeapons.Count > 0) && (names.Count >= N))
            {
                SpaceShip newShip;
                

                int HullPerType = (maxHealth - minHealth) / (10 * Enum.GetValues(typeof(ShipType)).Length);
                int chosenNameIndex;
                int maxHP;

                for (int i = 0; i < N; i++)
                {
                    newShip = new SpaceShip();
                    chosenNameIndex = random.Next(0, names.Count);
                    newShip.Name = names[chosenNameIndex];
                    names.RemoveAt(chosenNameIndex);

                    newShip.Type = (ShipType)random.Next(0, ((ShipType[])Enum.GetValues(typeof(ShipType))).Length);
                    maxHP = 10 * random.Next(minHealth / 10 + (int)newShip.Type * HullPerType, (minHealth / 10 + ((int)newShip.Type + 1) * HullPerType) + 1);
                    if (maxHP > maxHealth)
                    { maxHP = maxHealth; }
                    newShip.MaxHull = maxHP;
                    newShip.Hull = maxHP;
                    newShip.healRate = random.Next(0, maxHealRate + 1);
                    while (newShip.WeaponAdd(presetWeapons[random.Next(0, presetWeapons.Count)]));
                    result.Add(newShip);
                }
            }
            else
            {
                throw new ArgumentException("Некоректне значення!");
            }
            return result;
        }

        //статична функція, що повертає максимальну кількість зброї для даного типу корабля
        public static int GetTurretCount(ShipType type)
        {
            return (int)type + 1;
        }

        // Перевантажений метод перетворення у рядок, який сприймається методами Parse() і TryParse()
        public override string ToString()
        {
            StringBuilder result = new StringBuilder($"{Name},{(int)Type},{MaxHull},{Hull},{HealRate},{(int)state},");
            if (Weapons.Count > 0)
            {
                foreach (Weapon weapon in Weapons)
                {
                    result.Append($"{weapon.Name}_{weapon.Damage}/");
                }
                result.Remove(result.Length - 1, 1);
            }
            
            return result.ToString();
        }

        // статичний метод, який приймає рядок, що має представляти корабель та повертає об'єкт корабля, який був створений на основі даних в цьому рядку. В процесі може викинути виключення.
        public static SpaceShip Parse(string input)
        {
            SpaceShip newShip;
            string[] shipComponents = input.Split(',');
            string S_name;
            int S_type;
            int S_maxHull;
            int S_hull;
            int S_healRate;
            int S_state;

            if (shipComponents.Length != 7)
            {
                throw new FormatException("Вхідний рядок мав невірний формат!");
            }
            else if (!(int.TryParse(shipComponents[1], out S_type) &&
                       int.TryParse(shipComponents[2], out S_maxHull) &&
                       int.TryParse(shipComponents[3], out S_hull) &&
                       int.TryParse(shipComponents[4], out S_healRate) &&
                       int.TryParse(shipComponents[5], out S_state)))
            {
                throw new ArgumentException("Аргументи для створення корабля мали невірний формат!");
            }
            else
            {
                S_name = shipComponents[0];
                if (!(Validator.IsNameValid(S_name) &&
                      Validator.IsEnumValid(typeof(ShipType), S_type) &&
                      Validator.IsIntValid(S_maxHull, minHealth, maxHealth, null) &&
                      Validator.IsIntValid(S_hull, 0, S_maxHull, null) && 
                      Validator.IsIntValid(S_healRate, 0, maxHealRate, null) && 
                      Validator.IsEnumValid(typeof(ShipState), S_state)))
                {
                    throw new ArgumentOutOfRangeException($"name: {S_name}; type: {S_type}; maxHull: {S_maxHull}; hull: {S_hull}; healRate: {S_healRate}; state: {S_state}.", "Некоректні значення аргументів створення корабля");
                }
                else
                {
                    newShip = new SpaceShip(S_name, (ShipType)S_type, S_maxHull, S_healRate);
                    newShip.Hull = S_hull;
                    // доступ напряму до поля, аби задати вже відвалідоване значення напряму і не затригерити небажаний в даному випадку функціонал.
                    newShip.state = newShip.Hull > 0 ? (ShipState)S_state : ShipState.Destroyed;

                    string[] S_weapons = shipComponents[6].Split('/');
                    string[] weaponComponents;
                    Weapon weapon;

                    if (S_weapons.Length > 0 && S_weapons[0] != "")
                    {
                        if (S_weapons.Length > GetTurretCount(newShip.Type))
                        {
                            throw new ArgumentOutOfRangeException($"MaxTurretCount for that type: {GetTurretCount(newShip.Type)}", "Кількість рядкових представлень об'єктів зброї перевищує максимальну");
                        }
                        else
                        {
                            foreach (string line in S_weapons)
                            {
                                if (Weapon.TryParse(line, out weapon))
                                {
                                    newShip.WeaponAdd(weapon);
                                }
                                else
                                {
                                    throw new ArgumentException("Рядкове представлення зброї мало невірний формат або значення", $"input {line}");
                                }
                            }
                        }
                    }
                }
            }
            return newShip;
        }

        // статичний метод, який здійснює спробу перетворити вхідний рядок на об'єкт корабля, повертає успішність операції.
        public static bool TryParse(string input, out SpaceShip ship)
        {
            bool result = true;
            ship = null;
            try
            {
                ship = SpaceShip.Parse(input);
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
