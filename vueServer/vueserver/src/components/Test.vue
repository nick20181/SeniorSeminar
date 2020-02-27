<template>
  <div class="hello">
      <p>Test</p><ul id="demo">
        <p>{{message}}</p>
         {{mess}}
      <h1>{{orgList}}</h1>
</ul>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { Organization } from '../Custodial.Service.Organization/Organization';
import { APIHandler } from '../API Handler';
import { ServiceDictionary } from '../ServiceDictionary';

@Component
export default class HelloWorld extends Vue {
    @Prop() private orgList!: string;
    @Prop() private apiHandler!: APIHandler;
    @Prop() private serviceDict!: ServiceDictionary;
    private message!: string;
    private mess!: string;
  async destroyed(){
  }

  async beforeDestroy(){
      //https://xebia.com/blog/next-generation-async-functions-with-vue-async-function/
  }

  async updated(){
  }

  async beforeUpdate(){
  }

  async beforeCreate(){
    await this.apiHandler.refreshServiceDictionary();
    this.orgList = await this.apiHandler.getOrganizationList();
  }

  created(){
    this.message = 'Org Service at: '+this.serviceDict.getCSO().settings.networkSettings.addresses[1] + ':' +this.serviceDict.getCSO().settings.networkSettings.ports[0]
  }

  async beforeMounted(){
    this.orgList = await this.apiHandler.getOrganizationList();
  }

  async mounted(){
    this.mess = this.serviceDict.getCSO().serviceName;
    this.message = 'Org Service at: '+this.serviceDict.getCSO().settings.networkSettings.addresses[1] + this.serviceDict.getCSO().settings.networkSettings.ports[0]
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