<template>
 <div class="orgSelect" :key="update">
    <div v-show="!orgSelectedBool">
        <button v-on:click="refresh()">Refresh</button>
        <p>Please Choose a organization</p>
        <ul id="orgList" class="demo" :key="CSOLocation">
            <li v-for="organization in orgList" :key="organization">
                <button v-on:click="orgSelect(organization)">{{ organization.organizationName }}</button>
            </li>
        </ul>
    </div>
    <div v-show="orgSelectedBool" :key="update">
        <p :key="orgSelected">Organization Selected: {{orgSelectedName}}</p>
         <ChemicalView></ChemicalView>
        <button v-on:click="back()">Back</button>
    </div>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { Organization } from '../Custodial/Organization/Organization';
import axios from 'axios';

export default new Vue({
  name: 'Organization',
  components:{
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
        orgList: [Organization.Null],
        errMsg: "No Error Yet",
        update: 0,
      }
  },
  methods:{
    refresh(){
      this.update++;
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
})
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