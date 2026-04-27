namespace HouseholdTasksAPI.DataModels
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}
