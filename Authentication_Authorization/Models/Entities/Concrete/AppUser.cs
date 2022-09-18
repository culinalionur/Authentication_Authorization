using Authentication_Authorization.Models.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;

namespace Authentication_Authorization.Models.Entities.Concrete
{
    public class AppUser : IdentityUser, IBaseEntity
    {
        private DateTime _createDate = DateTime.Now;
        public DateTime CreateDate
        { 
            get => _createDate;
            set => _createDate = value;
        }
        public string Occupation { get; set; }

        public DateTime? DeleteDate{ get; set; }
        public DateTime? UpdateDate 
        {
            get ;
            set ; 
        }
        private Status _status = Status.Active;
        public Status Status 
        {
            get => _status; 
            set => _status = value; 
        }
    }
}
