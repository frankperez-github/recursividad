namespace Name
{
    class Program
    {
        public static int Main()
        {
            Console.WriteLine(IsPrimo(13));
            return 0;
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