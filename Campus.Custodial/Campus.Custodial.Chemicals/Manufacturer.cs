using Campus.Custodial.Chemicals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Campus.Custodial.Chemicals
{
    public class Manufacturer : IManufacturer
    {
        public string name;
        public string address;
        public string phoneNumber;

        public string GetAddress()
        {
            return address;
        }

        public string GetNam()
        {
            return name;
        }

        public string GetPhoneNumber()
        {
            return phoneNumber;
        }
    }
}
