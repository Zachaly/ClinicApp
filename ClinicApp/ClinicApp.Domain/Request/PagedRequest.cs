using ClinicApp.Domain.Attribute;

namespace ClinicApp.Domain.Request;

public class PagedRequest
{
    [RequestField(Skip = true)]
    public int? Index { get; set; }
    [RequestField(Skip = true)]
    public int? PageSize { get; set; }
}
