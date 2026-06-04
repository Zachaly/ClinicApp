namespace ClinicApp.Domain.Entity;

public interface ISoftDelete
{
    public DateTimeOffset? DeletedOn { get; set; }
}
