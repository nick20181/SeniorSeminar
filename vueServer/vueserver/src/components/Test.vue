<template>
  <div class="test">
      <p>Test</p><ul id="demo">
        <div v-if="message === undefined">
          <h1>Loading...</h1>
        </div>
        <div v-else>
          <h1>{{message}}</h1>
        </div>
</ul>
  </div>
</template>

<script lang="ts">
import VueCompositionApi from "@vue/composition-api";
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Organization } from '../Custodial.Service.Organization/Organization';
import axios from 'axios'

Vue.use(VueCompositionApi);
import { useAsync, useFetch } from "vue-async-function";

@Component
export default class Test extends Vue {
    @Prop() private message!: string;
    
  destroyed(){}

  beforeDestroy(){}

  updated(){}

  beforeUpdate(){}

  beforeCreate(){}

  created(){}

  beforeMounted(){}

  mounted(){
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