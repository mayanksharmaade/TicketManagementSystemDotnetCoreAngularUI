namespace TicketCycle.Core.Enums
{
    // ------ Ticket ------
    public enum TicketStatus
    {
        New = 1,
        Assigned = 2,
        WIP = 3,
        Resolved = 4,
        Testing = 5,
        Closed = 6,
        Rejected = 7
    }

    public enum TicketPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    // ----User --------
    public enum UserRole
    {
        Manager = 1,
        Developer = 2,
        Tester = 3
    }

    // ------ Project --------
    public enum ProjectStatus
    {
        Active = 1,
        OnHold = 2,
        Completed = 3,
        Cancelled = 4
    }

    // ---- Sprint -------
    public enum SprintStatus
    {
        Planning = 1,
        Active = 2,
        Completed = 3,
        Cancelled = 4
    }

    // --- Test Script -----
    public enum TestScriptStatus
    {
        Draft = 1,
        Active = 2,
        Passed = 3,
        Failed = 4
    }

    // ---- Test Step -----
    public enum TestStepStatus
    {
        NotRun = 1,
        Passed = 2,
        Failed = 3,
        Blocked = 4
    }
}
