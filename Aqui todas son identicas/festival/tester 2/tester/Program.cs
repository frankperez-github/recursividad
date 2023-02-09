using Weboo.Examen;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;


class Program
{
    static Logging logging = Logging.Info;


    static int seed = 0;
    static string cachedEasyResponses = @"./cached_easy_responses.json";
    static string cachedHardResponses = @"./cached_hard_responses.json";

    static void Main()
    {
        // Descomenten esto  si cambian algo en los casos de prueba, aqui se cachea las respuestas
        // GenerateResponses(seed, GenerateEasy, TestResolver.MenorCantidadEquipos, cachedEasyResponses);
        // GenerateResponses(seed, GenerateHard, TestResolverEficiente.AsignarCamaras, cachedHardResponses);

        var pass = Test(seed, @"./result_easy.json", cachedEasyResponses, GenerateEasy, 30);
        // if (pass)
        //     Test(seed, @"./result_hard.json", cachedHardResponses, GenerateHard, 15);
    }


    // static bool[,] GenerateHard(int seed)
    // {
    //     return Generate(seed, 15, 25, 0.0f, 1.0f);
    // }

    static bool[,] GenerateEasy(int seed)
    {
        return Generate(seed, 1, 14, 0.0f, 1.0f);
    }

    static bool[,] Generate(int seed, int minSize, int maxSize, float minFactor, float maxFactor)
    {
        Random random = new Random(seed);

        int size = random.Next(minSize, maxSize);

        int maxPosibleFactor = size * (size - 1) / 2;

        int totalFlags = random.Next((int)(minFactor * maxPosibleFactor), (int)(maxFactor * maxPosibleFactor));

        bool[,] result = new bool[size, size];

        List<(int, int)> positions = new List<(int, int)>();

        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                positions.Add((i, j));
            }
        }

        for (int i = 0; i < totalFlags; i++)
        {
            int j = random.Next(0, positions.Count);

            (int x, int y) = positions[j];
            positions.RemoveAt(j);

            result[x, y] = result[y, x] = true;
        }

        Console.WriteLine();
        Console.WriteLine($"Size: {size} Flags: {totalFlags} ({(maxPosibleFactor == 0 ? 0 : totalFlags * 100 / maxPosibleFactor)}%)");
        if (logging == Logging.Verbose)
        {
            Console.WriteLine();
            Console.Write("  ");
            for (int j = 0; j < size; j++)
            {
                // Console.ForegroundColor = j % 2 == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write(j > 9 ? j.ToString() : " " + j);
            }
            Console.WriteLine();
            for (int i = 0; i < size; i++)
            {
                // Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.ResetColor();
                Console.Write(i > 9 ? i.ToString() : i + " ");
                for (int j = 0; j < size; j++)
                {
                    Console.ForegroundColor = (j + i) % 3 == 0 ? ConsoleColor.Green : ConsoleColor.Blue;
                    Console.Write(result[i, j] ? " O" : "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        return result;
    }


    // Usé esto para cachear las respuestas nuestras* y no perder tiempo recalculando, ya que siempre se evalua lo mismo
    // Para los algoritmos inteligentes, utilicé 15 15 de base, y para los que no tienen poda utilicé  5 5

    static void GenerateResponses(int baseSeed, Func<int, bool[,]> generateFunc, Func<bool[,], int[]> testFunc, string cacheFile)
    {
        List<Object> responses = new List<Object>();
        for (int s = 0; s < 100; s++)
        {
            var seed = baseSeed + s;

            var friends = generateFunc.Invoke(seed);
            (int[] solution, long time) = RunTask<int[]>(() => testFunc.Invoke(friends), null);

            int result = solution.Distinct().Count();

            for (int i = 0; i < friends.GetLength(0); i++)
            {
                for (int j = i + 1; j < friends.GetLength(1); j++)
                {
                    // Dos amigos no pueden pertenecer al mismo equipo
                    if (friends[i, j] && solution[i] == solution[j])
                    {
                        throw new Exception("La solución retornada no es válida");
                    }
                }
            }



            List<int[]> e = new List<int[]>();

            for (int i = 0; i < friends.GetLength(0); i++)
            {
                for (int j = i + 1; j < friends.GetLength(0); j++)
                {
                    if (friends[i, j])
                        e.Add(new[] { i, j });
                }

            }

            var caseResult = new CaseCache
            {
                seed = seed,
                result = result,
                time = time,
                param = e,
                solution = solution,
            };

            Console.WriteLine(JsonSerializer.Serialize(caseResult));

            responses.Add(caseResult);
        }

        string json = JsonSerializer.Serialize(responses.ToArray());

        File.WriteAllText(cacheFile, json);
    }

    public static bool Test(int seed, string resultFile, string cachedResponses, Func<int, bool[,]> generateFunc, int maxTimeOuts)
    {
        int ok = 0;
        int wrong = 0;
        int exception = 0;
        int timeout = 0;

        int totalCases = 100;
        double percentToPass = 90;


        List<object> caseResults = new List<object>();

        List<CaseCache> cache = JsonSerializer.Deserialize<List<CaseCache>>(File.ReadAllText(cachedResponses));


        for (int i = 0; i < 100; i++)
        {


            // i % 100
            int caseSeed = i + seed;
            var expectedResult = cache[i].result;
            var param = generateFunc(caseSeed);

            (ResultType result, long time) = RunTask(() =>
            {
                try
                {
                    var studentSolution = Festival.MenorCantidadEquipos(param);

                    // int studentResult = studentSolution.Distinct().Count();

                    Console.WriteLine($"Expected: {expectedResult}\nResult: {studentSolution}");

                    if (expectedResult == studentSolution)
                    {
                        // Validate answer
                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            for (int j = i + 1; j < param.GetLength(1); j++)
                            {
                                // Dos amigos no pueden pertenecer al mismo equipo
                                if (param[i, j] && studentSolution[i] == studentSolution[j])
                                {
                                   return ResultType.Wrong;
                                }
                            }
                        }

                        return ResultType.Ok;
                    }
                    else return ResultType.Wrong;
                }
                catch (Exception)
                {
                    return ResultType.Exception;
                }

            }, ResultType.TimeOut, maxTimeOuts);

            if (result == ResultType.Ok)
                ok++;
            else if (result == ResultType.Wrong)
                wrong++;
            else if (result == ResultType.Exception)
                exception++;
            else
                timeout++;


            var isOk = result == ResultType.Ok;


            Console.ForegroundColor = isOk ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"Case {i}: {result} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{time}s");
            Console.ResetColor();

            caseResults.Add(new
            {
                seed = caseSeed,
                time = time,
                result = result.ToString(),
                lazyResult = 0,
                lazyTime = 0,
            });


            if (timeout > maxTimeOuts)
                break;
        }


        double percent = ok * 100 / (double)totalCases;
        Console.WriteLine($"Results: {ok} de {totalCases} {percent}% ");

        Console.ForegroundColor = percent >= percentToPass ? ConsoleColor.Green : ConsoleColor.Red;
        Console.ResetColor();
        Console.WriteLine(percent >= percentToPass ? "APROBADO" : "SUSPENSO");



        string json = JsonSerializer.Serialize(new
        {
            ok = ok,
            wrong = wrong,
            timeout = timeout,
            exception = exception,
            total = totalCases,
            seed = seed,
            caseResults = caseResults.ToArray(),
        });


        File.WriteAllText(resultFile, json);

        return percent >= percentToPass;
    }


    static (T, long) RunTask<T>(Func<T> func, T onTimeOut, int seconds = 10)
    {
        T result = onTimeOut;
        long time = -1;

        Task<T> task = Task.Run(func);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        if (task.Wait(new TimeSpan(seconds: seconds, hours: 0, minutes: 0)))
        {
            result = task.Result;
        }

        time = stopwatch.ElapsedMilliseconds;

        return (result, time);
    }


}




class CaseCache
{
    public int seed { get; set; }

    public int result { get; set; }
    public long time { get; set; }

    public List<int[]> param { get; set; }

    public int[] solution { get; set; }
}


enum Logging
{
    Info, Verbose
}

public enum ResultType
{
    Ok, Wrong, Exception, TimeOut
}