<template>
  <div class="orgSelect" :key="update">
    <div v-show="!orgSelectedBool">
        <button v-on:click="refresh()">Refresh</button>
        <p>Please Choose a organization</p>
        <ul id="orgList" class="demo" :key="CSOLocation">
            <li v-for="organization in orgList" :key="organization">
                <button v-on:click="orgSelect(organization)">{{ organization.organizationName }}</button>
            </li>
        </ul>
    </div>
    <div v-show="orgSelectedBool" :key="update">
        <p :key="orgSelected">Organization Selected: {{orgSelectedName}}</p>
        
        <button v-on:click="back()">Back</button>
    </div>
  </div>
</template>

<script lang="ts">
import VueCompositionApi from "@vue/composition-api";
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Organization } from '../Custodial.Service.Organization/Organization';
import axios, { AxiosInstance } from 'axios';
Vue.use(VueCompositionApi);
import { useAsync, useFetch } from "vue-async-function";

@Component
export default class OrganizationSelection extends Vue {
    @Prop() private CSOLocation!: string;
  private instance!: AxiosInstance;
  private orgList!: Array<Organization>;
  private errMsg!: string;
  private update: number = 0;
  private orgSelected!: Organization;
  private orgSelectedName: string = "none";
  private orgSelectedBool: boolean = false;
  destroyed(){}

  beforeDestroy(){}

  updated(){
  }

  beforeUpdate(){
    this.instance.get('/all').then(res => {
      this.orgList = res.data;
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
  }

  beforeCreate(){}

  created(){}

  beforeMounted(){
  }

  mounted(){
      this.instance = axios.create({
      baseURL: this.CSOLocation,
      timeout: 10000,
      headers: {"Content-Type": "application/json"}
      });
      this.update++;
      this.instance.get('/all').then(res => {
      this.orgList = res.data;
      this.update++;
    }).catch(e => { 
      this.errMsg = e+ ": " ;
    })
  }

  orgSelect(Org: Organization){
      this.orgSelected = Org
      this.orgSelectedName = this.orgSelected.organizationName;
      this.orgSelectedBool = true;
      this.update++;

  }

refresh(){
      this.update++;
}

back(){
    this.orgSelectedBool = false;
      this.update++;
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