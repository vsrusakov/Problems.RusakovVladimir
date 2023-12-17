using Xunit;

namespace Calculator
{
    public class CalculatorTests
    {
        public static void OneElementTest()
        {
            string expr1 = "1";
            string expr2 = "(((2)))";
            string expr3 = "((1 + 3 / 2))";

            double answer1 = Calculator.CalculateFromString(expr1);
            double answer2 = Calculator.CalculateFromString(expr2);
            double answer3 = Calculator.CalculateFromString(expr3);

            Assert.Equal(1, answer1);
            Assert.Equal(2, answer2);
            Assert.Equal(2.5, answer3);
        }
        public static void LotOfSpaceTest()
        {
            string expr1 = " (  1 +  2)    * 4  + 3";
            string expr2 = "2 +   2*   2";

            double answer1 = Calculator.CalculateFromString(expr1);
            double answer2 = Calculator.CalculateFromString(expr2);

            Assert.Equal(15, answer1);
            Assert.Equal(6, answer2);
        }
        public static void MissParenthesisTest()
        {
            string expr1 = "(1 + 2)) * 4 + 3";
            string expr2 = "((1 + 3) / 2))";

            Assert.Throws<Exception>(() => Calculator.CalculateFromString(expr1));
            Assert.Throws<Exception>(() => Calculator.CalculateFromString(expr2));
        }
        public static void CalculationTest()
        {
            string expr1 = "11 + 2 * 3 - (4 - 5) / 2";
            string expr2 = "1 * 1 + 2 * 2 + 3 * 3";
            string expr3 = "((((1 + 1) + 1) + 1) + 1)";

            double answer1 = Calculator.CalculateFromString(expr1);
            double answer2 = Calculator.CalculateFromString(expr2);
            double answer3 = Calculator.CalculateFromString(expr3);

            Assert.Equal(17.5, answer1);
            Assert.Equal(14, answer2);
            Assert.Equal(5, answer3);
        }
    }
}
