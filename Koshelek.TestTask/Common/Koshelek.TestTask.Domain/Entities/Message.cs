using Koshelek.TestTask.Domain.Entities.Base;
using System;

namespace Koshelek.TestTask.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }

        public DateTime ServerDateTime { get; set; }

        public int Order { get; set; }
    }
}
