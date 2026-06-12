using ClinicApp.Domain.Attribute;

namespace ClinicApp.Domain.Request.Get;

public class PagedRequest
{
    [RequestField(Skip = true)]
    public int? Index { get; set; }
    [RequestField(Skip = true)]
    public int? PageSize { get; set; }
    [RequestField(Skip = true)]
    public bool? SkipPagination { get; set; }
}
