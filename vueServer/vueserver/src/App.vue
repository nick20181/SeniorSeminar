<template>
  <div id="app">
    <Test v-bind:orgList="orgList" v-bind:apiHandler="apiHandler" v-bind:serviceDict="serviceDict"></Test>
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
  private orgList!: string;
  public serviceDictionary: ServiceDictionary = new ServiceDictionary();
  public apiHandler: APIHandler = new APIHandler(this.serviceDictionary);
  
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
