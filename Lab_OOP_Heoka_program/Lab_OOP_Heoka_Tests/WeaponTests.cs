namespace Lab_OOP_Heoka_Tests
{
    [TestClass]
    public class WeaponTests
    {
        [TestMethod]
        public void ToString_true()
        {
            // arrange
            Weapon testWeapon = new Weapon("Thunder", Weapon.maxDamage);
            string result;

            // act
            result = testWeapon.ToString();

            // assert
            Assert.AreEqual($"Thunder_{Weapon.maxDamage}", result);
        }

        [TestMethod]
        public void TryParse_true()
        {
            // arrange
            string name = "Gamma ray";
            int damage = Weapon.maxDamage;
            Weapon testWeapon;
            bool result;

            // act
            result = Weapon.TryParse($"{name}_{damage}", out testWeapon);

            // assert
            Assert.IsTrue(result && testWeapon.Name == name && testWeapon.Damage == damage);
        }

        [TestMethod]
        public void TryParse_false()
        {
            // arrange
            string name = "123";
            int damage = Weapon.maxDamage + 1;
            Weapon testWeapon;
            bool result;

            // act
            result = Weapon.TryParse($"{name}_{damage}", out testWeapon);

            // assert
            Assert.IsFalse(result && testWeapon.Name == null);
        }
    }
}