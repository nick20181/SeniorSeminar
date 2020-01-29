using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker_API_Test.Resource
{
    public class InMemoryDB
    {
        public Dictionary<String, String> DB = new Dictionary<string, string>();

        public String Get(String id)
        {
            return DB.GetValueOrDefault(id);
        }

        public String Post(String id, String Val)
        {
            DB.Add(id, Val);
            return DB.GetValueOrDefault(id);
        }

        public String Delete(String id)
        {
            var toReturn = DB.GetValueOrDefault(id);
            DB.Remove(id);
            return toReturn;
        }
    }
}
