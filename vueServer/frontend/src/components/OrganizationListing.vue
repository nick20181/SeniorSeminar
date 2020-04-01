<template>
  <div class="OrginizationListing" :key="update">
    <button>
        <slot>{{Org.organizationName}}</slot>
    </button>
  </div>
</template>

<script lang="ts">
import { Organization } from '../Custodial/Organization/Organization';

export default {
  name: 'OrganizationListing',
  components:{

  },
  props:{
      Org: Organization
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