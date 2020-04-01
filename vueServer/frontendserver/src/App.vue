<template>
  <div id="app">
  </div>
</template>

<script lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import Microservice from './Custodial/Addressing/Microservice'
export default {
  name: 'App',
  components: {
    HelloWorld
  },
  mounted(){
    this.instance.get('/all').then(res => {
      var serviceList: Array<Microservice> = res.data;
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
