using System;

namespace Authentication_Authorization.Models.Entities.Interfaces
{
    public enum Status { Active = 1, Modified, Passive}
    public interface IBaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Status Status { get; set; }

    }
}
