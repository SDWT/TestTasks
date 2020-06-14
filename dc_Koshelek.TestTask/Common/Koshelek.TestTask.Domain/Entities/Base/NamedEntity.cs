using Koshelek.TestTask.Domain.Entities.Base.Interfaces;

namespace Koshelek.TestTask.Domain.Entities.Base
{
    public abstract class NamedEntity : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
    }
}
