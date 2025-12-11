using System.ComponentModel.DataAnnotations;

namespace AxPlantSimWebApp.Models
{
  public class Workplace
  {
    [Key]
    public int Id { get; set; }     // PK generovaný automaticky EF

    public string WorkplaceId { get; set; } = "";      // S10201
    public string Group { get; set; } = "";            // P102
    public string Description { get; set; } = "";      // Popis pracoviště

    public string Week1 { get; set; } = "";
    public string Week2 { get; set; } = "";
    public string Week3 { get; set; } = "";
    public string Week4 { get; set; } = "";

    public bool IsActive { get; set; }

    public string WorkplaceList { get; set; } = "";    // Seznam pracovišť
    public string WorkerList { get; set; } = "";       // Seznam pracovníků
  }
}
