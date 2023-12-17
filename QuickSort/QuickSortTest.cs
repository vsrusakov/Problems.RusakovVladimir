using Xunit;

namespace QuickSort
{
    public class QuickSortTest
    {
        public static void PartitionTest()
        {
            QuickSort.MyComparer<int> int_cmp = new();

            int[] array1 = { 9, 4, 10, -6, 14, 0, -11, -12, -7, 4, 7, -9, -12, 10, 11, -7, -10, 13, -13, -4 };
            int[] array1_copy = (int[])array1.Clone();

            int[] array2 = { 1, 0, 8, 1, 1, 3, 7, 7, 1, 6, 1, 9, 4, 7, 6 };
            int[] array2_copy = (int[])array2.Clone();

            // найдем опорный элемент для array1
            int median1 = (array1.Length - 1) / 2;
            int[] temp_array1 = { array1[0], array1[median1], array1[array1.Length - 1] };
            QuickSort.InsertionSort<int>(temp_array1);
            int pivot1 = temp_array1[1];

            int p1 = QuickSort.Partition<int>(array1, 0, array1.Length - 1);
            int p2 = QuickSort.Partition<int>(array1_copy, 0, array1.Length - 1, int_cmp);

            Assert.Equal(p1, p2);
            Assert.True(array1.SequenceEqual(array1_copy));

            for (int i = 0; i < array1.Length; i++)
            {
                if (i <= p1)
                    Assert.True(array1[i] <= pivot1);
                else
                    Assert.True(array1[i] >= pivot1);
            }

            // найдем опорный элемент для array2
            int median2 = (array2.Length - 1) / 2;
            int[] temp_array2 = { array2[0], array2[median2], array2[array2.Length - 1] };
            QuickSort.InsertionSort<int>(temp_array2);
            int pivot2 = temp_array2[1];

            int q1 = QuickSort.Partition<int>(array2, 0, array2.Length - 1);
            int q2 = QuickSort.Partition<int>(array2_copy, 0, array2.Length - 1, int_cmp);

            Assert.Equal(q1, q2);
            Assert.True(array2.SequenceEqual(array2_copy));

            for (int i = 0; i < array2.Length; i++)
            {
                if (i <= q1)
                    Assert.True(array2[i] <= pivot2);
                else
                    Assert.True(array2[i] >= pivot2);
            }
        }
        public static void SortTest()
        {
            QuickSort.MyComparer<int> int_cmp1 = new();
            QuickSort.MyComparer<int> int_cmp2 = new();
            QuickSort.MyComparer<int> int_cmp1_default = new();
            QuickSort.MyComparer<int> int_cmp2_default = new();

            int[] array1 = { 9, 4, 10, -6, 14, 0, -11, -12, -7, 4, 7, -9, -12, 10, 11, -7, -10, 13, -13, -4 };
            int[] array1_copy1 = (int[])array1.Clone();
            int[] array1_copy2 = (int[])array1.Clone();

            int[] array2 = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] array2_copy1 = (int[])array2.Clone();
            int[] array2_copy2 = (int[])array2.Clone();

            QuickSort.Sort<int>(array1_copy1);
            QuickSort.Sort<int>(array1_copy2, int_cmp1);
            Array.Sort(array1, int_cmp1_default);

            QuickSort.Sort<int>(array2_copy1);
            QuickSort.Sort<int>(array2_copy2, int_cmp2);
            Array.Sort(array2, int_cmp2_default);

            Assert.True(array1.SequenceEqual(array1_copy1));
            Assert.True(array1.SequenceEqual(array1_copy2));

            Assert.True(array2.SequenceEqual(array2_copy1));
            Assert.True(array2.SequenceEqual(array2_copy2));

            Assert.True(int_cmp1.Count > 0);
            Assert.True(int_cmp2.Count > 0);

            Console.WriteLine($"Кол-во сравнений для array1: {int_cmp1.Count}"); // 97
            Console.WriteLine($"Кол-во сравнений для array2: {int_cmp2.Count}"); // 19

            Console.WriteLine($"Кол-во сравнений для array1 при помощи Array.Sort(): {int_cmp1_default.Count}"); // 88
            Console.WriteLine($"Кол-во сравнений для array2 при помощи Array.Sort(): {int_cmp2_default.Count}"); // 8
        }
        public static void InsertionSortTest()
        {
            QuickSort.MyComparer<int> int_cmp1 = new();
            QuickSort.MyComparer<int> int_cmp2 = new();

            int[] array1 = { 9, 4, 10, -6, 14, 0, -11, -12, -7, 4, 7, -9, -12, 10, 11, -7, -10, 13, -13, -4 };
            int[] array1_copy1 = (int[])array1.Clone();
            int[] array1_copy2 = (int[])array1.Clone();

            int[] array2 = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            int[] array2_copy1 = (int[])array2.Clone();
            int[] array2_copy2 = (int[])array2.Clone();

            QuickSort.InsertionSort<int>(array1_copy1);
            QuickSort.InsertionSort<int>(array1_copy2, int_cmp1);
            Array.Sort(array1);

            QuickSort.InsertionSort<int>(array2_copy1);
            QuickSort.InsertionSort<int>(array2_copy2, int_cmp2);
            Array.Sort(array2);

            Assert.True(array1.SequenceEqual(array1_copy1));
            Assert.True(array1.SequenceEqual(array1_copy2));

            Assert.True(array2.SequenceEqual(array2_copy1));
            Assert.True(array2.SequenceEqual(array2_copy2));

            Assert.True(int_cmp1.Count > 0);
            Assert.True(int_cmp2.Count > 0);
        }
    }
}
