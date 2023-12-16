namespace BinarySearch
{
    public class BinarySearch
    {
        public static int Find<T>(T[] array, IComparer<T> comparer, T element)
        {
            if (array.Length == 0) throw new Exception("Передан пустой массив");

            int left = 0;
            int right = array.Length - 1;

            while (left <= right)
            {
                int middle = left + (right - left) / 2;
                var compare_result = comparer.Compare(array[middle], element);
                if (compare_result == 0)
                {
                    return middle;
                }
                else if (compare_result > 0)
                {
                    right = middle - 1;
                }
                else
                {
                     left = middle + 1;
                }
            }
            return -1;
        }
    }
}