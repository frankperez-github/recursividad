namespace MatCom.Tester;

public class Tester : TesterBase<int, Tuple<int[][], int[], int[]>, int, int>
{
    public override Tuple<int[][], int[], int[]> InputGenerator(int seed, int arg)
    {
        Random random = new Random(seed);
        int N = random.Next(1, arg);
        int M = random.Next(1, arg);
        int[][] matrix = new int[M][];
        int[] demand = new int[M];
        int[] capacities = new int[N];
        for (int i = 0; i < M; i++)
        {
            matrix[i] = new int[M];
        }
        for (int i = 0; i < M; i++)
        {
            demand[i] = random.Next(0, 100);
            
            for (int j = i; j < M; j++)
            {
                matrix[i][j] = matrix[j][i] = i == j? 0: random.Next(0, 100);
            }   
        }
        demand[0] = 0;
        for (int i = 0; i < N; i++)
        {
            capacities[i] = random.Next(0, 100);
        }
        return new Tuple<int[][], int[], int[]>(matrix, demand, capacities);
    }

    public override bool OutputChecker(Tuple<int[][], int[], int[]> input, int output, int expectedOutput)
    {
        return output == expectedOutput;
    }

    public override int OutputGenerator(Tuple<int[][], int[], int[]> input)
    {
        return Solve(new Map(MakeMatrix(input.Item1), input.Item2), input.Item3);
    }

    public static int Solve(Map map, int[] capacities)
    {
        throw new NotImplementedException();
    }


    public static int[,] MakeMatrix(int[][] input)
    {
        int[,] matrix = new int[input[0].Length, input[0].Length];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = i; j < matrix.GetLength(1); j++)
            {
                matrix[i,j] = matrix[j,i] = input[i][j];
            }
        }
        return matrix;
    }

}
