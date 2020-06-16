using Koshelek.TestTask.Domain.Entities.Base.Interfaces;

namespace Koshelek.TestTask.Domain.Entities.Base
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
