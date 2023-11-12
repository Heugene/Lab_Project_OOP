namespace Lab_OOP_Heoka_Tests
{
    [TestClass]
    public class SpaceShipTests
    {
        [TestMethod]
        public void Attack_true()
        {
            // arrange
            int hp = SpaceShip.maxHealth;
            int regen = SpaceShip.maxHealRate;
            SpaceShip attacker = new SpaceShip("Atacker", ShipType.Fighter, hp, regen, new List<Weapon>() { new Weapon("weapon", Weapon.minDamage) });
            SpaceShip target = new SpaceShip("Target", ShipType.Fighter, hp, regen);
            bool Overkill = Weapon.minDamage > hp;


            // act
            attacker.Attack(target);

            // assert
            if (Overkill)
            {
                Assert.IsTrue(target.Hull == 0);
            }
            else
            {
                Assert.IsTrue(target.Hull == hp - attacker.DamagePerSalvo);
            }
        }

        [TestMethod]
        public void TakeDamage_true()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("test", ShipType.Fighter, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            int damage = Weapon.minDamage;
            bool Overkill = damage > testShip.MaxHull;


            // act
            testShip.TakeDamage(damage);

            // assert
            if (Overkill)
            {
                Assert.IsTrue(testShip.Hull == 0);
            }
            else
            {
                Assert.IsTrue(testShip.Hull == testShip.MaxHull - damage);
            }
        }

        [TestMethod]
        public void InflictStun_true()
        {
            // arrange
            SpaceShip test = new SpaceShip("Test", ShipType.Fighter, SpaceShip.maxHealth, SpaceShip.maxHealRate);

            // act
            test.InflictStun(test);

            // assert
            Assert.IsTrue(test.State == ShipState.Stunned);
        }

        [TestMethod]
        public void DeStun_true()
        {
            // arrange
            SpaceShip test = new SpaceShip("Test", ShipType.Fighter, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            test.State = ShipState.Stunned;

            // act
            test.DeStun();

            // assert
            Assert.IsTrue(test.State != ShipState.Stunned);
        }

        [TestMethod]
        public void GenerateRandomShips_true()
        {
            // arrange
            List<SpaceShip> ships;
            List<string> presetNames = new List<string>()
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
            List<Weapon> presetWeapons = new List<Weapon>()
            {
                new Weapon("Plasmer", Weapon.maxDamage),
                new Weapon("Multicannon", Weapon.maxDamage),
                new Weapon("Missile launcher", Weapon.maxDamage),
                new Weapon("Railgun", Weapon.maxDamage),
                new Weapon("Beam cannon", Weapon.maxDamage),
                new Weapon("Cannon", Weapon.maxDamage)
            };
            int N = 4;

            // act
            ships = SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N);

            // assert
            Assert.AreEqual(N, ships.Count);
        }

        [TestMethod]
        public void GenerateRandomShips_false1()
        {
            // arrange
            List<string> presetNames = new List<string>()
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
            List<Weapon> presetWeapons = new List<Weapon>()
            {
                new Weapon("Plasmer", Weapon.maxDamage),
                new Weapon("Multicannon", Weapon.maxDamage),
                new Weapon("Missile launcher", Weapon.maxDamage),
                new Weapon("Railgun", Weapon.maxDamage),
                new Weapon("Beam cannon", Weapon.maxDamage),
                new Weapon("Cannon", Weapon.maxDamage)
            };
            int N = -4;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void GenerateRandomShips_false2()
        {
            // arrange
            List<string>? presetNames = null;
            List<Weapon> presetWeapons = new List<Weapon>()
            {
                new Weapon("Plasmer", Weapon.maxDamage),
                new Weapon("Multicannon", Weapon.maxDamage),
                new Weapon("Missile launcher", Weapon.maxDamage),
                new Weapon("Railgun", Weapon.maxDamage),
                new Weapon("Beam cannon", Weapon.maxDamage),
                new Weapon("Cannon", Weapon.maxDamage)
            };
            int N = 4;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void GenerateRandomShips_false3()
        {
            // arrange
            List<string> presetNames = new List<string>()
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
            List<Weapon>? presetWeapons = null;
            int N = 4;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void GenerateRandomShips_false4()
        {
            // arrange
            List<string> presetNames = new List<string>();
            List<Weapon> presetWeapons = new List<Weapon>()
            {
                new Weapon("Plasmer", Weapon.maxDamage),
                new Weapon("Multicannon", Weapon.maxDamage),
                new Weapon("Missile launcher", Weapon.maxDamage),
                new Weapon("Railgun", Weapon.maxDamage),
                new Weapon("Beam cannon", Weapon.maxDamage),
                new Weapon("Cannon", Weapon.maxDamage)
            };
            int N = 4;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void GenerateRandomShips_false5()
        {
            // arrange
            List<string> presetNames = new List<string>()
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
            List<Weapon> presetWeapons = new List<Weapon>();
            int N = 4;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void GenerateRandomShips_false6()
        {
            // arrange
            List<string> presetNames = new List<string>()
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
            List<Weapon> presetWeapons = new List<Weapon>()
            {
                new Weapon("Plasmer", Weapon.maxDamage),
                new Weapon("Multicannon", Weapon.maxDamage),
                new Weapon("Missile launcher", Weapon.maxDamage),
                new Weapon("Railgun", Weapon.maxDamage),
                new Weapon("Beam cannon", Weapon.maxDamage),
                new Weapon("Cannon", Weapon.maxDamage)
            };
            int N = 12;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => SpaceShip.GenerateRandomShips(presetNames, presetWeapons, N));
        }

        [TestMethod]
        public void HealAmount_true()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("DamagedShip", ShipType.Fighter, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            int hull = (int)(0.5 * testShip.MaxHull);
            bool Overheal = testShip.MaxHull - hull < testShip.HealRate;

            // act
            testShip.Hull = hull;
            testShip.Heal(testShip.HealRate);

            // assert
            if (Overheal)
            {
                Assert.AreEqual(testShip.MaxHull, testShip.Hull);
            }
            else
            {
                Assert.AreEqual(hull + testShip.HealRate, testShip.Hull);
            }
        }

        [TestMethod]
        public void HealAll_true()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("DamagedShip", ShipType.Fighter, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            int hull = 0;

            // act
            testShip.Hull = hull;
            testShip.Heal();

            // assert
            Assert.IsTrue((testShip.State == ShipState.Alive) && (testShip.Hull == testShip.MaxHull));
        }

        [TestMethod]
        public void GetTurretCount_true()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("Test", ShipType.Frigate, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            int result;
            // act
            result = SpaceShip.GetTurretCount(testShip.Type);

            // assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void WeaponAdd_true()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("Test", ShipType.Frigate, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            Weapon testWeapon = new Weapon("GGG", Weapon.minDamage);
            bool result;

            // act
            testShip.WeaponAdd(testWeapon);
            testShip.WeaponAdd(testWeapon);
            result = testShip.WeaponAdd(testWeapon);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WeaponAdd_false()
        {
            // arrange
            SpaceShip testShip = new SpaceShip("Test", ShipType.Frigate, SpaceShip.maxHealth, SpaceShip.maxHealRate);
            Weapon testWeapon = new Weapon("GGG", Weapon.minDamage);
            bool result;

            // act
            testShip.WeaponAdd(testWeapon);
            testShip.WeaponAdd(testWeapon);
            testShip.WeaponAdd(testWeapon);
            result = testShip.WeaponAdd(testWeapon);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ToString_true()
        {
            // arrange
            string expected = "Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip = new SpaceShip("Catship", ShipType.Frigate, 670, 33);
            testShip.WeaponAdd(new Weapon("Multicannon", 10));
            testShip.WeaponAdd(new Weapon("Missile launcher", 60));
            testShip.WeaponAdd(new Weapon("Cannon", 30));
            string result;

            // act
            result = testShip.ToString();

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Parse_true()
        {
            // arrange
            string input = "Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip;

            // act
            testShip = SpaceShip.Parse(input);

            // assert
            Assert.AreEqual(input, testShip.ToString());
        }

        [TestMethod]
        public void Parse_false1()
        {
            // arrange
            string input = "Catship,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip;

            // act
            // assert
            Assert.ThrowsException<FormatException>(() => testShip = SpaceShip.Parse(input));
        }

        [TestMethod]
        public void Parse_false2()
        {
            // arrange
            string input = "Catship,2,6u70,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => testShip = SpaceShip.Parse(input));
        }

        [TestMethod]
        public void Parse_false3()
        {
            // arrange
            string input = "1Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip;

            // act
            // assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testShip = SpaceShip.Parse(input));
        }

        [TestMethod]
        public void Parse_false4()
        {
            // arrange
            string input = "Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30/Cannon_30";
            SpaceShip testShip;

            // act
            // assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testShip = SpaceShip.Parse(input));
        }

        [TestMethod]
        public void Parse_false5()
        {
            // arrange
            string input = "Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon+30";
            SpaceShip testShip;

            // act
            // assert
            Assert.ThrowsException<ArgumentException>(() => testShip = SpaceShip.Parse(input));
        }

        [TestMethod]
        public void TryParse_true()
        {
            // arrange
            string input = "Catship,2,670,670,33,0,Multicannon_10/Missile launcher_60/Cannon_30";
            SpaceShip testShip;
            bool result;

            // act
            result = SpaceShip.TryParse(input, out testShip);

            // assert
            Assert.IsTrue(result && string.Equals(input, testShip.ToString()));
        }

        [TestMethod]
        public void TryParse_false()
        {
            // arrange
            string input = "1Catship,67e0,670,33,0,Multicannon+10/Missile launcher_60/Cannon_30/Cannon_30";
            SpaceShip testShip;
            bool result;

            // act
            result = SpaceShip.TryParse(input, out testShip);

            // assert
            Assert.IsFalse(result && string.Equals(input, testShip.ToString()));
        }
    }
}
