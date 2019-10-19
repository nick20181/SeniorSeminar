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

        public IChemical CreateChemical(string id, string name)
        {
            Chemical chemical = new Chemical()
            {
                id = id,
                name = name,
                DB = DB
            };
            DB.CreateChemical(chemical);
            return chemical;
        }
    }
}
