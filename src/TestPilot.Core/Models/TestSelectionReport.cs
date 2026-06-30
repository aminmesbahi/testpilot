namespace TestPilot.Core.Models;

public sealed class TestSelectionReport
{
    public required OptimizationSummary Summary { get; init; }

    public required IReadOnlyList<TestCase> SelectedTests { get; init; }

    public required IReadOnlyList<FeatureAreaCoverage> AreaCoverage { get; init; }

    public required IReadOnlyList<DependencyViolation> DependencyViolations { get; init; }

    public static TestSelectionReport Create(TestSuite suite, OptimizationResult result)
    {
        var selectedIds = result.Selection.SelectedIds;

        var areaCoverage = suite.Tests
            .GroupBy(test => test.Area, StringComparer.OrdinalIgnoreCase)
            .Select(group =>
            {
                var testsInArea = group.ToArray();
                var selectedInArea = testsInArea
                    .Where(test => selectedIds.Contains(test.Id))
                    .ToArray();

                return new FeatureAreaCoverage
                {
                    Area = group.Key,
                    SelectedTestCount = selectedInArea.Length,
                    TotalTestCount = testsInArea.Length,
                    SelectedRisk = selectedInArea.Sum(test => test.Risk),
                    TotalRisk = testsInArea.Sum(test => test.Risk)
                };
            })
            .OrderByDescending(area => area.RiskCoveragePercent)
            .ThenBy(area => area.Area)
            .ToArray();

        return new TestSelectionReport
        {
            Summary = result.ToSummary(suite),
            SelectedTests = result.Selection.SelectedTests,
            AreaCoverage = areaCoverage,
            DependencyViolations = result.DependencyViolations
        };
    }
}