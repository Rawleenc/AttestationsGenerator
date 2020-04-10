using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttestationsGenerator
{
    class Profile
    {
        public string Fullname { get; set; }
        public string BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public Profile(string fullname, string birthDate, string birthPlace, string address, string city)
        {
            this.Fullname = fullname;
            this.BirthDate = birthDate;
            this.BirthPlace = birthPlace;
            this.Address = address;
            this.City = city;
        }
    }
}
