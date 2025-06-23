using Application.DTOs.Tasks;

namespace Application.DTOs.Shared;

public class CsvExportRequest
{
    public TaskFilterDto? Filter { get; set; }
}