using System.Reflection.Metadata;
namespace Name
{
    class Program
    {
        public static int Main()
        {
            string[,] board = new string[,]
            {
                {"0","0","0","0"},
                {"0","0","0","0"},
                {"0","X","0","0"},
                {"0","0","0","0"},

            };
            bool[,] boolBoard = new bool[board.GetLength(0), board.GetLength(0)];
            Dictionary<(int f, int c), bool> moves = new Dictionary<(int f, int c), bool>();
            moves.Add((1,0),false);
            moves.Add((-1,0),false);
            moves.Add((0,1),false);
            moves.Add((0,-1),false);
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if(i==2 && j ==2)
                    {
                        Console.WriteLine("2,2");
                    }
                    rellenar(moves, board, boolBoard, i, j, 1, 3);
                }
            }
            return 0;
        }
        public static void rellenar(Dictionary<(int f, int c), bool> moves, string[,]board, bool[,] boolBoard, int currF, int currC, int num, int numCount)
        {
            if(Finished(board))
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        Console.Write(board[i,j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                return;
            }

            board[currF, currC] = num.ToString();
            foreach (var move in moves)
            {
                try
                {
                    if(!move.Value)
                    {
                        if(board[currF+move.Key.f, currC+move.Key.c] != "X" && !boolBoard[currF+move.Key.f, currC+move.Key.c])
                        {
                            //Caminar hacia move
                            foreach (var Move in moves)
                            {
                                moves[Move.Key] = false;
                            }

                            moves[move.Key] = true;
                            (int , int) contrario = (move.Key.f*(-1), move.Key.c*(-1));
                            moves[(contrario)] = true;

                            int prevNum = num;
                            int prevCount = numCount;
                            

                            if(numCount>=2) numCount--;
                            else
                            {
                                num++;
                                numCount = 3;
                                foreach (var Move in moves)
                                {
                                    moves[Move.Key] = false;
                                }
                            }

                            boolBoard[currF+move.Key.f, currC+move.Key.c] = true;
                            rellenar(moves, board, boolBoard, currF+move.Key.f, currC+move.Key.c, num, numCount);
                            boolBoard[currF+move.Key.f, currC+move.Key.c] = false;
                            board[currF+move.Key.f, currC+move.Key.c] = "0";

                            num = prevNum;
                            numCount = prevCount;

                            moves[move.Key] = false;
                            moves[(contrario)] = false;
                        }
                    }
                }
                catch (System.Exception){}
            }
        }
        public static bool Finished(string[,]board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if(board[i,j] == "0") return false;
                }
            }
            return true;
        }
    }
}