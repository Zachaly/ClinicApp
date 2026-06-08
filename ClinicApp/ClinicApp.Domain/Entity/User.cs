using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Domain.Entity;

public class ApplicationUser : IUser
{
    public Guid Id { get; set ; }
}
