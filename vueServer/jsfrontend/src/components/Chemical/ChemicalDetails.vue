<template>
  <div class="ChemicalDetail">
      <h2>{{Chemical.chemicalName}}</h2>
      <p><span>{{Chemical.chemicalWarning}}</span><span :v-show="Chemical.ventilationNeeded"> and ventilation is needed when useing this chemical.</span></p>
      <p>To prepare this product use {{Chemical.usesAndPrep.chemicalPrepAmmount}} of product for every {{Chemical.usesAndPrep.waterPrepAmmount}} of water.</p>
      <p>Please contact {{Chemical.saftyContactInformation.phoneNumber}} or {{Chemical.saftyContactInformation.email}} for more safty information.</p>
      <Manufactor 
        :Manufactor="Chemical.chemicalManufactor"
        :states='states'
        :countries='countries'
      ></Manufactor>
  </div>
</template>

<script >
import Manufactor from './Manufactor'

export default {
  name: 'ChemicalDetail',
  components:{
      Manufactor
  },
  props:{
      Chemical: {
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
      countries: Array,
      states: Array
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