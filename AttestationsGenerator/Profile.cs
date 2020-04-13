using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttestationsGenerator
{
    public class Profile
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

        public override bool Equals(object obj)
        {
            return obj is Profile profile &&
                   Fullname == profile.Fullname;
        }

        public override int GetHashCode()
        {
            int hashCode = 1578132623;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Fullname);
            return hashCode;
        }
    }
}
