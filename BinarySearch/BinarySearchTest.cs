using Xunit;

namespace BinarySearch
{
    public class BinarySearchIntTest
    {
        public class IntComparer: IComparer<int>
        {
            public int Compare(int x, int y) {  return x.CompareTo(y); }
        }
        public static void EmptyArrayTest()
        {
            IntComparer intComparer = new();

            int[] emptyArray = new int[0];

            Assert.Throws<Exception>(() => BinarySearch.Find<int>(emptyArray, intComparer, 1));
        }
        public static void OneElementTest()
        {
            IntComparer intComparer = new();

            int[] oneElement = { 10 };

            Assert.Equal(0, BinarySearch.Find<int>(oneElement, intComparer, 10));
            Assert.Equal(-1, BinarySearch.Find<int>(oneElement, intComparer, 9));
        }
        public static void NotFoundTest()
        {
            //тест на поиск отсутствующих значений
            IntComparer intComparer = new();

            int[] intArray = { 1, 4, 7, 12, 15 };

            Assert.Equal(-1, BinarySearch.Find<int>(intArray, intComparer, -2));
            Assert.Equal(-1, BinarySearch.Find<int>(intArray, intComparer, 8));
            Assert.Equal(-1, BinarySearch.Find<int>(intArray, intComparer, 16));
        }
        public static void FindFirstLastTest()
        {
            IntComparer intComparer = new();

            int[] intArray = { -3, 2, 5, 9, 11, 13 };

            Assert.Equal(0, BinarySearch.Find<int>(intArray, intComparer, -3));
            Assert.Equal(intArray.Length - 1, BinarySearch.Find<int>(intArray, intComparer, 13));
        }
    }
    public class BinarySearchStringTest
    {
        public static void EmptyArrayTest()
        {
            StringComparer ordComparer = StringComparer.Ordinal;

            string[] emptyArray = new string[0];

            Assert.Throws<Exception>(() => BinarySearch.Find<string>(emptyArray, ordComparer, "str"));
        }
        public static void OneElementTest()
        {
            StringComparer ordComparer = StringComparer.Ordinal;

            string[] oneElement = { "String" };

            Assert.Equal(0, BinarySearch.Find<string>(oneElement, ordComparer, "String"));
            Assert.Equal(-1, BinarySearch.Find<string>(oneElement, ordComparer, "string"));
        }
        public static void NotFoundTest()
        {
            //тест на поиск отсутствующих значений
            StringComparer ordComparer = StringComparer.Ordinal;

            string[] strArray = { "Cd", "aa", "ba", "cd" };

            Assert.Equal(-1, BinarySearch.Find<string>(strArray, ordComparer, "CD")); // "CD" < "Cd"
            Assert.Equal(-1, BinarySearch.Find<string>(strArray, ordComparer, "ab")); // "aa" < "ab" < "ba"
            Assert.Equal(-1, BinarySearch.Find<string>(strArray, ordComparer, "eg")); // "cd" < "eg"
        }
        public static void FindFirstLastTest()
        {
            StringComparer ordComparer = StringComparer.Ordinal;

            string[] strArray = { "Aa", "ba", "cd", "ef" };

            Assert.Equal(0, BinarySearch.Find<string>(strArray, ordComparer, "Aa"));
            Assert.Equal(strArray.Length - 1, BinarySearch.Find<string>(strArray, ordComparer, "ef"));
        }
    }
}
