<template>
  <div id="app">
    <p>Custodial Services</p>
    <OrganizationSelection 
            @back="forceRerender()"
            :CSOLocation='getServiceLocation(CSOService)'
            :CSCLocation='getServiceLocation(CSCService)'
            :CSBLocation='getServiceLocation(CSBService)'
            :key="testComponetKey"
            :states='states'
            :countries='countries'
            ></OrganizationSelection>
  </div>
</template>

<script>
//Componets
import OrganizationSelection from './components/OrganizationSelection.vue'
//Modules
import axios from 'axios';

export default {
  name: 'App',
  components:{
    OrganizationSelection
  },
  data(){
    return {
      instance: axios.create({
        baseURL: 'http://192.168.0.149:5000/Addressing',
        timeout: 10000,
        headers: {"Content-Type": "application/json"}
      }),
      CSOService: {
        timeCreated:"",
        iD:"",
        isDeleted:false,
        serviceName:"",
        settings:{
          networkSettings:{
            addresses:[],
            ports:[]
          },
          databaseSettings:{
            address:"",
            port:"",
            collectionNames:[],
            databaseNames:[]
          }
        },
        discription:"",
        shortName:""
      },
      CSCService: {
        timeCreated:"",
        iD:"",
        isDeleted:false,
        serviceName:"",
        settings:{
          networkSettings:{
            addresses:[],
            ports:[]
          },
          databaseSettings:{
            address:"",
            port:"",
            collectionNames:[],
            databaseNames:[]
          }
        },
        discription:"",
        shortName:""
      },
      CSBService:{
        timeCreated:"",
        iD:"",
        isDeleted:false,
        serviceName:"",
        settings:{
          networkSettings:{
            addresses:[],
            ports:[]
          },
          databaseSettings:{
            address:"",
            port:"",
            collectionNames:[],
            databaseNames:[]
          }
        },
        discription:"",
        shortName:""
      },
      msg: "Default",
      testComponetKey: 0,
      countries:["United States of America"],
      states: ["Illionis"]

    }
  },
  methods: {
    forceRerender(){
      this.testComponetKey += 1;
    },
    getServiceLocation(service){
      let location = "http://" + service.settings.networkSettings.addresses[2] + ":" + service.settings.networkSettings.ports[0];
      if(service.serviceName == "Custodial.Service.Organization"){
          location = location + "/Organization"
        } else if(service.serviceName == "Custodial.Service.Chemical"){
          location = location + "/chemical"
        } else if(service.serviceName == "Custodial.Service.Building"){
          location = location + "/building"
        }
      return location
    }
  },
  mounted(){
    this.instance.get('/all').then(res => {
      const serviceList = res.data;
      serviceList.forEach(service => {
        if(service.serviceName == "Custodial.Service.Organization"){
          this.CSOService = service
        } else if(service.serviceName == "Custodial.Service.Chemical"){
          this.CSCService = service
        } else if(service.serviceName == "Custodial.Service.Building"){
          this.CSBService = service
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
