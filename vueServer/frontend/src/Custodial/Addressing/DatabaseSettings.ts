export class DatabaseSettings {
    public connectionString: string;
    public database: string;
    public collection: string;

    constructor(connectionString: string, database: string, collection: string){
        this.connectionString = connectionString;
        this.database = database;
        this.collection = collection;
    }
}   