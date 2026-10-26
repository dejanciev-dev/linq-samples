using BenchmarkDotNet.Running;

// Usage (always in Release):
//   dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks                     -> interactive picker
//   dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks -- --filter '*Any*' -> run matching benchmarks
//   dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks -- --filter '*' --job short

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
