<template>
  <div class="BuildingToAdd" :key="update">
      <input v-show="!showAddChem" v-model="floor.floorName" placeholder="Floor Name">
  <div>
      <button v-show="!showAddChem" v-on:click="showAddChemical()">Add Chemical to floor inventory.</button>
      <div v-show="showAddChem">
              Add 
          <select v-model="chemicalToAdd.chemical" class="form-control">
            <option v-for="chem in ChmicalList" :value="chem.iD" v-bind:key="chem.iD">{{chem.chemicalName}}</option>
          </select>
            with a container size of <input v-model="chemicalToAdd.chemicalSize" placeholder="Size of container">
            with <input v-model="chemicalToAdd.chemicalRemaining" placeholder="Remaining of container"> remaining, and
            <select v-model="chemicalToAdd.needsReplaced">
              <option :value="true">does need replaced</option>
              <option :value="false">does not need replaced</option>
            </select>
            <div>
        <button  v-on:click="submitChemical()">Submit Chemical</button>
        </div>
      </div>
      <div v-show="!showAddChem">
          <Inventory
            :Inventory='floor.floorInventory'
            :CSCLocation='CSCLocation'
            :countries='country'
            :states='states'
          ></Inventory>
      </div>
      <button v-show="!showAddChem" v-on:click="submit()">Submit Floor</button>
  </div>
  </div>
</template>

<script >
import Inventory from '../Inventory'

import axios from 'axios';

export default {
  name: 'BuildingToAdd',
  components:{
      Inventory
  },
  props:{
      CSCLocation: String,
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
        floor: {
            floorName: "",
            floorInventory: {
                chemicals: [{
                    chemical: "",
                    chemicalSize: "",
                    chemicalRemaining: "",
                    needsReplaced: ""
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
      errMessage: "",
      chemicals: [],
      chemicalToAdd: {
        chemical: "",
        chemicalSize: "",
        chemicalRemaining: "",
        needsReplaced: ""
      },
      showAddChem: false,
      ChmicalList: []
    }
  },
  methods:{
    submit(){
        this.$emit('FloorAdded', this.floor)
    },
    showAddChemical(){
        this.showAddChem = !this.showAddChem
    },
    submitChemical(){
        this.chemicals.push(this.chemicalToAdd)
        this.floor.floorInventory = {
            chemicals: this.chemicals
        }
        this.showAddChemical()
    }
  },
  beforeMount(){
    this.floor= {
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
        }
  },
  mounted(){
      this.CSCinstance = axios.create({
      baseURL: this.CSCLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
      this.update++;
      this.CSCinstance.get('/organizationId/'+ this.organization.iD).then(res => {
      this.ChmicalList = res.data;
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