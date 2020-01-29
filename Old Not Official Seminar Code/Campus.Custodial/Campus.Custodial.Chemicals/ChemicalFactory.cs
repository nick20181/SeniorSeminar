using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals
{
    public class ChemicalFactory : IChemicalFactory
    {
        private IDatabase DB { get; set; }

        public ChemicalFactory(IDatabase db)
        {
            this.DB = db;
        }

        public async Task<IChemical> ReadChemicalAsync(IChemical chem)
        {
            var temp = await DB.ReadChemicalAsync(chem.GetName());
            foreach(var x in temp)
            {
                if (x.GetName().Equals(chem.GetName()))
                {
                    return x;
                }
            }
            return new Chemical().NullChemical();
        }

        public async Task<IChemical> CreateChemicalAsync(IChemical chem)
        {
            var x = await DB.CreateChemicalAsync(chem);
            foreach(var y in x)
            {
                if (y.GetName().Equals(chem.GetName()))
                {
                    return y;
                }
            }
            return new Chemical().NullChemical();
        }

        public async Task<List<IChemical>> ReadAllChemicalsAsync()
        {
            return await DB.ReadAllChemicalAsync();
        }

        public IDatabase getDB()
        {
            return DB;
        }
    }
}
