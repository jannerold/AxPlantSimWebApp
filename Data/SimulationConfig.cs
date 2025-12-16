namespace AxPlantSimWebApp.Data
{
  public class SimulationConfig
  {
    public int Id { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? SimulationTime { get; set; }

    public int? DeadlineDays { get; set; }
    public int? ReplacementCalendarDays { get; set; }
    public int? MaterialLeadTimeHours { get; set; }

    public DateTime UpdatedAt { get; set; }
  }

}
