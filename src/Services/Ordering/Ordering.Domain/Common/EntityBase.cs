namespace Ordering.Domain.Common;

public enum LastModifiedStatusEnum
{
    Deleted,
    Created,
    Updated,
}

public abstract class EntityBase
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public LastModifiedStatusEnum LastModifiedStatus { get; set; }
}
