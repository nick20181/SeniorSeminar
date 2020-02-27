export class Address{
    public country: Country;
    public state: State;
    public city: String;
    public zip: number;
    public street: string;
    public mainAddress: boolean;
    constructor(country: Country, state: State, city: string, zip: number, street: string, mainAddress: boolean){
        this.city = city;
        this.country = country;
        this.mainAddress = mainAddress;
        this.state = state;
        this.street = street;
        this.zip = zip;
    }
}

enum Country{
    United_States_of_America
}
enum State{
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