import { Address } from '../Utilities/Address';
import { ContactDetails } from '../Utilities/ContactDetails';

export class Organization {
    public timeCreated: number;
    public id: string;
    public isDeleted: boolean;
    public activeService: boolean;
    public organizationName: string;
    public organizationLocations: Address[];
    public contactDetails: ContactDetails[];
    public employeeCount: number;


    constructor(timeCreated: number, id: string, isDeleted: boolean, activeService: boolean,
        organizationName: string, organizationLocations: Address[], contactDetails: ContactDetails[],
        employeeCount: number){
            this.timeCreated = timeCreated;
            this.id = id;
            this.isDeleted = isDeleted;
            this.activeService = activeService;
            this.organizationLocations = organizationLocations;
            this.organizationName = organizationName;
            this.contactDetails = contactDetails;
            this.employeeCount = employeeCount;
    }
}