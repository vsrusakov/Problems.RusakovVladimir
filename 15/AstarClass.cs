namespace Game15
{
    public class Solver
    {
        public static Stack<NodeMat> Astar(NodeMat start, NodeMat goal)
        {
            // Метод реализует алгоритм A*. На вход подаются начальный и целевой узел.
            // Множество closedset содержит уже раскрытые узлы, очередь openqueue
            // содержит узлы, которые предстоит раскрыть (изначально содержит только start). 
            // Стек steps_to_goal после работы алгоритма содержит только узел goal, после чего 
            // этот стек заполняется узлами PreviousNode, начиная с узла goal.

            HashSet<NodeMat> closedset = new();
            Stack<NodeMat> steps_to_goal = new();
            PriorityQueue<NodeMat, double> openqueue = new();
            bool goal_attained = false;

            start.Manhatten = NodeMat.ManhattenDist(start, goal);
            openqueue.Enqueue(start, start.Priority);

            while (openqueue.TryDequeue(out var node, out var _))
            {
                // Если узел уже раскрыт, выходим из цикла.
                if (closedset.Contains(node)) { continue; }

                if (node.Equals(goal))
                {
                    Console.WriteLine($"Просмотрено узлов: {closedset.Count}");
                    steps_to_goal.Push(node);
                    goal_attained = true;
                    break;
                }
                closedset.Add(node);

                foreach (var neighbor in node.NeighborNodes())
                {
                    // Добавим в очередь всех соседей узла node.
                    // После этого node считается раскрытым.
                    neighbor.PreviousNode = node;
                    neighbor.Manhatten = NodeMat.ManhattenDist(neighbor, goal);
                    openqueue.Enqueue(neighbor, neighbor.Priority);
                }
            }
            if (goal_attained)
            {
                // Восстановим весь путь от goal до start.
                while (steps_to_goal.Peek().PreviousNode is not null)
                {
                    steps_to_goal.Push(steps_to_goal.Peek().PreviousNode);
                }
                return steps_to_goal;
            }
            // Если целевой узел не достигнут, вместо 
            // стека steps_to_goal вернем пустой стек.
            return new Stack<NodeMat>();
        }
    }
}