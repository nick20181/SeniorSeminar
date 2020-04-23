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
          @ChemicalAdded="addChemical()"
          :CSCLocation='CSCLocation'
          :organization='Organization'
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
      Organization: {
        timeCreated: "",
        iD: "",
        isDeleted: "",
        activeService: "",
        organizationName: "",
        organizationLocations:	[{
          country: "",
          state: "",
          city: "",
          zip: "",
          street: "",
          mainAddress: ""
        }],
        contactDetails:  {
          email: "",
          phoneNumber: ""
        },
        emplyeeCount: ""
      },
      CSCLocation: String,
      countries: Array,
      states: Array
  },
  data(){
      return{
        chemicalList: [],
        update: 0,
        showingChemical: false,
        selectedChemical: {
          timeCreated: "",
          iD: "",
          isDeleted: "",
          organizationId: "",
          chemicalName: "",
          chemicalManufactor: {
            manufactorName: "",
            manufactorAddress: {
              country: "",
              state: "",
              city: "",
              zip: "",
              street: "",
              mainAddress: ""
            }
          },
          saftyContactInformation: {
            email: "",
            phoneNumber: ""
          },
          chemicalWarning: "",
          disinfectant: "",
          ventilationNeeded: "",
          usesAndPrep: {
            chemicalPrepAmmount: "",
            waterPrepAmmount: ""
          }
        },
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
      this.selectedChemical = {
          timeCreated: "",
          iD: "",
          isDeleted: "",
          organizationId: "",
          chemicalName: "",
          chemicalManufactor: {
            manufactorName: "",
            manufactorAddress: {
              country: "",
              state: "",
              city: "",
              zip: "",
              street: "",
              mainAddress: ""
            }
          },
          saftyContactInformation: {
            email: "",
            phoneNumber: ""
          },
          chemicalWarning: "",
          disinfectant: "",
          ventilationNeeded: "",
          usesAndPrep: {
            chemicalPrepAmmount: "",
            waterPrepAmmount: ""
          }
        }
      this.showingChemical = false
      this.$emit('toggleReturnToOrg')
      this.update++
    },
    addChemical(){
      this.showToAdd = !this.showToAdd
    }
  },
  beforeUpdate(){
    this.instance.get('/organizationId/'+ this.Organization.iD).then(res => {
      if(this.chemicalList == res.data){
      this.chemicalList = res.data;
      this.update++;
      }
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
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