<template>
  <div class="Organization">
      <button v-on:click="onClickButton()">{{Organization.organizationName}}</button>
  </div>
</template>

<script >

export default {
  name: 'Organization',
  components:{
  },
  props:{
      Organization: undefined
  },
  data(){
      return{
      }
  },
  methods:{
      onClickButton(){
          this.$emit('clicked', this.Organization)
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