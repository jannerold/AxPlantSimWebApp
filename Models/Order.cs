using System.ComponentModel.DataAnnotations;

namespace AxPlantSimWebApp.Models
{
  public class Order
  {
    [Key]
    public int Id { get; set; }              // PK v DB

    // Základní identifikace
    public string OrderNumber { get; set; } = "";   // Zakázka (číslo)
    public int WorkCenterGroup { get; set; }        // Skupina (např. 1)
    public string ProductCode { get; set; } = "";   // Výrobek (kód)
    public string ProductName { get; set; } = "";   // Název výrobku

    // Množství + priority
    public int Quantity { get; set; }               // Kusy
    public int Priority { get; set; }               // Priorita
    public int ManualPriority { get; set; }         // Ruční priorita

    // Materiál
    public string Material { get; set; } = "";      // Materiál (číslo)
    public decimal MaterialAmount { get; set; }     // Množství mat. (Kč)

    // Stavy a termíny
    public DateTime? DueDate { get; set; }          // Termín dokončení
    public bool HasInvalidData { get; set; }        // Chybná data (0/1)
    public string Status { get; set; } = "";        // Rozpracováno / Dokončeno / ...

    // Rozpracovanost operace
    public int? OperationNumber { get; set; }       // R. operace
    public double? TimeRatio { get; set; }          // R. poměr času
    public DateTime? ReportTime { get; set; }       // R. termín hlášení

    // TZ informace
    public bool ContainsTz { get; set; }            // Obsahuje TZ
    public string ProgramTz { get; set; } = "";     // Program TZ
    public DateTime? StartDate { get; set; }        // Termín zahájení (vjp)
    public int? BasketCountTz { get; set; }         // Počet košů TZ
  }
}
