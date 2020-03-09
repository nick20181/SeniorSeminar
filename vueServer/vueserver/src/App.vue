<template>
  <div id="app">
    <test :key='testComponetKey' :message='msg'> </test>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Test from './components/Test.vue';
import { APIHandler } from './API Handler';
import { ServiceDictionary } from './ServiceDictionary';
import { Microservice } from './Custodial.Addressing.Service/Microservice';
import { Organization } from './Custodial.Service.Organization/Organization';
import { Address } from './Custodial.Service.Utility/Address';
import { ContactDetails } from './Custodial.Service.Utility/ContactDetails';
import axios, { AxiosInstance } from 'axios';
@Component({
  components: {
    Test,
  },
})
export default class App extends Vue {
  public serviceDictionary: ServiceDictionary = new ServiceDictionary();
  public apiHandler!: APIHandler;
  private spiMessage: string = "Hello";
  private testComponetKey: number = 0;
  private orgUpdated: boolean = false;
  private org!: Organization;
  private msg!: string;
  private instance!: AxiosInstance;

  beforeCreate() {
    this.instance = axios.create({
      baseURL: 'http://localhost:5000/Addressing',
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
    });
    this.apiHandler = new APIHandler(this.serviceDictionary);
  }

  created(){ 
  }
//https://www.telerik.com/fiddler
  beforeMount(){
  }

  mounted(){
    this.instance.get('/all').then(res => {
      this.msg = res.data;
      this.forceRerender();
    }).catch(e => { 
      this.msg = e+ ": " ;
      this.forceRerender();
    })
    //var loca: Address[] = [new Address(1,1,"city", 1111, "street", false)];
    //var contact: ContactDetails[] = [new ContactDetails("emal", "number")];
    //this.org = new Organization(0, "id", false, true, "orgName", loca, contact, 1);
    //var response!: string;
    //response = await this.apiHandler.getOrganizationList();
    //var orgList: Organization[] = JSON.parse(response)
    //this.org = orgList[0];
    //this.forceRerender();
    //this.orgUpdated = true;
  }

  async forceRerender(){
    this.testComponetKey += 1;
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
