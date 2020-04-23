using Custodial.Services.Building.InventoryItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Services.Building
{
    public class Inventory
    {
        public ChemicalItems[] chemicals { get; set; }
        public Machines[] machines { get; set; }
        public Tool[] tools { get; set; }
    }
}
