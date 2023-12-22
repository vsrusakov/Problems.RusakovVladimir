using System.Numerics;

namespace CSR
{
    public class CSR<T> : IEquatable<CSR<T>>
        where T : INumber<T>
    {
        // Класс реализует представление CSR разреженной матрицы.
        // Свойство Values хранит массив значений, свойство Indices 
        // хранит массив индексов столбцов, свойство Indptr хранит
        // массив индексации строк, свойство Shape хранит размеры матрицы.

        public T[] Values { get; set; }
        public int[] Indices { get; set; }
        public int[] Indptr { get; set; }
        public (int, int) Shape { get; set; }
        public CSR(T[] values, int[] indices, int[] indptr, (int, int) shape)
        {
            Values = values;
            Indices = indices;
            Indptr = indptr;
            Shape = shape;
        }
        public static CSR<T> ToCSR(T[,] dense_array)
        {
            // Метод строит CSR представление двумерного массива.

            if (dense_array.Rank != 2) throw new ArgumentException("Матрица должна быть двумерной");

            (int, int) shape = (dense_array.GetLength(0), dense_array.GetLength(1));
            var values = new List<T>();
            var indices = new List<int>();
            int[] indptr = new int[shape.Item1 + 1];

            for (int i = 0; i < shape.Item1; i++)
            {
                for (int j = 0; j < shape.Item2; j++)
                {
                    if (!T.IsZero(dense_array[i, j]))
                    {
                        values.Add(dense_array[i, j]);
                        indices.Add(j);
                    }
                }
                indptr[i + 1] = indices.Count;
            }

            return new CSR<T>(values.ToArray(), indices.ToArray(), indptr, shape);
        }
        public static T[,] ToDense(CSR<T> csr_array)
        {
            // Метод строит двумерный массив по его CSR представлению. 

            T[,] dense_array = new T[csr_array.Shape.Item1, csr_array.Shape.Item2];
            for (int i = 0; i < csr_array.Shape.Item1; i++)
            {
                int num_values = csr_array.Indptr[i + 1] - csr_array.Indptr[i];
                for (int j = 0; j < num_values; j++)
                {
                    T value = csr_array.Values[csr_array.Indptr[i] + j];
                    int k = csr_array.Indices[csr_array.Indptr[i] + j];
                    dense_array[i, k] = value;
                }
            }
            return dense_array;
        }
        public CSR<T> Transpose()
        {
            // Транспонирование матрицы в CSR представлении. 

            (int, int) shape = (this.Shape.Item2, this.Shape.Item1);
            int[] indptr = new int[shape.Item1 + 1];
            List<int> new_indices = new();

            for (int i = 0; i < this.Shape.Item1; i++)
            {
                int num_rows = this.Indptr[i + 1] - this.Indptr[i];
                for (int j = 0; j < num_rows; j++)
                {
                    new_indices.Add(i);
                    indptr[this.Indices[this.Indptr[i] + j] + 1]++;
                }
            }
            var unsorted = this.Indices.Select((x, i) => new KeyValuePair<int, (int, T, int)>(x, (i, this.Values[i], new_indices[i])));
            var sorted = from pair in unsorted
                         orderby pair.Key, pair.Value.Item1
                         select pair;

            T[] values = sorted.Select(x => x.Value.Item2).ToArray();
            int[] indices = sorted.Select(x => x.Value.Item3).ToArray();
            for (int i = 1; i < indptr.Length; i++)
            {
                indptr[i] += indptr[i - 1];
            }

            return new CSR<T>(values, indices, indptr, shape);
        }
        public CSR<T> Dot(CSR<T> other)
        {
            // Умножение двух матриц в CSR представлении.

            if (this.Shape.Item2 != other.Shape.Item1)
                throw new Exception("Недопустимые размеры матриц");

            (int, int) shape = (this.Shape.Item1, other.Shape.Item2);
            List<T> values = new();
            List<int> indices = new();
            int[] indptr = new int[shape.Item1 + 1];

            CSR<T> otherT = other.Transpose();

            for (int i = 0; i < shape.Item1; i++)
            {
                for (int j = 0; j < shape.Item2; j++)
                {
                    T result = T.Zero;
                    int p = this.Indptr[i], q = otherT.Indptr[j];

                    while (p < this.Indptr[i + 1] && q < otherT.Indptr[j + 1])
                    {
                        if (this.Indices[p] > otherT.Indices[q])
                            q++;
                        else if (this.Indices[p] == otherT.Indices[q])
                        {
                            result += this.Values[p] * otherT.Values[q];
                            p++; q++;
                        }
                        else
                            p++;
                    }

                    if (!T.IsZero(result))
                    {
                        values.Add(result);
                        indices.Add(j);
                    }
                }
                indptr[i + 1] = values.Count;
            }

            return new CSR<T>(values.ToArray(), indices.ToArray(), indptr, shape);
        }
        public bool Equals(CSR<T>? other)
        {
            if (other is null) { return false; }

            bool condition = this.Values.SequenceEqual(other.Values) &&
                this.Indices.SequenceEqual(other.Indices) &&
                this.Indptr.SequenceEqual(other.Indptr) &&
                this.Shape.Equals(other.Shape);
            return condition;
        }
        public override bool Equals(object? obj) => this.Equals(obj as CSR<T>);
        public override int GetHashCode() => 0;
    }
}