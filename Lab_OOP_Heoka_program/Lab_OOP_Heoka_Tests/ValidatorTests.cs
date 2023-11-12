namespace Lab_OOP_Heoka_Tests
{
    [TestClass]
    public class ValidatorTests
    {
        enum TestEnum
        {
            variant_1,
            variant_2,
            variant_3
        }

        readonly string[] TestNames =
        {
            // Правильний рядок
            "CorrectLine_1234",

            // Неправильні рядки
            "іаапа",
            "123sdfgdf",
            "hh"
        };
        readonly int maxBorder = 100;
        readonly int minBorder = -100;

        [TestMethod]
        public void IsEnumValid_true()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsEnumValid(typeof(TestEnum), 1);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsEnumValid_false()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsEnumValid(typeof(TestEnum), 188);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsNameValid_true()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsNameValid(TestNames[0]);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNameValid_false1()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsNameValid(TestNames[1]);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsNameValid_false2()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsNameValid(TestNames[2]);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsNameValid_false3()
        {
            // arrange
            bool result;

            // act
            result = Validator.IsNameValid(TestNames[3]);

            // assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void IsIntValid_true1()
        {
            // arrange
            bool result;
            int testVal = 90;

            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder,  true);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsIntValid_true2()
        {
            // arrange
            bool result;
            int testVal = 27;

            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder, false);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsIntValid_true3()
        {
            // arrange
            bool result;
            int testVal = -56;

            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder, null);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsIntValid_false1()
        {
            // arrange
            bool result;
            int testVal = -273;
            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder, true);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsIntValid_false2()
        {
            // arrange
            bool result;
            int testVal = 277;
            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder, false);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsIntValid_false3()
        {
            // arrange
            bool result;
            int testVal = 1245;
            // act
            result = Validator.IsIntValid(testVal, minBorder, maxBorder, null);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFileNameValid_true()
        {
            // arrange
            bool result;
            string testName = "sdkfj122_289d";
            // act
            result = Validator.IsFileNameValid(testName);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFileNameValid_false()
        {
            // arrange
            bool result;
            string testName = @"sd\kfj1<22_28>9d";
            // act
            result = Validator.IsFileNameValid(testName);

            // assert
            Assert.IsFalse(result);
        }
    }
}
