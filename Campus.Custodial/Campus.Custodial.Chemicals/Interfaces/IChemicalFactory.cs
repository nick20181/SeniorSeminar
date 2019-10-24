using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals
{
    public interface IChemicalFactory
    {
        public Task<List<IChemical>> ReadChemicalAsync(string name);
        public Task<List<IChemical>> ReadAllChemicalsAsync();
        public Task<IChemical> CreateChemicalAsync(string chemName, Manufacturer manufacturer, string productIdentifier,
            List<signalWords> sigWords, List<string> hazardStatements, List<string> precautionStatements);
        public IDatabase getDB();
    }
}