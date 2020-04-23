<template>
  <div class="ChemicalToAdd" :key="update">
      <h3>Chemical Details</h3>
      <div>
      <input v-model="chemicalToAdd.chemicalName" placeholder="Chemical Name">
      </div>
      <input v-model="chemicalToAdd.chemicalWarning" placeholder="Chemical Warning">
          <p> Chemical 
            <select v-model="isDisinfectant">
              <option :value="true">Is a disinfectant</option>
              <option :value="false">Is not a disinfectant</option>
            </select>,
          </p>
          <p> Chemical 
            <select v-model="needsVenta">
              <option :value="true">needs ventalation</option>
              <option :value="false">does not needs ventalation</option>
            </select>,
          </p>

      <h3>Chemical Prep Details</h3>
      <div>
        <p>
          <span><input v-model="chemicalToAdd.usesAndPrep.chemicalPrepAmmount" placeholder="product ammount"> of product for every 
          <input v-model="chemicalToAdd.usesAndPrep.waterPrepAmmount" placeholder="water ammount"> of water</span>
        </p>
      </div>

      <h3>Manufactor Details</h3>
      <div class="Manufactor Name">
          <input v-model="chemicalToAdd.chemicalManufactor.manufactorName" placeholder="Manufactor's Name">
      </div>
      <div class="State Selection">
        <input v-model="chemicalToAdd.chemicalManufactor.manufactorAddress.city" placeholder="Manufactor's City">, 
          <select v-model="stateSelected" class="form-control">
            <option v-for="state in states" :value="state" v-bind:key="state">{{state}}</option>
          </select>, 
        <input v-model="chemicalToAdd.chemicalManufactor.manufactorAddress.zip" placeholder="Manufactor's Zip">,
      </div>
      <div class="Country Selection">
        <select v-model="countrySelected" class="form-control">
          <option v-for="count in country" :value="count" v-bind:key="count">{{count}}</option>
        </select>
        , <input v-model="chemicalToAdd.chemicalManufactor.manufactorAddress.street" placeholder="Manufactor's Street">
      </div>
      <button v-on:click="update++">Back</button>

      <h3>Chemical Safty Contact Details</h3>
          <input v-model="chemicalToAdd.saftyContactInformation.phoneNumber" placeholder="Phone Number">
          <input v-model="chemicalToAdd.saftyContactInformation.email" placeholder="Email">
          
      <div>
      <button v-on:click="submit()">Submit</button>
      
      </div>
  </div>
</template>

<script >
import axios from 'axios';

export default {
  name: 'ChemicalToAdd',
  components:{
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
      countrySelected: "",
      stateSelected:"",
      isDisinfectant: false,
      needsVenta: false,

      chemicalToAdd:{
          timeCreated: "",
          iD: "",
          isDeleted: "",
          organizationId: this.organization.iD,
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
      update: 0,
      instance: axios.create({
        baseURL: this.CSCLocation,
        timeout: 10000,
        headers: {
          "Content-Type": "text/plain"
        }
      }),
      errMessage: ""
    }
  },
  methods:{
   
    submit(){
      this.chemicalToAdd.chemicalManufactor.manufactorAddress.country = this.confirmCountry()
      this.chemicalToAdd.chemicalManufactor.manufactorAddress.state = this.confirmState()
      if(this.isDisinfectant){
        this.chemicalToAdd.disinfectant = "true"
      } else {
        this.chemicalToAdd.disinfectant = "false"
      }
      if(this.needsVenta){
        this.chemicalToAdd.ventilationNeeded = "true"
      } else {
        this.chemicalToAdd.ventilationNeeded = "false"
      }
      this.chemicalToAdd.isDeleted = "false"
      this.chemicalToAdd.timeCreated = "" + new Date().getTime() + ""
      this.chemicalToAdd.chemicalManufactor.manufactorAddress.mainAddress = "false"
      this.instance.post("", this.chemicalToAdd)
        .then((response) => {
          this.errMessage = response
        })
        .catch((err) => {
          this.errMessage = err

        })
        this.$emit('ChemicalAdded')
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
    this.chemicalToAdd = {
          timeCreated: new Date().getTime(),
          iD: "",
          isDeleted: false,
          organizationId: this.organization.iD,
          chemicalName: "",
          chemicalManufactor: {
            manufactorName: "",
            manufactorAddress: {
              country: "",
              state: "",
              city: "",
              zip: "",
              street: "",
              mainAddress: false
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