namespace TestPilot.Core.Models;

public sealed class OptimizationResult
{
    public required string AlgorithmName { get; init; }

    public required TestSelection Selection { get; init; }

    public required double Fitness { get; init; }

    public required TimeSpan Budget { get; init; }

    public required TimeSpan Elapsed { get; init; }

    public int Iterations { get; init; }

    public int EvaluatedCandidates { get; init; }

    public bool IsWithinBudget => Selection.IsWithinBudget(Budget);

    public IReadOnlyList<DependencyViolation> DependencyViolations =>
        Selection.GetDependencyViolations();

    public IReadOnlyDictionary<string, object> Metadata { get; init; } =
        new Dictionary<string, object>();

    public OptimizationSummary ToSummary(TestSuite suite)
    {
        var coveredAreas = Selection.CoveredAreas.Count;
        var totalAreas = suite.Areas.Count;

        return new OptimizationSummary
        {
            AlgorithmName = AlgorithmName,
            SelectedTests = Selection.Count,
            TotalTests = suite.Count,
            TotalDurationSeconds = Selection.TotalDurationSeconds,
            BudgetSeconds = (int)Budget.TotalSeconds,
            Fitness = Fitness,
            CoveredAreas = coveredAreas,
            TotalAreas = totalAreas,
            DependencyViolationCount = DependencyViolations.Count,
            AverageFlakiness = Selection.AverageFlakiness,
            Iterations = Iterations,
            EvaluatedCandidates = EvaluatedCandidates,
            ElapsedMilliseconds = Elapsed.TotalMilliseconds
        };
    }
}