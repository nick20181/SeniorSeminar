<template>
  <div v-show="Floor.floorName != ''" class="Floor">
      <p><button v-on:click="showDataButton()">Floor: {{Floor.floorName}}</button></p>
      <Inventory
        v-show='showData'
        :CSBLocation='CSBLocation'
        :CSCLocation='CSCLocation'
        :Inventory='Floor.floorInventory'
        :countries='countries'
        :states='states'
      ></Inventory>
      <p v-show="showData"></p>
  </div>
</template>

<script >
import Inventory from "./Inventory"

export default {
  name: 'Floor',
  components:{
      Inventory
  },
  props:{
      Floor: {
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
            },
      countries: Array,
      states: Array,
      CSBLocation: String,
      CSCLocation:String,
  },
  data(){
      return{
          showData: false
      }
  },
  methods:{
      showDataButton(){
          this.showData = !this.showData
      },
      country(country){
        return this.countries[country-1]
      },
      state(state){
        return this.states[state-1]
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