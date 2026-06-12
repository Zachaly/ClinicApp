using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Domain.Request.Update;

public interface IUpdateRequest
{
    public Guid Id { get; set; }
}
