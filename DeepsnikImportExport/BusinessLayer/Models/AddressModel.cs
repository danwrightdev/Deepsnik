using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class AddressModel
    {
        public AddressModel() {}
        public AddressModel(string title,
                            string forename,
                            string surname,
                            string company,
                            string address1,
                            string address2,
                            string town,
                            string postcode,
                            string county)
        {
            Title = title;
            Forename = forename;
            Surname = surname;
            Company = company;
            Address1 = address1;
            Address2 = address2;
            Town = town;
            Postcode = postcode;
            County = county;
        }

        public string Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
    }
}
