using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.Services.Utility
{
    public class Address
    {
        public Country country { get; set; } = Country.United_States_of_America;
        public State state { get; set; }
        public string city { get; set; }
        public int zip { get; set; }
        public string street { get; set; }
        public bool mainAddress { get; set; }
    }

    public enum Country
    {
        United_States_of_America
    }
    public enum State
    {
        Alabama,
        Alaska,
        Arizona,
        Arkansas,
        California,
        Colorado,
        Connecticut,
        Delaware,
        Florida,
        Georgia,
        Hawaii,
        Idaho,
        Illinois,
        Indiana,
        Iowa,
        Kansas,
        Kentucky,
        Louisiana,
        Maine,
        Maryland,
        Massachusetts,
        Michigan,
        Minnesota,
        Mississippi,
        Missouri,
        MontanaNebraska,
        Nevada,
        New_Hampshire,
        New_Jersey,
        New_Mexico,
        New_York,
        North_Carolina,
        North_Dakota,
        Ohio,
        Oklahoma,
        Oregon,
        Pennsylvania,
        Rhode_Island,
        South_Carolina,
        South_Dakota,
        Tennessee,
        Texas,
        Utah,
        Vermont,
        Virginia,
        Washington,
        West_Virginia,
        Wisconsin,
        Wyoming
    }
}
