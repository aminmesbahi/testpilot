namespace TestPilot.Core.Models;

public sealed class FeatureAreaCoverage
{
    public required string Area { get; init; }

    public int SelectedTestCount { get; init; }

    public int TotalTestCount { get; init; }

    public double SelectedRisk { get; init; }

    public double TotalRisk { get; init; }

    public double CoveragePercent =>
        TotalTestCount == 0
            ? 0
            : 100.0 * SelectedTestCount / TotalTestCount;

    public double RiskCoveragePercent =>
        TotalRisk == 0
            ? 0
            : 100.0 * SelectedRisk / TotalRisk;
}