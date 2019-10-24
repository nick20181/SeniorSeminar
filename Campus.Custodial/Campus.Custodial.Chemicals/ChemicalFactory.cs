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

        public async Task<List<IChemical>> ReadChemicalAsync(string id)
        {
            return await DB.ReadChemicalAsync(id);
        }

        public async Task<IChemical> CreateChemicalAsync(string chemName, Manufacturer manufacturer, string productIdentifier,
            List<signalWords> sigWords, List<string> hazardStatements, List<string> precautionStatements)
        {
            Chemical chemical = new Chemical()
            {
                chemicalName = chemName,
                DB = DB,
                deleted = false,
                manufacturer = manufacturer,
                productIdentifier = productIdentifier,
                sigWords = sigWords,
                hazardStatements = hazardStatements,
                precautionStatements = precautionStatements

            };
            IChemical result = await DB.CreateChemicalAsync(chemical);
            if (result == null)
            {
                return new Chemical().NullChemical();
            }
            return result;
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
