namespace Name
{
    class Program
    {
        public static int Main()
        {
            MakeRing(4);
            return 0;
        }
        public static void MakeRing(int n)
        {
            int[] perlas = new int[n];
            for (int i = 1; i <= n; i++)
            {
                perlas[i-1] = i;
            }
            MakeRing(n, perlas, new int[n], new bool[n], 0);
        }

        public static void MakeRing(int n, int[]perlas, int[]anillo, bool[]boolperlas, int index)
        {
            if(index == anillo.Length)
            {
                Console.WriteLine(string.Join(',', anillo));
            }
            for (int i = 0; i < n; i++)
            {
                if(!boolperlas[i])
                {
                    anillo[index] = perlas[i];
                    if(IsValid(anillo))
                    {
                    boolperlas[i] = true;
                    MakeRing(n, perlas, anillo, boolperlas, index+1);
                    boolperlas[i] = false;
                    }
                    anillo[index] = 0;
                }
            }
        }

        public static bool IsValid(int[] anillo)
        {
            for (int i = 0; i < anillo.Length-1; i++)
            {
                if(!IsPrimo(anillo[i]+anillo[i+1])) return false;
            }
            if(!IsPrimo(anillo[0]+anillo[anillo.Length-1])) return false;
            return true;
        }
        public static bool IsPrimo(int n)
        {
            bool[] criba = new bool[n+1];

            for (int i = 2; i < n; i++)
            {
                if(!criba[i])
                {
                    for (int j = i; j <= n; j+=i)
                    {
                        criba[j] = true;
                    }
                }
            }

            return !criba[n];
        }
    }
}