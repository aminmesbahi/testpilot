namespace TestPilot.Core.Models;

public sealed class OptimizationSummary
{
    public required string AlgorithmName { get; init; }

    public int SelectedTests { get; init; }

    public int TotalTests { get; init; }

    public int TotalDurationSeconds { get; init; }

    public int BudgetSeconds { get; init; }

    public double Fitness { get; init; }

    public int CoveredAreas { get; init; }

    public int TotalAreas { get; init; }

    public int DependencyViolationCount { get; init; }

    public double AverageFlakiness { get; init; }

    public int Iterations { get; init; }

    public int EvaluatedCandidates { get; init; }

    public double ElapsedMilliseconds { get; init; }

    public double BudgetUsagePercent =>
        BudgetSeconds == 0
            ? 0
            : 100.0 * TotalDurationSeconds / BudgetSeconds;

    public double AreaCoveragePercent =>
        TotalAreas == 0
            ? 0
            : 100.0 * CoveredAreas / TotalAreas;
}