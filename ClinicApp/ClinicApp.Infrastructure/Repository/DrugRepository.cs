using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Infrastructure.Repository;

public class DrugRepository : RepositoryBase<Drug, GetDrugRequest>, IDrugRepository
{
    public DrugRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
