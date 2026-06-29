namespace TestPilot.Core.Models;

public sealed class TestCase
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Area { get; init; }

    public int DurationSeconds { get; init; }

    /// <summary>
    /// Risk score from 0 to 10.
    /// Higher means the test covers a riskier area.
    /// </summary>
    public double Risk { get; init; }

    /// <summary>
    /// Business criticality score from 0 to 10.
    /// Higher means the related feature is more business-critical.
    /// </summary>
    public double BusinessCriticality { get; init; }

    /// <summary>
    /// Historical failure rate from 0.0 to 1.0.
    /// Higher means this test has failed more often in the past.
    /// </summary>
    public double FailureRate { get; init; }

    /// <summary>
    /// Number of days since the test last failed.
    /// Null means there is no known recent failure.
    /// </summary>
    public int? LastFailedDaysAgo { get; init; }

    /// <summary>
    /// Flakiness score from 0.0 to 1.0.
    /// Higher means the test is less trustworthy.
    /// </summary>
    public double Flakiness { get; init; }

    /// <summary>
    /// Other tests that must be selected before this test is useful or valid.
    /// </summary>
    public IReadOnlyList<string> Requires { get; init; } = Array.Empty<string>();

    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

    public string? Service { get; init; }

    public string? Environment { get; init; }

    public string? Owner { get; init; }

    public string? Country { get; init; }

    public bool IsSmokeTest =>
        Tags.Any(tag => string.Equals(tag, "smoke", StringComparison.OrdinalIgnoreCase));

    public bool HasDependencies => Requires.Count > 0;

    public TimeSpan Duration => TimeSpan.FromSeconds(DurationSeconds);

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Id))
            throw new InvalidOperationException("Test case id is required.");

        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException($"Test case '{Id}' must have a name.");

        if (string.IsNullOrWhiteSpace(Area))
            throw new InvalidOperationException($"Test case '{Id}' must have an area.");

        if (DurationSeconds <= 0)
            throw new InvalidOperationException($"Test case '{Id}' must have a positive duration.");

        ValidateScore(nameof(Risk), Risk, 0, 10);
        ValidateScore(nameof(BusinessCriticality), BusinessCriticality, 0, 10);
        ValidateScore(nameof(FailureRate), FailureRate, 0, 1);
        ValidateScore(nameof(Flakiness), Flakiness, 0, 1);

        if (LastFailedDaysAgo < 0)
            throw new InvalidOperationException($"Test case '{Id}' cannot have a negative LastFailedDaysAgo value.");
    }

    private void ValidateScore(string name, double value, double min, double max)
    {
        if (double.IsNaN(value) || value < min || value > max)
            throw new InvalidOperationException(
                $"Test case '{Id}' has invalid {name}. Expected range: {min} to {max}, actual: {value}.");
    }
}