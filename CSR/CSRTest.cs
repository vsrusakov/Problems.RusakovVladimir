using Xunit;

namespace CSR
{
    public class CSRTest
    {
        public static int[,] mat1 = {
            { 1, 2, 0, 3 },
            { 0, 0, 4, 0 },
            { 0, 1, 0, 11 }
        };
        public static double[,] mat2 = {
            { 11.1, 22, 0, 0, 0 },
            { 0, 33, 44, 0, 0},
            { 0, 0, 55, 66.6, 77 },
            { 0, 0, 0, 0, 0}
        };
        public static int[] mat1Values = { 1, 2, 3, 4, 1, 11 };
        public static int[] mat1Indices = { 0, 1, 3, 2, 1, 3 };
        public static int[] mat1Indptr = { 0, 3, 4, 6 };

        public static double[] mat2Values = { 11.1, 22, 33, 44, 55, 66.6, 77 };
        public static int[] mat2Indices = { 0, 1, 1, 2, 2, 3, 4 };
        public static int[] mat2Indptr = { 0, 2, 4, 7, 7 };

        public static void ToCSRTest()
        {
            // Проверим, что метод ToCSR верно определяет свойства
            // Values, Indices и Indptr для матриц mat1 и mat2.

            CSR<int> csr_mat1 = CSR<int>.ToCSR(mat1);
            CSR<double> csr_mat2 = CSR<double>.ToCSR(mat2);

            Assert.True(csr_mat1.Values.SequenceEqual(mat1Values));
            Assert.True(csr_mat1.Indices.SequenceEqual(mat1Indices));
            Assert.True(csr_mat1.Indptr.SequenceEqual(mat1Indptr));

            Assert.True(csr_mat2.Values.SequenceEqual(mat2Values));
            Assert.True(csr_mat2.Indices.SequenceEqual(mat2Indices));
            Assert.True(csr_mat2.Indptr.SequenceEqual(mat2Indptr));
        }
        public static void ToDenseTest()
        {
            // Проверим, что метод ToDense правильно строит двумерный массив
            // по его CSR представлению для матриц mat1 и mat2.

            CSR<int> csr_mat1 = new(mat1Values, mat1Indices, mat1Indptr, (3, 4));
            CSR<double> csr_mat2 = new(mat2Values, mat2Indices, mat2Indptr, (4, 5));

            int[,] dense_mat1 = CSR<int>.ToDense(csr_mat1);
            double[,] dense_mat2 = CSR<double>.ToDense(csr_mat2);

            Assert.True(dense_mat1.Cast<int>().SequenceEqual(mat1.Cast<int>()));
            Assert.True(dense_mat2.Cast<double>().SequenceEqual(mat2.Cast<double>()));
        }
        public static void TransposeTest()
        {
            // Проверим, что метод Transpose правильно строит CSR представление 
            // при транспонировании и что соответствующий двумерный массив равен
            // транспонированному исходному массиву.

            int[,] mat1T = {
                { 1, 0, 0 },
                { 2, 0, 1 },
                { 0, 4, 0 },
                { 3, 0, 11 }
            };
            double[,] mat2T = {
                { 11.1, 0, 0, 0 },
                { 22, 33, 0, 0 },
                { 00, 44, 55, 0 },
                { 0, 0, 66.6, 0 },
                { 0, 0, 77, 0}
            };

            CSR<int> csr_mat1 = new(mat1Values, mat1Indices, mat1Indptr, (3, 4));
            CSR<double> csr_mat2 = new(mat2Values, mat2Indices, mat2Indptr, (4, 5));

            csr_mat1 = csr_mat1.Transpose();
            csr_mat2 = csr_mat2.Transpose();

            CSR<int> csr_mat1T = CSR<int>.ToCSR(mat1T);
            CSR<double> csr_mat2T = CSR<double>.ToCSR(mat2T);

            Assert.True(csr_mat1.Equals(csr_mat1T));
            Assert.True(CSR<int>.ToDense(csr_mat1).Cast<int>().SequenceEqual(mat1T.Cast<int>()));
            Assert.True(CSR<int>.ToDense(csr_mat1T).Cast<int>().SequenceEqual(mat1T.Cast<int>()));

            Assert.True(csr_mat2.Equals(csr_mat2T));
            Assert.True(CSR<double>.ToDense(csr_mat2).Cast<double>().SequenceEqual(mat2T.Cast<double>()));
            Assert.True(CSR<double>.ToDense(csr_mat2T).Cast<double>().SequenceEqual(mat2T.Cast<double>()));
        }
        public static void DotTest()
        {
            // Проверим, что нильпотеннтная матрица в квадрате равна нулевой матрице.

            int[,] matNilpotent = {
                { 5, -3, 2 },
                { 15, -9, 6 },
                { 10, -6, 4 }
            };
            var csr_matNilpotent = CSR<int>.ToCSR(matNilpotent);
            var csr_matNilpotentSquared = csr_matNilpotent.Dot(csr_matNilpotent);

            Assert.True(CSR<int>.ToDense(csr_matNilpotentSquared).Cast<int>().SequenceEqual(new int[9]));

            // Умножение матриц размера (1, 3) и (3, 4).

            int[,] vec = {
                { 1 },
                { 1 },
                { 1 },
            };

            CSR<int> csr_mat1 = CSR<int>.ToCSR(mat1);
            CSR<double> csr_mat2 = CSR<double>.ToCSR(mat2);
            CSR<int> csr_vec = CSR<int>.ToCSR(vec).Transpose();

            Assert.Throws<Exception>(() => csr_mat1.Dot(csr_vec));
            Assert.True(CSR<int>.ToDense(csr_vec.Dot(csr_mat1)).Cast<int>().SequenceEqual(new int[] { 1, 3, 4, 14 }));

            // Умножение двух матриц размера (3, 3)

            int[,] m1 = {
                { 5, 2, 1 },
                { 4, 3, 2 },
                { 2, 1, 5 }
            };
            int[,] m2 = {
                { 1, 4, 3 },
                { 2, 1, 5 },
                { 3, 2, 1 }
            };
            int[,] m1xm2 = {
                { 12, 24, 26 },
                { 16, 23, 29 },
                { 19, 19, 16 }
            };

            CSR<int> csr_m1 = CSR<int>.ToCSR(m1);
            CSR<int> csr_m2 = CSR<int>.ToCSR(m2);
            CSR<int> csr_m1xm2 = CSR<int>.ToCSR(m1xm2);

            Assert.True(csr_m1.Dot(csr_m2).Equals(csr_m1xm2));
        }
    }
}
