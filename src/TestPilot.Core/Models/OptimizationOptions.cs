namespace TestPilot.Core.Models;

public sealed class OptimizationOptions
{
    public TimeSpan TimeBudget { get; init; } = TimeSpan.FromMinutes(30);

    public int? RandomSeed { get; init; }

    public int MaxIterations { get; init; } = 1_000;

    public int MaxEvaluatedCandidates { get; init; } = 10_000;

    public bool EnforceDependencies { get; init; } = true;

    public bool AllowBudgetOverflow { get; init; } = false;

    public double BudgetOverflowPenaltyWeight { get; init; } = 4.0;

    public double DependencyViolationPenaltyWeight { get; init; } = 5.0;

    public double FlakinessPenaltyWeight { get; init; } = 2.0;

    public void Validate()
    {
        if (TimeBudget <= TimeSpan.Zero)
            throw new InvalidOperationException("Time budget must be positive.");

        if (MaxIterations <= 0)
            throw new InvalidOperationException("MaxIterations must be positive.");

        if (MaxEvaluatedCandidates <= 0)
            throw new InvalidOperationException("MaxEvaluatedCandidates must be positive.");

        if (BudgetOverflowPenaltyWeight < 0)
            throw new InvalidOperationException("BudgetOverflowPenaltyWeight cannot be negative.");

        if (DependencyViolationPenaltyWeight < 0)
            throw new InvalidOperationException("DependencyViolationPenaltyWeight cannot be negative.");

        if (FlakinessPenaltyWeight < 0)
            throw new InvalidOperationException("FlakinessPenaltyWeight cannot be negative.");
    }
}