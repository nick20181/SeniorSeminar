<template>
  <div id="app">
    <organizations :CSOLocation='getCSOLocation()' :key="testComponetKey"></organizations>
  </div>
</template>

<script lang="ts">
import axios from 'axios';
import Organizations from './components/Organizations.vue';
import {Microservice} from './Custodial/Addressing/Microservice';

export default {
  name: 'App',
  components:{
    Organizations
  },
  data(){
    return {
      instance: axios.create({
        baseURL: 'http://localhost:5000/Addressing',
        timeout: 10000,
        headers: {"Content-Type": "application/json"}
      }),
      CSOService: Microservice.NullService(),
      msg: "Default",
      testComponetKey: 0
    }
  },
  methods: {
    forceRerender(){
      this.testComponetKey += 1;
    },
    getCSOLocation(): string {
      return "http://" + this.CSOService.settings.networkSettings.addresses[1] + ":" + this.CSOService.settings.networkSettings.ports[0]
      + "/organization";
    }
  },
  mounted(){
    this.instance.get('/all').then(res => {
      const serviceList: Array<Microservice> = res.data;
      serviceList.forEach(service => {
        if(service.serviceName == "Custodial.Service.Organization"){
          this.CSOService = service
        }
      });
      this.forceRerender();
    }).catch(e => { 
      this.msg = e+ ": " ;
      this.forceRerender();
    })
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
