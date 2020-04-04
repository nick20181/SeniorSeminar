<template>
  <div class="ChemicalList" :key="showingChemical">
    <div v-show="!showToAdd">
      <h2>Chemicals   <button v-on:click="addChemical()">Add Chemical</button></h2>
      <div v-for="chemical in chemicalList" v-bind:key="chemical.iD"> 
        <button v-show="!showingChemical" v-on:click="selectChemical(chemical)">{{chemical.chemicalName}}</button>
      </div>
      <div v-show="showingChemical && selectedChemical != undefined">
        <ChemicalDetail 
          :Chemical="selectedChemical"
          :states='states'
          :countries='countries'
          ></ChemicalDetail>
        <button v-on:click="back()">Back</button>
      </div>
    </div>
      <div v-show="showToAdd">
        <ChemicalToAdd
          :states='states'
          :country='countries'
          ></ChemicalToAdd>
      </div>
  </div>
</template>

<script >
import ChemicalDetail from './ChemicalDetails'
import ChemicalToAdd from './ChemicalToAdd'

import axios from 'axios';

export default {
  name: 'ChemicalList',
  components:{
    ChemicalDetail,
    ChemicalToAdd
  },
  props:{
      Organization: undefined,
      CSCLocation: String,
      countries: undefined,
      states: undefined
  },
  data(){
      return{
        chemicalList: [],
        update: 0,
        showingChemical: false,
        selectedChemical: undefined,
        showToAdd: false
      }
  },
  methods:{
    selectChemical(chem){
      this.selectedChemical = chem
      this.showingChemical = true
      this.$emit('toggleReturnToOrg')
    },
    back(){
      this.selectedChemical = undefined
      this.showingChemical = false
      this.$emit('toggleReturnToOrg')
    },
    addChemical(){
      this.showToAdd = !this.showToAdd
    }
  },
  mounted(){
      this.instance = axios.create({
      baseURL: this.CSCLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
      this.update++;
      this.instance.get('/organizationId/'+ this.Organization.iD).then(res => {
      this.chemicalList = res.data;
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