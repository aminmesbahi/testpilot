namespace TestPilot.Core.Models;

public sealed record DependencyViolation(
    string TestId,
    string RequiredTestId);