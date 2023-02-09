using System.Globalization;
public static class Solution
{
    public static int[] Capacities = new int[0];
    public static int Solve(Map map, int[] capacities)
    {
        Capacities = capacities;

        #region params of solve

        List<int[]> answers = new List<int[]>();
        int[] answer = new int[map.M - 1 + capacities.Length];

        int[] toCombine = new int[answer.Length-1];
        for (int i = 0 ; i < answer.Length-1; i++)
        {
            toCombine[i] = i;
        }

        bool[] boolToCombine = new bool[toCombine.Length];
        #endregion

        answers = Solve(map, toCombine, boolToCombine, answer, new List<int[]>(), 0);

        if (answers.Count() != 0)
        {
            List<int[]> sortedByCost = answers.OrderByDescending((x) => CalcCost(x, map)).ToList(); // Ordenar descendentemente por costo
             
            return CalcCost(sortedByCost[sortedByCost.Count-1], map); // Devolver costo del ultimo (smaller)
        }
        return int.MaxValue;

        
        // Console.WriteLine(IsValid(new int[]{3,1,0,2,0}, map));
        // return 0;
    }

    public static int CalcCost(int[] array, Map map)
    {
        int prev = 0;
        int costo = 0;
        for (int i = 0; i < array.Length; i++)
        {
            costo += map[prev, array[i]];
            prev = array[i];
        }
        return costo;
    }

    public static List<int[]> Solve(Map map, int[]toCombine, bool[] boolToCombine, int[] answer, List<int[]> answers, int index)
    {
        if (index == answer.Length-1)
        {
            if (answer.Distinct().ToArray().Length == map.Demand.Length)
            {
                if (IsValid(answer, map))
                {
                    int[] toAdd = new int[answer.Length];
                    for (int i = 0; i < answer.Length; i++)
                    {
                        toAdd[i] = answer[i];
                    }
                    answers.Add(toAdd);

                    return answers;
                }
            }
        }
                        // Penultimo (al final hay un 0 fijo)
        for (int i = index; i < answer.Length-1; i++)
        {
            for (int j = 0; j < toCombine.Length; j++)
            {
                if (!boolToCombine[j])
                {
                    answer[i] = toCombine[j];
                    boolToCombine[j] = true;
                    if (!IsValid(answer, map)) // Poda
                    {
                    //     answer[i] = 0;
                    //     boolToCombine[j] = false;
                        continue; 
                    } 
                    Solve(map, toCombine, boolToCombine, answer, answers, index+1);
                    answer[i] = 0;
                    boolToCombine[j] = false;
                }
            }
        }
        return answers;
    }

    public static bool IsValid(int[] array, Map map)
    {
        
        bool[] boolCapacities = new bool[Capacities.Length];
        List<int> sumCapacities = new List<int>();

        int capacity = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if(array[i] != 0)
            {
                capacity += map.Demand[array[i]];
            }
            else
            {
                sumCapacities.Add(capacity);
                capacity = 0;
            }
        }

        int[] sortedSums = sumCapacities.OrderBy(x => x).ToArray();
        int[] sortedCapacities = Capacities.OrderBy(x => x).ToArray();

        bool flag = true;
        for (int i = 0; i < sortedSums.Length; i++)
        {
            flag = true;
            if (sortedSums[i] == 0) continue;

            for (int j = 0; j < sortedCapacities.Length; j++)
            {
                if (!boolCapacities[j])
                {

                    if (sortedSums[i] <= sortedCapacities[j])
                    {
                        flag = false;
                        boolCapacities[j] = true;
                        break;
                    }
                }
            }
            if (flag) 
            {
                
                // Console.WriteLine(sortedSums[i]+","+ sortedCapacities[j]);
                // Console.WriteLine(sortedSums[i]);
                // Console.WriteLine(flag);
                return false;
            }
        }
        if(flag) return false;

        return true;
    }
}
