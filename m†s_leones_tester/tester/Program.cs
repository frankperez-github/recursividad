using System.Text.Json;
using MatCom.Tester;

Directory.CreateDirectory(".output");
var tester = new Tester();
tester.GenerateResponses(0, 5, Path.Combine(".output", "cache.json"));
var result = tester.Test(Path.Combine(".output", "cache.json"), input => Solution.Solve(new Map(Tester.MakeMatrix(input.Item1), input.Item2), input.Item3))!;
File.Delete(Path.Combine(".output", "result.json"));
File.WriteAllText(Path.Combine(".output", "result.json"), JsonSerializer.Serialize(result));