using Campus.Custodial.Chemicals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals
{
    public interface IChemical
    {
        public string ToString();
        public Task<List<IChemical>> UpdateChemicalAsync(IChemical updatedChemical);
        public Task<List<IChemical>> DeleteChemicalAsync();
        public string ToJson();
        public string GetName();
        public bool GetDeletedStatus();

        public string GetID();
        public void SetID(string id);
        public Manufacturer GetManufacturer();
        public string GetProductIdentifier();
        public List<string> GetHazardStatements();
        public List<string> GetPrecautionStatements();
        public List<signalWords> GetSignalWords();
        public abstract IChemical NullChemical();
    }
}
