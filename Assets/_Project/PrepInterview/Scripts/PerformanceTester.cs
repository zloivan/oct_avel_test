using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

/// <summary>
/// v2 — исправлены замеры аллокаций (per-run) и добавлен
/// Profiler.GetTotalAllocatedMemoryLong для честного подсчёта байт.
/// </summary>
public class PerformanceTester : MonoBehaviour
{
    [Header("Настройки теста")]
    [SerializeField] private int listSize    = 100_000;
    [SerializeField] private int threshold   = 50_000;
    [SerializeField] private int warmupRuns  = 3;
    [SerializeField] private int measureRuns = 10;   // больше прогонов = точнее среднее

    private void Start() => RunAll();

    private void RunAll()
    {
        Log("══════════════════════════════════════════════════════");
        Log($"[PerfTest] listSize={listSize:N0}  threshold={threshold:N0}");
        Log($"[PerfTest] warmup={warmupRuns}  runs={measureRuns}");
        Log($"[PerfTest] Runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
        Log("══════════════════════════════════════════════════════");

        var source = CreateSourceList(listSize);

        // ── Прогрев JIT ───────────────────────────────────────────────────
        Log("[PerfTest] Прогрев JIT...");
        for (int i = 0; i < warmupRuns; i++)
        {
            _ = RunLinq(source, threshold);
            _ = RunForLoop(source, threshold);
        }
        ForceFullGC();

        // ── Замеры ────────────────────────────────────────────────────────
        var linqStats = Measure("LINQ   ", () => RunLinq(source, threshold),   measureRuns);
        var forStats  = Measure("ForLoop", () => RunForLoop(source, threshold), measureRuns);

        // ── Итоговое сравнение ────────────────────────────────────────────
        PrintComparison(linqStats, forStats);
        Debug.Break();
    }

    // ──────────────────────────────────────────────────────────────────────
    private struct Stats
    {
        public string Label;
        public double AvgMs, MinMs, MaxMs, MedianMs;
        public long   AvgAllocBytes;
        public int    TotalGcGen0;
    }

    private Stats Measure(string label, Func<List<int>> action, int runs)
    {
        var times  = new double[runs];
        var allocs = new long[runs];
        int gcTotal = 0;

        string marker = $"PerfTest_{label.Trim()}";

        for (int i = 0; i < runs; i++)
        {
            ForceFullGC();

            // ── Baseline ─────────────────────────────────────────────────
            int  gcBefore  = GC.CollectionCount(0);
            long memBefore = Profiler.GetTotalAllocatedMemoryLong();  // Unity Profiler heap

            // ── Замер ────────────────────────────────────────────────────
            Profiler.BeginSample(marker);
            var sw = Stopwatch.StartNew();

            var result = action();

            sw.Stop();
            Profiler.EndSample();
            // ─────────────────────────────────────────────────────────────

            int  gcAfter  = GC.CollectionCount(0);
            long memAfter = Profiler.GetTotalAllocatedMemoryLong();

            times[i]  = TicksToMs(sw.ElapsedTicks);
            allocs[i] = memAfter - memBefore;
            gcTotal  += gcAfter - gcBefore;

            Log($"  [{label}] #{i + 1,2}/{runs} | " +
                $"{times[i]:F4} ms | " +
                $"alloc: {allocs[i] / 1024.0:+0.0;-0.0;0} KB | " +
                $"GC gen0Δ: {gcAfter - gcBefore} | " +
                $"count={result.Count}");
        }

        Array.Sort(times);
        double avg = 0;
        foreach (var t in times) avg += t;
        avg /= runs;

        long avgAlloc = 0;
        foreach (var a in allocs) avgAlloc += a;
        avgAlloc /= runs;

        var stats = new Stats
        {
            Label         = label.Trim(),
            AvgMs         = avg,
            MinMs         = times[0],
            MaxMs         = times[runs - 1],
            MedianMs      = times[runs / 2],
            AvgAllocBytes = avgAlloc,
            TotalGcGen0   = gcTotal,
        };

        Log($"  [{label}] ── avg={stats.AvgMs:F4} ms | " +
            $"min={stats.MinMs:F4} | max={stats.MaxMs:F4} | median={stats.MedianMs:F4} | " +
            $"avgAlloc={stats.AvgAllocBytes / 1024.0:F1} KB | " +
            $"GC gen0 total={stats.TotalGcGen0}");
        Log("──────────────────────────────────────────────────────");

        return stats;
    }

    // ──────────────────────────────────────────────────────────────────────
    private static void PrintComparison(Stats a, Stats b)
    {
        Log("══════════════════  СРАВНЕНИЕ  ══════════════════════");
        Log($"  {"Метрика",-22} {"LINQ",12} {"ForLoop",12} {"LINQ/ForLoop",12}");
        Log($"  {"avg время (ms)",-22} {a.AvgMs,12:F4} {b.AvgMs,12:F4} {Ratio(a.AvgMs, b.AvgMs),12}");
        Log($"  {"median время (ms)",-22} {a.MedianMs,12:F4} {b.MedianMs,12:F4} {Ratio(a.MedianMs, b.MedianMs),12}");
        Log($"  {"min время (ms)",-22} {a.MinMs,12:F4} {b.MinMs,12:F4} {Ratio(a.MinMs, b.MinMs),12}");
        Log($"  {"avgAlloc (KB)",-22} {a.AvgAllocBytes/1024.0,12:F1} {b.AvgAllocBytes/1024.0,12:F1} {Ratio(a.AvgAllocBytes, b.AvgAllocBytes),12}");
        Log($"  {"GC gen0 total",-22} {a.TotalGcGen0,12} {b.TotalGcGen0,12}");
        Log("══════════════════════════════════════════════════════");
    }

    private static string Ratio(double x, double y)
        => y == 0 ? "—" : $"{x / y:F2}x";

    // ──────────────────────────────────────────────────────────────────────
    private static List<int> RunLinq(List<int> source, int threshold)
        => source.Where(x => x > threshold).ToList();

    private static List<int> RunForLoop(List<int> source, int threshold)
    {
        var result = new List<int>(source.Count / 2);
        for (int i = 0; i < source.Count; i++)
            if (source[i] > threshold)
                result.Add(source[i]);
        return result;
    }

    private static List<int> CreateSourceList(int size)
    {
        var list = new List<int>(size);
        for (int i = 1; i <= size; i++) list.Add(i);
        return list;
    }

    private static void ForceFullGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private static double TicksToMs(long ticks)
        => ticks * 1000.0 / Stopwatch.Frequency;

    private static void Log(string msg)
        => UnityEngine.Debug.Log(msg);
}