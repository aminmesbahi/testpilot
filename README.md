# Test Pilot
This is a from-scratch optimization playground for risk-based test selection.

Given a test suite with execution time, feature coverage, historical failure rate, business risk, and environment constraints, TestPilot tries to select the best subset of tests for a limited execution window.

The goal is to compare classic heuristic and metaheuristic algorithms such as Genetic Algorithm, Tabu Search, and Simulated Annealing on the same practical software engineering problem.

I'm trying to code again these algorithms without libaries, similar to 20 years ago which I'd had code them in old-school way!


---

Given:
- A set of test cases
- Each test has duration, risk coverage, feature area, failure history, stability, and dependencies
- A maximum execution time budget

Find:
- The best subset of tests to run

Optimize for:
- Maximum risk coverage
- Maximum feature diversity
- Higher priority for recently failed or flaky areas
- Minimum execution time
- Dependency correctness
- Environment constraints