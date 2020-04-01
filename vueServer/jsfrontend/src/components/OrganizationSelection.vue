<template>
  <div class="orgSelect" v-bind:key="update">
    <div v-if="!orgSelectedBool" :key="update">
        <p>Please Choose a organization</p>
        <div 
        v-for="Org in orgList" 
        v-bind:key="Org.id">
            <Organization 
            @clicked="orgSelected"
            :Organization="Org" 
            :key="Org.id"
            ></Organization>
        </div>
    </div>
    <div v-show="orgSelectedBool" :key="update">
        <p :key="orgSelected">Organization Selected: {{orgSelected.organizationName}}</p>
         <ChemicalView></ChemicalView>
        <button v-on:click="back()">Back</button>
    </div>
  </div>
</template>

<script >
//Componet
import Organization from "./Organization"
import axios from 'axios';

export default {
  name: 'organizationSelection',
  components:{
      Organization
  },
  props:{
      CSOLocation: String
  },
  data(){
      return{
        instance: axios.create({
          baseURL: this.CSOLocation,
          timeout: 10000,
          headers: {"Content-Type": "application/json"}
        }),
        orgList: [],
        errMsg: "No Error Yet",
        update: 0,
      }
  },
  methods:{
    orgSelected(Organization){
        this.orgSelectedBool = true
        this.orgSelected = Organization
        this.update++
    },
    refresh(){
      this.update++;
    },
    back(){
          this.$emit('back')
    }
  },
  beforeUpdate(){
    this.instance.get('/all').then(res => {
      this.orgList = res.data;
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
  },
  mounted(){
      this.instance = axios.create({
      baseURL: this.CSOLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
      this.update++;
      this.instance.get('/all').then(res => {
      this.orgList = res.data;
      this.update++;
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>