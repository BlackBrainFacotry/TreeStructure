<script setup lang="ts">
  import { onMounted, provide, ref } from 'vue'
  import NodesList from './components/NodesList.vue';
  import CreateModal from './components/Create.vue';

  import {VueDraggableNext as Draggable} from 'vue-draggable-next';

  
  const sortAscending = ref(true)
  const loading = ref(false)
  const fetchingError = ref(false)
  const showCreateModal = ref(false)
  const nodes = ref([])

  const toggleShowCreateModal = () => { showCreateModal.value = !showCreateModal.value}
 
  const getData = async () => {
    loading.value = true
    try {
      const response = await fetch("https://localhost:7053/api/nodes/"+ sortAscending.value)

      const response_data = await response.json()
      console.log(response_data)
      nodes.value = response_data
    } catch ( err : any) {
      console.error(err)
      fetchingError.value = true
    } finally {
      loading.value = false
    }
  }
  
  onMounted( () => {
    getData();
  })
  
  
  const toggleSort = () => {
    sortAscending.value = !sortAscending.value
    getData()
  }

  
  </script>
  
  <template>
  <div v-if="nodes.length">
    <div class="nodes-module" v-if="!loading">
      <h1>Tree structure</h1>
      <div class="nodes-module-heading-buttons">
        <button @click="toggleSort"> Reverse sort direction </button>
        <button @click="toggleShowCreateModal"> Create new</button>
      </div>
      <div class="nodes-module-content">
        <NodesList :nodes="nodes" @refreshData="getData"/>
      </div>
    </div>
    <div v-else-if="fetchingError">
      Error
    </div>
    <div v-else>
      Loading...
    </div>
    <create-modal v-if="showCreateModal && nodes" @submitSuccess="getData" :nodes="nodes" @closeModal="toggleShowCreateModal"/>

  </div>
  
  </template>
  
  <style lang="scss" scoped>
  .nodes-module {
    display: flex;
    flex-direction: column;
    align-items: stretch;
    gap: 4px;
    background: #fff;
    padding: 20px 22px;
    border-radius: 8px;
  
    h1 {
      font-size: 2rem;
      color: #000;
    }
    
    &-heading-buttons {
      display: flex;
      align-items: center;
      gap: 8px;
  
  
      button {
        color: #fff;
        padding: 6px 14px;
        border-radius: 6px;
        background: rgba(0,0,20, 0.8);
      }
    }
  
    &-content {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
  }
  
  </style>
  
  