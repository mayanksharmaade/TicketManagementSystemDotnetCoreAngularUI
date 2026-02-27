namespace TicketCycle.Core.Enums
{
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

    public enum UserRole
    {
        Manager = 1,
        Developer = 2,
        Tester = 3
    }
}
