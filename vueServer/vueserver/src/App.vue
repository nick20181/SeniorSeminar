<template>
  <div id="app">
    <Test :key="testComponetKey" v-bind:message="spiMessage" :from="spiMessage"></Test>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import Test from './components/Test.vue';
import { APIHandler } from './API Handler';
import { ServiceDictionary } from './ServiceDictionary';
import { Microservice } from './Custodial.Addressing.Service/Microservice';
import { Organization } from './Custodial.Service.Organization/Organization';


@Component({
  components: {
    Test,
  },
})
export default class App extends Vue {
  public serviceDictionary: ServiceDictionary = new ServiceDictionary();
  public apiHandler: APIHandler = new APIHandler(this.serviceDictionary);
  private spiMessage: string = "Hello";
  private testComponetKey: number = 0;

  
  mounted(){
    this.forceRerender();
  }

  
  beforeUpdate(){
    this.spiMessage = "World";
  }

  async forceRerender(){
    await new Promise( resolve => setTimeout(resolve, 5000) );
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
