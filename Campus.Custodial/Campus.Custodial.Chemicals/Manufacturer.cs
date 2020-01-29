using Campus.Custodial.Chemicals.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Campus.Custodial.Chemicals
{
    public class Manufacturer : IManufacturer
    {

        [BsonElement("Manufacturer Name")]
        public string name;
        [BsonElement("Manufacturer Address")]
        public string address;
        [BsonElement("Manufacturer Phonenumber")]
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
