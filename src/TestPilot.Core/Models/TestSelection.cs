namespace TestPilot.Core.Models;

public sealed class TestSelection
{
    public required IReadOnlyList<TestCase> SelectedTests { get; init; }

    public int Count => SelectedTests.Count;

    public int TotalDurationSeconds =>
        SelectedTests.Sum(test => test.DurationSeconds);

    public TimeSpan TotalDuration =>
        TimeSpan.FromSeconds(TotalDurationSeconds);

    public double TotalRisk =>
        SelectedTests.Sum(test => test.Risk);

    public double TotalBusinessCriticality =>
        SelectedTests.Sum(test => test.BusinessCriticality);

    public double AverageFlakiness =>
        SelectedTests.Count == 0
            ? 0
            : SelectedTests.Average(test => test.Flakiness);

    public IReadOnlySet<string> CoveredAreas =>
        SelectedTests
            .Select(test => test.Area)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public IReadOnlySet<string> SelectedIds =>
        SelectedTests
            .Select(test => test.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public bool IsEmpty => SelectedTests.Count == 0;

    public bool IsWithinBudget(TimeSpan budget)
    {
        return TotalDuration <= budget;
    }

    public IReadOnlyList<DependencyViolation> GetDependencyViolations()
    {
        var selectedIds = SelectedIds;

        return SelectedTests
            .SelectMany(test => test.Requires
                .Where(requiredId => !selectedIds.Contains(requiredId))
                .Select(requiredId => new DependencyViolation(
                    TestId: test.Id,
                    RequiredTestId: requiredId)))
            .ToArray();
    }

    public static TestSelection Empty { get; } = new()
    {
        SelectedTests = Array.Empty<TestCase>()
    };
}