namespace TestPilot.Core.Models;

public sealed class TestSuite
{
    public required string Name { get; init; }

    public required IReadOnlyList<TestCase> Tests { get; init; }

    public int Count => Tests.Count;

    public TimeSpan TotalDuration =>
        TimeSpan.FromSeconds(Tests.Sum(test => test.DurationSeconds));

    public IReadOnlySet<string> Areas =>
        Tests.Select(test => test.Area)
            .Where(area => !string.IsNullOrWhiteSpace(area))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public IReadOnlySet<string> Services =>
        Tests.Select(test => test.Service)
            .Where(service => !string.IsNullOrWhiteSpace(service))
            .Select(service => service!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public TestCase? FindById(string id)
    {
        return Tests.FirstOrDefault(test =>
            string.Equals(test.Id, id, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyDictionary<string, TestCase> ToDictionaryById()
    {
        return Tests.ToDictionary(
            test => test.Id,
            StringComparer.OrdinalIgnoreCase);
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException("Test suite name is required.");

        if (Tests is null || Tests.Count == 0)
            throw new InvalidOperationException("Test suite must contain at least one test.");

        foreach (var test in Tests)
            test.Validate();

        var duplicatedIds = Tests
            .GroupBy(test => test.Id, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToArray();

        if (duplicatedIds.Length > 0)
        {
            throw new InvalidOperationException(
                $"Test suite contains duplicated test ids: {string.Join(", ", duplicatedIds)}");
        }

        ValidateDependencies();
    }

    private void ValidateDependencies()
    {
        var knownIds = Tests
            .Select(test => test.Id)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingDependencies = Tests
            .SelectMany(test => test.Requires.Select(requiredId => new
            {
                TestId = test.Id,
                RequiredId = requiredId
            }))
            .Where(item => !knownIds.Contains(item.RequiredId))
            .ToArray();

        if (missingDependencies.Length == 0)
            return;

        var message = string.Join(
            Environment.NewLine,
            missingDependencies.Select(item =>
                $"Test '{item.TestId}' requires missing test '{item.RequiredId}'."));

        throw new InvalidOperationException(message);
    }
}