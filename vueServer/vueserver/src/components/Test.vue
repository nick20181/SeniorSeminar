<template>
  <div class="hello">
      <p>Test</p><ul id="demo">
        <p>Message: {{message}}</p>
        <p>{{org.organizationName}}</p>
</ul>
  </div>
</template>

<script lang="ts">
import VueCompositionApi from "@vue/composition-api";
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Organization } from '../Custodial.Service.Organization/Organization';

Vue.use(VueCompositionApi);
import { useAsync, useFetch } from "vue-async-function";

@Component
export default class Test extends Vue {
    @Prop() private message!: string;
    @Prop() private org!: Organization;
    
  destroyed(){}

  beforeDestroy(){}

  updated(){}

  beforeUpdate(){}

  beforeCreate(){}

  created(){}

  beforeMounted(){}

  mounted(){}
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