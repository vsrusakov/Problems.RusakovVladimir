namespace Game15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Я записал несколько примеров начальных позиций. 
            // В комментариях к каждой позиции записаны кол-во перемещений,
            // приводящих в целевую позицию, и кол-во просмотренных узлов
            // во время работы A* (они хранятся в closedset).
            // Для позиций, требующих >= 40 ходов, моя реализация А*
            // работает довольно долго.

            int[,] start1 = { { 1, 5, 2 }, { 7, 4, 6 }, { 9, 3, 8 } }; // 10,  64
            int[,] start2 = { { 2, 3, 6 }, { 5, 1, 4 }, { 8, 7, 9 } }; // 20, 1871

            int[,] start3 = { { 1, 6, 2, 4 }, { 16, 5, 3, 12 }, { 9, 10, 8, 7 }, { 13, 14, 11, 15 } }; // 11,  47
            int[,] start4 = { { 1, 2, 8, 3 }, { 6, 7, 10, 4 }, { 5, 14, 15, 11 }, { 9, 13, 12, 16 } }; // 16,  119
            int[,] start5 = { { 5, 3, 4, 8 }, { 2, 7, 10, 12 }, { 1, 9, 16, 6 }, { 13, 14, 11, 15 } };  // 20,  1311
            int[,] start6 = { { 10, 1, 3, 16 }, { 7, 5, 2, 4 }, { 6, 12, 11, 8 }, { 9, 13, 14, 15 } }; // 25, 5181

            int[,] start7 = { 
                { 1, 2, 8, 3, 5 },
                { 6, 17, 12, 7, 10 }, 
                { 11, 25, 9, 4, 14 }, 
                { 16, 22, 13, 19, 15 }, 
                { 21, 23, 18, 24, 20 } 
            }; // 19,  1112

            Solve(start7);
        }
        public static void Solve(int[,] start_pos)
        {
            // Метод для решения головоломки и вывода на консоль
            // последовательности ходов, приводящих из позиции start в позицию goal. 
            // Вход start_pos соответствует расположению костяшек в начале игры. 

            NodeMat start = new NodeMat(start_pos);
            NodeMat goal = NodeMat.GoalNode(start.N_size);
            var steps = Solver.Astar(start, goal);

            Console.WriteLine($"Потребуется перемещений: {steps.Count - 1}");

            PrintSteps(steps, 3);
        }
        public static void PrintSteps(Stack<NodeMat> steps, int n = 3)
        {
            // Метод для вывода на консоль последовательности ходов.

            while (steps.TryPop(out var mat))
            {
                int j = 0;
                foreach (int v in mat.Value)
                {
                    if (j % mat.N_size == 0) Console.Write("\n");

                    Console.Write(v.ToString().PadLeft(n));
                    j++;
                }
                Console.Write("\n");
            }
        }
    }
}
