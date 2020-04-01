<template>
  <div class="test">
      <p>Test</p><ul id="demo">
        <div>
          Test
          {{message}}
        </div>
</ul>
  </div>
</template>

<script lang="ts">
import VueCompositionApi from "@vue/composition-api";
import { Organization } from '../Custodial.Service.Organization/Organization';
import axios from 'axios'

export default {
  name: "test"
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