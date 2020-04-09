<template>
  <div class="orgSelect" :key="update">
    <div v-if="!orgSelectedBool">
        <p>Please Choose a organization</p>
        <div 
        v-for="Org in orgList" 
        v-bind:key="Org.Id">
            <Organization 
            @clicked="orgSelected"
            :Organization="Org"
            ></Organization>
        </div>
    </div>
    <div v-show="orgSelectedBool">
        <p>Organization Selected: {{orgSelected.organizationName}}</p>
          <ul v-show="!chemicalList && !buildingList">
            <li>
              <button v-on:click="chemicalListSleceted()">Chemicals</button>
            </li>
            <li>
              <button v-on:click="buildingListSleceted()">Buildings</button>
            </li>
          </ul>
        <ChemicalList 
            @toggleReturnToOrg="toggleReturnToOrg()" 
            v-show="chemicalList" :Organization="orgSelected" 
            :CSCLocation="CSCLocation"
            :states='states'
            :countries='countries'
            ></ChemicalList>
          <button v-show="showReturnToOrg"  v-on:click="returnFromChemicalOrBuilding()">Return to Organiztion</button>
      <div v-show="!chemicalList && !buildingList" >
        <button v-on:click="back()">Back</button>
      </div>
    </div>
  </div>
</template>

<script >
//Componet
import Organization from "./Organization"
import ChemicalList from "./ChemicalList"
import axios from 'axios';

export default {
  name: 'organizationSelection',
  components:{
      Organization,
      ChemicalList
  },
  props:{
      CSOLocation: String,
      CSCLocation: String,
      countries: Array,
      states: Array
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
        chemicalList: false,
        buildingList: false,
        showReturnToOrg: true,
        orgSelectedBool: false

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
    },
    chemicalListSleceted(){
      this.chemicalList = true
      this.buildingList = false
      this.showReturnToOrg = true
      this.update++
    },
    buildingListSleceted(){
      this.chemicalList = false
      this.buildingList = true
      this.showReturnToOrg = true
      this.update++
    },
    returnFromChemicalOrBuilding(){
      this.chemicalList = false
      this.buildingList = false
      this.update++
    },
    toggleReturnToOrg(){
      this.showReturnToOrg = !this.showReturnToOrg;
    }
  },
  beforeUpdate(){
    this.instance.get('/all').then(res => {
      if(this.orgList == res.data){
        this.orgList = res.data;
      }
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