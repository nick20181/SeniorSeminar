<template>
  <div class="InventoryChemicalData">
    {{chemicalNeedsReplaced()}}
    {{getChemicalData(ChemicalData.chemical)}}
        <span><p>The inventory has {{ChemicalData.chemicalRemaining}} out of a {{ChemicalData.chemicalSize}} container of 
        <button v-on:click="showChemicalButton()">{{Chemical.chemicalName}}</button></p>
        </span>
          <ChemicalDetail 
          :Chemical="Chemical"
          :states='states'
          :countries='countries'
          v-show='showChemical'
          ></ChemicalDetail> 
    </div>
</template>

<script >
import ChemicalDetail from '../Chemical/ChemicalDetails'

import axios from 'axios';
export default {
  name: 'InventoryChemicalData',
  components:{
      ChemicalDetail
  },
  props:{
      ChemicalData: {
        chemical: "",
        chemicalSize: "",
        chemicalRemaining: "",
        needsReplaced: ""
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
        showChemical: false
      }
  },
  methods:{
      showChemicalButton(){
          this.showChemical = !this.showChemical
          this.update++
      },
      chemicalNeedsReplaced(){
          this.$emit("InventoryNeedsStocked")
      },
      getChemicalData(){
        if(this.ChemicalData != this.Chemical.iD){
        this.instance = axios.create({
            baseURL: this.CSCLocation,
            timeout: 10000,
            headers: {"Content-Type": "application/json"}
        });
        this.instance.get('/_id/'+ this.ChemicalData.chemical).then(res => {
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