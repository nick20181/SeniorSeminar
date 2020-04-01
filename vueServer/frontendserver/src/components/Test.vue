<template>
  <div class="test">
    Heelo
    {{message}}
  </div>
</template>

<script lang="ts">

export default {
  name: 'Test',
  props: {
    msg: String
  },
  components: {
  },
  data(){
    return {
      message: "hi"
    }
  },
  created(){
    this.message = 'fuck'
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
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>