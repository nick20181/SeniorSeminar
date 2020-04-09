<template>
  <div class="Manufactor">
      <p><button v-on:click="showDataButton()">Manufactor: {{Manufactor.manufactorName}}</button></p>
      <p v-show="showData">{{Manufactor.manufactorName}} is located at {{Manufactor.manufactorAddress.city}}, {{country(Manufactor.manufactorAddress.country)}}, {{Manufactor.manufactorAddress.zip}} at {{Manufactor.manufactorAddress.street}}, {{state(Manufactor.manufactorAddress.state)}}<span v-show="Manufactor.manufactorAddress.mainAddress"> and is the main address</span>.</p>
  </div>
</template>

<script >

export default {
  name: 'Manufactor',
  components:{
  },
  props:{
      Manufactor: {
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
      countries: Array,
      states: Array
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