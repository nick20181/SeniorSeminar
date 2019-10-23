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

        public IChemical ReadChemical(string id)
        {
            return DB.ReadChemical(id);
        }

        public IChemical CreateChemical(string chemName, Manufacturer manufacturer, string productIdentifier,
            List<signalWords> sigWords, List<string> hazardStatements, List<string> precautionStatements)
        {
            Chemical chemical = new Chemical()
            {
                name = chemName,
                DB = DB,
                deleted = false,
                manufacturer = manufacturer,
                productIdentifier = productIdentifier,
                sigWords = sigWords,
                hazardStatements = hazardStatements,
                precautionStatements = precautionStatements

            };
            IChemical result = DB.CreateChemical(chemical);
            if (result == null)
            {
                return new Chemical().NullChemical();
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
