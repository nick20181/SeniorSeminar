<template>
  <div id="app">
    <div v-if="orgUpdated === true">
      <Test :key="testComponetKey" v-bind:org="org"></Test>
    </div>
    <div v-else>
      <h1>Loading...</h1>
    </div>
    {{msg}}
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

  async created(){
    this.apiHandler = new APIHandler(this.serviceDictionary);
    this.msg = "Last: " + this.apiHandler.lastError;
  }

  async mounted(){
    //var loca: Address[] = [new Address(1,1,"city", 1111, "street", false)];
    //var contact: ContactDetails[] = [new ContactDetails("emal", "number")];
    //this.org = new Organization(0, "id", false, true, "orgName", loca, contact, 1);
    var response!: string;
    response = await this.apiHandler.getOrganizationList();
    var orgList: Organization[] = JSON.parse(response)
    this.org = orgList[0];
    this.forceRerender();
    this.orgUpdated = true;
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
