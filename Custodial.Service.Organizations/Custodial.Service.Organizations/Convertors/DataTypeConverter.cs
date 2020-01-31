using Custodial.BoilerPlate.Core.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Organizations.Convertors
{
    public class DataTypeConverter : IDataTypeConverter<Organization>
    {
        public Organization getTypeData(string dataType, string data)
        {
            switch (dataType)
            {
                case "timeCreated":
                    return new Organization()
                    {
                        timeCreated = Convert.ToInt64(data)
                    };
                case "iD":
                    return new Organization()
                    {
                        iD = data
                    };
                case "isDeleted":
                    return new Organization()
                    {
                        isDeleted = Convert.ToBoolean(data)
                    };
                case "activeService":
                    return new Organization()
                    {
                        activeService = Convert.ToBoolean(data)
                    };
                case "organizationName":
                    return new Organization()
                    {
                        organizationName = data
                    };
                case "organizationAddress":
                    return new Organization()
                    {
                        organizationAddress = data
                    };
                case "phoneNumber":
                    return new Organization()
                    {
                        phoneNumber = data
                    };
            }
            throw new DataTypeNotFoundException($"Could not find data type: {dataType} with data: {data}");
        }
    }

    public class DataTypeNotFoundException: Exception
    {
        public DataTypeNotFoundException() {
        }

        public DataTypeNotFoundException(string message) : base (message)
        {
        }
    }
}
