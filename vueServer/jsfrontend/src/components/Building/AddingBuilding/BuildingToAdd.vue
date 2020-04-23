<template>
  <div class="BuildingToAdd" :key="update">
    
  <div v-show="!addFloor">
      <input v-model="buildingToAdd.buildingName" placeholder="Building Name">
      <button v-on:click="addaFloor()">Add A Floor</button>
      <div v-for="floor in buildingToAdd.floors" v-bind:key="floor.floorName">
      <Floor
        v-if="floor.floorName != ''"
        :Floor='floor'
        :CSBLocation='CSBLocation'
        :CSCLocation='CSCLocation'
        :country='country'
        :states='states'
      ></Floor>
      </div>
  </div>
  
  <div v-show="addFloor">
      <AddFLoor
        @FloorAdded='submitFloor'
        :CSCLocation='CSCLocation'
        :organization='organization'
        :country='country'
        :states='states'
      ></AddFLoor>
  </div>
  <button v-on:click="submit()">Submit</button>
  </div>
</template>

<script >
import AddFLoor from './AddFloor'
import Floor from '../Floor'

import axios from 'axios';

export default {
  name: 'BuildingToAdd',
  components:{
    AddFLoor,
    Floor
  },
  props:{
      CSCLocation: String,
      CSBLocation: String,
      organization: {
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
      country: Array,
      states: Array
  },
  data(){
      return{
      countrySelected: "",
      stateSelected:"",
      isDisinfectant: false,
      needsVenta: false,
      
      buildingToAdd:{
            timeCreated: "",
            iD: "",
            isDeleted: "",
            organizationId: "",
            buildingName: "",
            ammountOfFloors: "",
            floors: [{
            }],
            buildingInventory: {
                chemicals: [{
                    chemical: "",
                    chemicalSize: "",
                    chemicalRemaining: "",
                    needsReplaced: false
                }]
                }
            },
      update: 0,
      CSCinstance: axios.create({
        baseURL: this.CSCLocation,
        timeout: 10000,
        headers: {
          "Content-Type": "text/plain"
        }
      }),
      CSBinstance: axios.create({
        baseURL: this.CSBLocation,
        timeout: 10000,
        headers: {
          "Content-Type": "text/plain"
        }
      }),
      errMessage: "",
      addFloor: false
    }
  },
  methods:{
    submitFloor(floor){
      this.buildingToAdd.floors.push(floor)
      this.addaFloor()
      this.update++
    },
    addaFloor(){
      this.addFloor = !this.addFloor
    },
    submit(){
      this.buildingToAdd.timeCreated = "" + new Date().getTime()
      this.buildingToAdd.isDeleted = "false";
      this.buildingToAdd.organizationId = this.organization.iD
      this.buildingToAdd.ammountOfFloors = this.buildingToAdd.floors.length
      this.buildingToAdd.buildingInventory = {
        chemicals: []
      }
      for(let i = 0; i < this.buildingToAdd.floors.length; i++){
        for(let x = 0; x < this.buildingToAdd.floors[i].floorInventory.chemicals.length; x++){
          if(this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced == undefined || 
          this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced == ""){
            
          this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced = false
          } else {
            if(this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced == true){
              
          this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced = true
            } else {
              
          this.buildingToAdd.floors[i].floorInventory.chemicals[x].needsReplaced = false
            }
          }
          this.buildingToAdd.buildingInventory.chemicals.push(this.buildingToAdd.floors[i].floorInventory.chemicals[x])
        }
      }
      this.CSBinstance.post("", this.buildingToAdd)
        .then((response) => {
          this.errMessage = response
        })
        .catch((err) => {
          this.errMessage = err

        })
        this.$emit('BuildingAdded')
    },
    confirmCountry(){
      if(this.countrySelected == this.country[0]){
        return "1"
      }
    },
    confirmState(){
      if(this.stateSelected == this.states[0]){
        return "1"
      }
    }
  },
  beforeMount(){
    this.buildingToAdd = {
            timeCreated: "",
            iD: "",
            isDeleted: false,
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
            }
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