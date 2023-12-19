global using static System.Math;

namespace Game15
{
    public class NodeMat : IEquatable<NodeMat>
    {
        // Класс для хранения вершин/узла графа.
        // Каждый узел соответствует матрице размера N_size x N_size,
        // которая хранится в свойстве Value, при этом число N_size^2
        // соответствует пустой клетке.
        // Свойства Manhatten и Moves хранят расстояние до целевого узла
        // и кол-во проделанных ходов от начального узла соотв..
        // Свойство PreviousNode хранит ссылку на предыдущий узел для того,
        // чтобы позднее восстановить все пройденные узлы от целевого до начального.
        // Конструктор создает узел по переданной матрице, которая
        // сохраняется в Value.

        // веса в формуле вычисления приоритета узла
        private static double a = 2, b = 5;
        public double Manhatten { get; set; }
        public double Moves { get; set; }
        public double Priority { get {  return a * Manhatten + b * Moves; } }
        public int[,] Value { get; set; } = new int[,] { };
        public int N_size { get {  return Value.GetLength(0); } }
        public NodeMat? PreviousNode { get; set; } = null;
        public NodeMat(int[,] value, double manhatten = 0, double moves = 0)
        {
            Manhatten = manhatten;
            Moves = moves;
            if (value.Rank == 2 && (value.GetLength(0) == value.GetLength(1)))
            {
                Value = value;
            }
            else throw new Exception("Недопустимое значение массива");
        }
        // Равенство двух узлов определяется как раенство
        // соответствующих матриц Value. Это потребуется для 
        // поиска заданного узла в списке узлов в методе Solver.Astar().
        public bool Equals(NodeMat? other)
        {
            if (other is null) { return false; }

            return Value.Cast<int>().SequenceEqual(other.Value.Cast<int>());
        }
        public override bool Equals(object? obj) => this.Equals(obj as NodeMat);
        public override int GetHashCode() => 0;
        public static double ManhattenDist(NodeMat x, NodeMat y)
        {
            // метод для подсчета манхеттенского расстояния между двумя узлами/матрицами

            if (x.N_size == y.N_size)
            {
                double result = 0;
                for (int i = 0; i < x.N_size; i++)
                {
                    for (int j  = 0; j < x.N_size; j++)
                    {
                        result += Abs(x.Value[i, j] - y.Value[i, j]);
                    }
                }
                return result;
            }
            else throw new Exception("Матрицы имеют разные размеры");
        }
        public List<NodeMat> NeighborNodes()
        {
            // Метод возвращает список узлов, в которые можно попасть
            // с помощь одного перемещения костяшки в пустую клетку
            // (т.е. метод вернет 2, 3 или 4 узла).

            List<NodeMat> neighbors = new();
            int[] space_index = { 0, 0 };

            for (int k = 0; k < this.N_size; k++)
            {
                bool is_found = false;
                for (int l = 0; l < this.N_size; l++)
                {
                    if (this.Value[k, l] == Pow(this.N_size, 2))
                    {
                        is_found = true;
                        (space_index[0], space_index[1]) = (k, l);
                        break;
                    }
                }
                if (is_found) { break; }
            }
            int i, j;
            (i, j) = (space_index[0], space_index[1]);

            if (i == 0 || i == this.N_size - 1)
            {
                var neighbour = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);
                int shift = i == 0 ? 1 : -1;

                (neighbour.Value[i, j], neighbour.Value[i + shift, j]) =
                    (neighbour.Value[i + shift, j], neighbour.Value[i, j]);

                neighbors.Add(neighbour);
            }
            else
            {
                var neighbour1 = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);
                var neighbour2 = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);

                (neighbour1.Value[i, j], neighbour1.Value[i + 1, j]) =
                    (neighbour1.Value[i + 1, j], neighbour1.Value[i, j]);

                (neighbour2.Value[i, j], neighbour2.Value[i - 1, j]) =
                    (neighbour2.Value[i - 1, j], neighbour2.Value[i, j]);

                neighbors.Add(neighbour1);
                neighbors.Add(neighbour2);
            }

            if (j == 0 || j == this.N_size - 1)
            {
                var neighbour = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);
                int shift = j == 0 ? 1 : -1;

                (neighbour.Value[i, j], neighbour.Value[i, j + shift]) =
                    (neighbour.Value[i, j + shift], neighbour.Value[i, j]);

                neighbors.Add(neighbour);
            }
            else
            {
                var neighbour1 = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);
                var neighbour2 = new NodeMat((int[,])this.Value.Clone(), 0, this.Moves + 1);

                (neighbour1.Value[i, j], neighbour1.Value[i, j + 1]) =
                    (neighbour1.Value[i, j + 1], neighbour1.Value[i, j]);

                (neighbour2.Value[i, j], neighbour2.Value[i, j - 1]) =
                    (neighbour2.Value[i, j - 1], neighbour2.Value[i, j]);

                neighbors.Add(neighbour1);
                neighbors.Add(neighbour2);
            }

            return neighbors;
        }
        public static NodeMat GoalNode(int N_size)
        {
            // Метод возвращает целевой узел графа, соответствующий
            // матрице порядка N_size, заполненной последовательными
            // значениями 1, 2, 3, ... , N_size^2

            int[,] value = new int[N_size, N_size];

            for (int i = 0; i < N_size; i++)
            {
                for (int j = 0; j < N_size; j++)
                {
                    value[i, j] = N_size * i + j + 1;
                }
            }
            return new NodeMat(value);
        }
    }
}
