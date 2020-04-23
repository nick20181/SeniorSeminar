<template>
  <div class="BuildingList" :key="showingBuilding">
    <div v-show="!showToAdd">
      <h2>Buildings   <button v-on:click="addBuilding()">Add Building</button></h2>
      <div v-for="building in buildingList" v-bind:key="building.iD"> 
        <button v-show="!showingBuilding" v-on:click="selectBuilding(building)">{{building.buildingName}}</button>
      </div>
      <div v-show="showingBuilding && selectedBuilding != undefined">
        <BuildingDetails
          :Building="selectedBuilding"
          :states='states'
          :countries='countries'
          :CSBLocation="CSBLocation"
          :CSCLocation="CSCLocation"
        ></BuildingDetails>
        <button v-on:click="back()">Back</button>
      </div>
    </div>
      <div v-show="showToAdd">
        <BuildingToAdd
          @BuildingAdded='addBuilding'
          :CSCLocation='CSCLocation'
          :CSBLocation='CSBLocation'
          :organization='Organization'
          :states='states'
          :country='countries'
        >
        </BuildingToAdd>
      </div>
  </div>
</template>

<script >
import BuildingDetails from './BuildingDetails'
import BuildingToAdd from './AddingBuilding/BuildingToAdd'

import axios from 'axios';

export default {
  name: 'BuildingList',
  components:{
    BuildingDetails,
    BuildingToAdd
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
      CSBLocation: String,
      CSCLocation: String,
      countries: Array,
      states: Array
  },
  data(){
      return{
        buildingList: [],
        update: 0,
        showingBuilding: false,
        selectedBuilding: {
            timeCreated: "",
            iD: "",
            isDeleted: "",
            organizationId: "",
            buildingName: "",
            ammountOfFloors: "",
            floors: [{
                floorName: "",
                floorInventory: {
                    chemicals: [{
                        chemical: "",
                        chemicalSize: "",
                        chemicalRemaining: "",
                        needsReplaced: ""
                    }],
                    equipment:[{
                        name: "",
                        discription: ""
                    }],
                }
            }],
            buildingInventory: {
                chemicals: [{
                    chemical: "",
                    chemicalSize: "",
                    chemicalRemaining: "",
                    needsReplaced: ""
                }],
                equipment:[{
                    name: "",
                    discription: ""
                }],
                }
            },
        showToAdd: false
      }
  },
  methods:{
    selectBuilding(build){
      this.selectedBuilding = build
      this.showingBuilding = true
      this.$emit('toggleReturnToOrg')
    },
    back(){
      this.selectedBuilding = {
            timeCreated: "",
            iD: "",
            isDeleted: "",
            organizationId: "",
            buildingName: "",
            ammountOfFloors: "",
            floors: [{
                floorName: "",
                floorInventory: {
                    chemicals: [{
                        chemical: "",
                        chemicalSize: "",
                        chemicalRemaining: "",
                        needsReplaced: "false"
                    }],
                    equipment:[{
                        name: "",
                        discription: ""
                    }],
                }
            }],
            buildingInventory: {
                chemicals: [{
                    chemical: "",
                    chemicalSize: "",
                    chemicalRemaining: "",
                    needsReplaced: ""
                }],
                equipment:[{
                    name: "",
                    discription: ""
                }],
            }
        }
      this.showingBuilding = false
      this.$emit('toggleReturnToOrg')
      this.update++
    },
    addBuilding(){
      this.showToAdd = !this.showToAdd
    }
  },
  beforeUpdate(){
    this.instance.get('/organizationId/'+ this.Organization.iD).then(res => {
      if(this.buildingList == res.data){
      this.buildingList = res.data;
      this.update++;
      }
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
  },
  mounted(){
      this.instance = axios.create({
      baseURL: this.CSBLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
      this.update++;
      this.instance.get('/organizationId/'+ this.Organization.iD).then(res => {
      this.buildingList = res.data;
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