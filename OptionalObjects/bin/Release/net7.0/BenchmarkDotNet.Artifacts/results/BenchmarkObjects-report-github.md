``` ini

BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.4 (22F66) [Darwin 22.5.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.304
  [Host]     : .NET 7.0.7 (7.0.723.27404), Arm64 RyuJIT AdvSIMD [AttachedDebugger]
  DefaultJob : .NET 7.0.7 (7.0.723.27404), Arm64 RyuJIT AdvSIMD


```
|             Method |     N |      Mean |    Error |   StdDev |   Gen0 | Allocated |
|------------------- |------ |----------:|---------:|---------:|-------:|----------:|
| **TestNullableObject** |  **1000** |  **85.94 ns** | **0.254 ns** | **0.225 ns** | **0.0587** |     **368 B** |
| TestOptionalObject |  1000 | 247.96 ns | 0.781 ns | 0.692 ns | 0.2089 |    1312 B |
| **TestNullableObject** | **10000** |  **86.66 ns** | **0.211 ns** | **0.198 ns** | **0.0587** |     **368 B** |
| TestOptionalObject | 10000 | 256.31 ns | 0.708 ns | 0.627 ns | 0.2089 |    1312 B |
