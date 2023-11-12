using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_OOP_Heoka_Tests
{
    [TestClass]
    public class ProgramTests
    {
        string txtName = @"\test.txt";
        string jsonName = @"\test.json";

        static int hp = SpaceShip.maxHealth;
        static int regen = SpaceShip.maxHealRate;

        List<SpaceShip> testShips = new List<SpaceShip>()
        {
            new SpaceShip("Test1", ShipType.Fighter, hp, regen, new List<Weapon>() { new Weapon("weapon", Weapon.minDamage) }),
            new SpaceShip("Test2", ShipType.Frigate, hp, regen, new List<Weapon>() { new Weapon("weapon", Weapon.minDamage), new Weapon("weapon", Weapon.minDamage), new Weapon("weapon", Weapon.minDamage) }),
            new SpaceShip("Test3", ShipType.Destroyer, hp, regen, new List<Weapon>() { new Weapon("weapon", Weapon.minDamage), new Weapon("weapon", Weapon.minDamage) })
        };


        [TestMethod]
        public void SaveJSON_true()
        {
            // assert
            Assert.IsTrue(Program.CollectionSaveJSON(testShips, jsonName));
        }

        [TestMethod]
        public void SaveJSON_false()
        {
            // assert
            Assert.IsFalse(Program.CollectionSaveJSON(testShips, jsonName + "<>"));
        }

        [TestMethod]
        public void ReadJSON_AllValid()
        {
            // assert
            Assert.AreEqual(testShips.Count, Program.CollectionReadJSON(jsonName).Count);
        }

        [TestMethod]
        public void ReadJSON_SomeInvalid()
        {
            // assert
            Assert.AreEqual(testShips.Count - 1, Program.CollectionReadJSON(@"\test_invalid.json").Count);
        }

        [TestMethod]
        public void SaveTXT_true()
        {
            // assert
            Assert.IsTrue(Program.CollectionSaveTXT(testShips, txtName));
        }

        [TestMethod]
        public void SaveTXT_false()
        {
            // assert
            Assert.IsFalse(Program.CollectionSaveTXT(testShips, txtName + "<>"));
        }

        [TestMethod]
        public void ReadTXT_AllValid()
        {
            // assert
            Assert.AreEqual(testShips.Count, Program.CollectionReadTXT(txtName).Count);
        }

        [TestMethod]
        public void ReadTXT_SomeInvalid()
        {
            // assert
            Assert.AreEqual(testShips.Count - 2, Program.CollectionReadTXT(@"\test_invalid.txt").Count);
        }

        [TestMethod]
        public void ShipSearch_index()
        {
            // assert
            Assert.AreEqual(testShips[2], Program.ShipSearch("2", testShips).FirstOrDefault());
        }

        [TestMethod]
        public void ShipSearch_name()
        {
            // assert
            Assert.AreEqual(testShips[2], Program.ShipSearch("Test3", testShips).FirstOrDefault());
        }

        [TestMethod]
        public void ShipSearch_false1()
        {
            // assert
            Assert.IsTrue(Program.ShipSearch("999", testShips).Count == 0);
        }

        [TestMethod]
        public void ShipSearch_false2()
        {
            // assert
            Assert.IsTrue(Program.ShipSearch("sdfjhsdfhisdfjdfhf", testShips).Count == 0);
        }

    }
}
