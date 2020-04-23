<template>
  <div class="BuildingDetail">
      <h2>{{Building.buildingName}}</h2>
        <p>This building has {{Building.ammountOfFloors}} floors, which are:</p>
        <div v-for="floor in Building.floors" v-bind:key='floor.floorName' >
            <Floor
            :Floor='floor'
            :countries='countries'
            :states='states'
            :CSBLocation='CSBLocation'
            :CSCLocation="CSCLocation"
            >
            </Floor>
        </div>
  </div>
</template>

<script >
import Floor from './Floor'

export default {
  name: 'BuildingDetail',
  components:{
      Floor
  },
  props:{
      Building: {
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
      countries: Array,
      states: Array,
      CSBLocation: String,
      CSCLocation: String
  },
  data(){
      return{
      }
  },
  methods:{
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