<template>
  <div class="Inventory">
    <p v-show="needsStocked"> This Inventory needs stocked</p>
    <div
        v-for="chemicals in Inventory.chemicals"
        v-bind:key='chemicals.chemical'
    >
        <InventoryChemicalData
            @InventoryNeedsStocked="chemicalNeedsReplaced"
            :ChemicalData='chemicals'
            :countries='countries'
            :states='states'
            :CSCLocation='CSCLocation'
        ></InventoryChemicalData>
    </div>
  </div>
</template>

<script >
import InventoryChemicalData from './InventoryChemicalData'

import axios from 'axios';
export default {
  name: 'Inventory',
  components:{
      InventoryChemicalData
  },
  props:{
      Inventory: {
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
                },
      CSCLocation: String,
      countries: Array,
      states: Array,
  },
  data(){
      return{
          errMsg: "",
          update: 0,
          Chemical:{
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
        showChemical: false,
        needsStocked: false
      }
  },
  methods:{
      showChemicalButton(){
          this.showChemical = !this.showChemical
          this.update++
      },
      chemicalNeedsReplaced(){
          this.needsStocked = true
      },
      getChemicalData(chemical){
        if(chemical != this.Chemical.iD){
        this.instance = axios.create({
            baseURL: this.CSCLocation,
            timeout: 10000,
            headers: {"Content-Type": "application/json"}
        });
        this.instance.get('/_id/'+ chemical).then(res => {
            let chemList = res.data
            this.Chemical = chemList[0]
        }).catch(e => { 
            this.errMsg = e+ ": " ;
        })
      }
        }
  },
  mounted(){
      this.Chemical= {
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