using HouseholdTasksAPI.DataModels.Enum;

namespace HouseholdTasksAPI.DataModels
{
    public class MealSuggestion
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string? Text { get; set; }
        public Weekday? weekday{ get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActiv { get; set; }
    }
}
