<template>
  <div class="ChemicalToAdd" :key="update">
      <h3>Manufactor Details</h3>
      <div class="Manufactor Name">
          <input v-model="chemicalManufactorerName" placeholder="Manufactor's Name">
      </div>
      <div class="State Selection">
        <input v-model="chemicalManuAddressCity" placeholder="Manufactor's City">, 
          <select v-model="chemicalManuAddressState" class="form-control">
            <option v-for="state in states" :value="state" v-bind:key="state">{{state}}</option>
          </select>, 
        <input v-model="chemicalManuAddressZip" placeholder="Manufactor's Zip">,
      </div>
      <div class="Country Selection">
        <select v-model="chemicalManuAddressCountry" class="form-control">
          <option v-for="count in country" :value="count" v-bind:key="count">{{count}}</option>
        </select>
        , <input v-model="chemicalManuAddressStreet" placeholder="Manufactor's Street">
      </div>
      <button v-on:click="update++">Back</button>
  </div>
</template>

<script >

export default {
  name: 'ChemicalToAdd',
  components:{
  },
  props:{
      organization: undefined,
      country: [],
      states: []
  },
  data(){
      return{
          coun: ["United States of America"],
          today: new Date(),
          timeCreated: this.today.getTime(),
          iD: "",
          isDeleted: false,
          organizationId: this.organization.iD,
          chemicalName: "",
          chemicalManufactorerName: "",
          chemicalManuAddressCountry: "",
          chemicalManuAddressState: "",
          chemicalManuAddressCity: "",
          chemicalManuAddressZip: "",
          chemicalManuAddressStreet: "",
          chemicalManuAddressMainAddress: false,
          chemicalSaftyEmail: "",
          chemicalSaftyPhone: "",
          chemicalIsDisinfectant: false,
          chemicalNeedsventalation: false,
          chemicalPrepChemAmmount: "",
          chemicalPrepWaterAmmount: "",
          update: 0,

      }
  },
  methods:{
      //country(country){

      //},
      //state(state){

      //}
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