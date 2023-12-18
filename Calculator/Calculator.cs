namespace Calculator
{
    public class Token
    {
        // класс для создания токенов из элементов входной строки
        public int Type { get; set; }
        public string Value { get; set; }
        public int Priority { get; set; }
        public Token(int type, string value)
        {
            this.Type = type; // 0 - число, 1 - операция, 2 - откр. скобка, 3 - закр. скобка  
            this.Value = value;
        }
        public static int GetPriority(string operation)
        {
            // приоритет операций: 1 для "+" и "-", 2 для "*" и "/"

            if (operation == "+" || operation == "-") { return 1; } 
            else { return 2; }
        }
    }
    public class Tokenizer
    {
        // класс для разбора строки на токены
        public static Queue<Token> Tokenize(string input)
        {
            string[] chars = { "+", "-", "*", "/", "(", ")" };

            string input_spacedel = input;
            foreach (string c in chars) { input_spacedel = input_spacedel.Replace(c, $" {c} "); }
            string[] input_tokens = input_spacedel.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Queue<Token> tokens = new();
            foreach (string s in input_tokens)
            {
                if (Array.IndexOf(chars, s) == -1)
                {
                    tokens.Enqueue(new Token(0, s));
                }
                else if (s == "(")
                {
                    tokens.Enqueue(new Token(2, s));
                }
                else if (s == ")")
                {
                    tokens.Enqueue(new Token(3, s));
                }
                else
                {
                    int priority = Token.GetPriority(s);
                    tokens.Enqueue(new Token(1, s) { Priority = priority });
                }
            }
            return tokens;
        }
    }
    public class RPN
    {
        // класс для получения выражения обратной польской записи (ОПЗ)
        public static Queue<Token> GetRPN(Queue<Token> input)
        {
            // метод для нахождения ОПЗ с помощью алгоритма сортировочной станции
            Stack<Token> operations = new();
            Queue<Token> output = new();

            while (input.TryDequeue(out var token))
            {
                switch (token.Type)
                {
                    case 0:
                        output.Enqueue(token);
                        break;
                    case 1:
                        while (operations.TryPeek(out var top))
                        {
                            if (top.Type == 1 && (top.Priority >= token.Priority))
                            {
                                output.Enqueue(operations.Pop());
                            }
                            else break;
                        }
                        operations.Push(token);
                        break;
                    case 2:
                        operations.Push(token);
                        break;
                    default:
                        bool is_found = false;
                        while (operations.TryPop(out var top))
                        {
                            if (top.Type != 2)
                            {
                                output.Enqueue(top);
                            }
                            else if (top.Type == 2)
                            {
                                is_found = true;
                                break;
                            }
                        }
                        if (!is_found) throw new Exception("Пропущена скобка");
                        break;
                }
            }

            while (operations.TryPop(out var top))
            {
                if (top.Type != 2)
                {
                    output.Enqueue(top);
                }
                else
                    throw new Exception("Пропущена скобка");
            }
            return output;
        }
    }
    public class Calculator
    {
        // класс для вычисления значения выражения по ОПЗ
        public static double CalculateFromQueue(Queue<Token> input)
        {
            // метод для подсчета значения выражения в ОПЗ

            Stack<double> computations = new();

            while (input.TryDequeue(out var token))
            {
                if (token.Type == 0)
                {
                    computations.Push(Convert.ToDouble(token.Value));
                }
                else
                {
                    double result;
                    double y = computations.Pop();
                    double x = computations.Pop();

                    switch (token.Value)
                    {
                        case "+":
                            result = x + y;
                            break;
                        case "-":
                            result = x - y;
                            break;
                        case "*":
                            result = x * y;
                            break;
                        default:
                            result = x / y;
                            break;
                    }
                    computations.Push(result);
                }
            }
            return computations.Pop();
        }
        public static void CalculateInteractive()
        {
            // метод для запуска калькулятора из консоли
            // для выхода нужно ввести пустую строку

            while (true)
            {
                string? input_read = Console.ReadLine();
                if (input_read.Length == 0)
                {
                    break;
                }
                else
                {
                    var tokens = Tokenizer.Tokenize(input_read);
                    var rpn_queue = RPN.GetRPN(tokens);
                    double result = CalculateFromQueue(rpn_queue);
                    Console.WriteLine($"Ответ: {result}");
                }
            }
        }
        public static double CalculateFromString( string input )
        {
            // основной метод для вычисления значения выражения поданной строки

            var tokens = Tokenizer.Tokenize(input);
            var rpn_queue = RPN.GetRPN(tokens);
            double result = CalculateFromQueue(rpn_queue);

            return result;
        }
    }
}