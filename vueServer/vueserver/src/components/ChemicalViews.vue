<template>
  <div class="Chemical" :key="update">
      <p>{{organization}} It worked</p>
  </div>
</template>

<script lang="ts">
import VueCompositionApi from "@vue/composition-api";
import { Component, Prop, Vue } from 'vue-property-decorator';
import axios, { AxiosInstance } from 'axios';
Vue.use(VueCompositionApi);

@Component
export default class ChemicalViews extends Vue {
    @Prop() private CSCLocation!: string;
    @Prop() private organization!: string;
  private instance!: AxiosInstance;
  private errMsg!: string;
  private update: number = 0;
  destroyed(){}

  beforeDestroy(){}

  updated(){
  }

  beforeUpdate(){
  }

  beforeCreate(){}

  created(){}

  beforeMounted(){
  }

  mounted(){
      this.instance = axios.create({
      baseURL: this.CSCLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
  }

  orgSelect(){}

refresh(){
      this.update++;
}

back(){
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