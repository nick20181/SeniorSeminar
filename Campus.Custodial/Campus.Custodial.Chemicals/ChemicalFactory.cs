using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class ChemicalFactory : IChemicalFactory
    {
        private IDatabase DB { get; set; }

        public ChemicalFactory(IDatabase db)
        {
            this.DB = db;
        }

        public IChemical ReadChemical(string id)
        {
            return DB.ReadChemical(id);
        }

        public IChemical CreateChemical(string name)
        {
            Chemical chemical = new Chemical()
            {
                name = name,
                DB = DB
            };
            IChemical result = DB.CreateChemical(chemical);
            if (result == null)
            {
                return new Chemical()
                {
                    name = null,
                    deleted = true
                };
            }
            return result;
        }

        public List<IChemical> ReadAllChemicals()
        {
            return DB.ReadAllChemical();
        }

        public IDatabase getDB()
        {
            return DB;
        }
    }
}
