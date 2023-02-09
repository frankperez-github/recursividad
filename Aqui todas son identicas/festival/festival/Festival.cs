using System;
namespace Weboo.Examen {

    public class Festival
    {
        public static int Min = int.MaxValue;
        public static int MenorCantidadEquipos(bool[,] amigos)
        {
            // Borre esta línea y escriba su código
            int personas = amigos.GetLength(0);
            int[] toCombine = new int[personas + personas-1];
            for (int i = 0; i < personas; i++)
            {
                toCombine[i] = i;
            }
            for (int i = personas; i < toCombine.Length; i++)
            {
                toCombine[i] = -1;
            }
            bool[] boolCombine = new bool[personas + personas-1];
            int[] answer = new int[toCombine.Length];
            for (int i = 0; i < answer.Length; i++)
            {
                answer[i] = -1;
            }
            List<int[]> answers = new List<int[]>();
            // Console.WriteLine(IsValid(new int[]{0,3,1,-1}, amigos));
            // return 0;
            // Console.WriteLine(CountTeams(new int[]{-1, 2, 2, 2, -1}));
            Min = int.MaxValue;
            return GetTeams(amigos, toCombine, boolCombine, 0, answer);
        }

        public static int GetTeams(bool[,] amigos, int[] toCombine, bool[] boolCombine,int index, int[]answer)
        {
            if(Min == 1) return 1;
            if(index == answer.Length)
            {
                int localMin = CountTeams(answer);
                Min = Math.Min(Min, localMin);
            }

            for (int i = 0; i < toCombine.Length; i++)
            {
                if(!boolCombine[i])
                {
                    answer[index] = toCombine[i];
                    if(IsValid(answer, amigos))
                    {
                        boolCombine[i] = true;
                        GetTeams(amigos, toCombine, boolCombine, index+1, answer);
                        boolCombine[i] = false;
                    }
                    answer[index] = -1;
                }
            }

            return Min;
        }

        public static bool IsValid(int[]array, bool[,] amigos)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if(array[i] != -1)
                {
                    for (int j = i+1; j < array.Length; j++)
                    {
                        if(array[j]!=-1)
                        {
                            if(amigos[array[i],array[j]]) return false;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return true;
        }
        public static int CountTeams(int[] array)
        {
            int count = 0;
            bool counting = true;
            for (int i = 0; i < array.Length; i++)
            {
                if(array[i] == -1)
                {
                    counting = true;
                }
                else if(counting)
                {
                    count++;
                    counting = false;
                }
            }
            return count;
        }
    }

}