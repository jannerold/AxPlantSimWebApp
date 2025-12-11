using AxPlantSimWebApp.Models;
using AxPlantSimWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AxPlantSimWebApp.Controllers;

public class DatabaseController : Controller
{
  private readonly TableBrowserService _tables;

  public DatabaseController(TableBrowserService tables)
  {
    _tables = tables;
  }

  // /Database
  public IActionResult Index()
  {
    var tableNames = _tables.GetTableNames();
    return View(tableNames);
  }

  // /Database/Table?tableName=...
  public IActionResult Table(string tableName)
  {
    if (string.IsNullOrWhiteSpace(tableName))
      return RedirectToAction(nameof(Index));

    var model = _tables.GetTable(tableName);
    return View(model);
  }

  [HttpPost]
  public IActionResult UpdateCell([FromBody] DynamicUpdateModel model)
  {
    if (model == null || string.IsNullOrWhiteSpace(model.TableName))
      return BadRequest("Invalid data");

    try
    {
      _tables.UpdateRow(model.TableName, model.Row);
      return Ok();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  public IActionResult Graph(string tableName, string chartType, string xCol, string yCol)
  {
    if (string.IsNullOrWhiteSpace(tableName))
      return BadRequest("Missing table name");

    var model = _tables.GetTable(tableName, limit: 5000); // na graf klidně více dat

    ViewBag.ChartType = chartType;
    ViewBag.XCol = xCol;
    ViewBag.YCol = yCol;
    ViewBag.TableName = tableName;

    return View(model);
  }

}
