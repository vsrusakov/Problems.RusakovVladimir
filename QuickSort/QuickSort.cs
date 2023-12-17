namespace QuickSort
{
    public class QuickSort
    {
        // быстрая сортировка с переключением на сортировку вставками и 
        // выбором опорного элемента с помощью медианы из трех 

        public const int small_array_size = 5;
        public static void Sort<T>(T[] array, int left = 0, int right = -1) where T : IComparable<T>
        {
            if (right == -1) right = array.Length - 1;

            if (right - left <= small_array_size - 1)
            {
                InsertionSort<T>(array, left, right);
                return;
            }
            int p = Partition<T>(array, left, right);
            Sort(array, left, p);
            Sort(array, p + 1, right);
        }
        public static int Partition<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            int median = left + (right - left) / 2;
            T[] temp_array = { array[left], array[median], array[right] };
            InsertionSort<T>(temp_array);
            (array[left], array[median], array[right]) = (temp_array[0], temp_array[1], temp_array[2]);

            int i = left, j = right;
            T pivot = array[median];
            
            while (i <= j)
            {
                while (array[i].CompareTo(pivot) < 0) i++;

                while (array[j].CompareTo(pivot) > 0) j--;

                if (i >= j) break;

                (array[i], array[j]) = (array[j], array[i]);
                i++;
                j--;
            }
            return j;
        }
        public static void InsertionSort<T>(T[] array, int left = 0, int right = -1) where T : IComparable<T>
        {
            if (right == -1) right = array.Length - 1;

            for (int i = left + 1; i < right + 1; i++)
            {
                T temp = array[i];
                int j = i - 1;
                while ((j >= left) && (temp.CompareTo(array[j]) < 0))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = temp;
            }
        }
        public static void Sort<T>(T[] array, IComparer<T> comparer, int left = 0, int right = -1)
        {
            if (right == -1) right = array.Length - 1;

            if (right - left <= small_array_size - 1)
            {
                InsertionSort<T>(array, comparer, left, right);
                return;
            }
            int p = Partition<T>(array, left, right, comparer);
            Sort(array, comparer, left, p);
            Sort(array, comparer, p + 1, right);
        }
        public static int Partition<T>(T[] array, int left, int right, IComparer<T> comparer)
        {
            int median = left + (right - left) / 2;
            T[] temp_array = { array[left], array[median], array[right] };
            InsertionSort<T>(temp_array, comparer);
            (array[left], array[median], array[right]) = (temp_array[0], temp_array[1], temp_array[2]);

            int i = left, j = right;
            T pivot = array[median];

            while (i <= j)
            {
                while (comparer.Compare(array[i], pivot) < 0) i++;

                while (comparer.Compare(array[j], pivot) > 0) j--;

                if (i >= j) break;

                (array[i], array[j]) = (array[j], array[i]);
                i++;
                j--;
            }
            return j;
        }
        public static void InsertionSort<T>(T[] array, IComparer<T> comparer, int left = 0, int right = -1)
        {
            if (right == -1) right = array.Length - 1;

            for (int i = left + 1; i < right + 1; i++)
            {
                T temp = array[i];
                int j = i - 1;
                while ((j >= left) && (comparer.Compare(temp, array[j]) < 0))
                {
                    array[j + 1] = array[j];
                    j--;
                }
                array[j + 1] = temp;
            }
        }
        public class MyComparer<T> : IComparer<T>
            where T : IComparable<T>
        {
            private int count = 0;
            public int Count { get { return count; } }
            public int Compare(T? x, T? y)
            {
                count++;
                return x.CompareTo(y);
            }
        }
    }
}