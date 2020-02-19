using Custodial.Service.Chemical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Services.Building.InventoryItems
{
    public class ChemicalItems
    {
        public Chemical chemical { get; set; }
        public string chemicalSize { get; set; }
        public string chemicalVolume { get; set; }
        public string chemicalRemaining { get; set; }
        public bool needsReplaced { get; set; }
    }
}
