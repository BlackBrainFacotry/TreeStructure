<script setup lang="ts">
  import { onMounted, provide, ref } from 'vue'
  
  const loading = ref(false)
  const fetchingError = ref(false)    
  
  const { nodes, id } = defineProps<{nodes: any,id: number | null}>()
  
  const emit = defineEmits(['refreshData','closeModal'])

const deleteOne = async () => {
    loading.value = true
    try {
      const response = await fetch("https://localhost:7053/api/nodes/deletenode/" + id)
      const response_data = await response.json()

      emit("refreshData")
      return response_data

    } catch ( err : any) {
      console.error(err)
      fetchingError.value = true
    } finally {
      loading.value = false
      emit("closeModal")
    }
}

const deleteAll = async () => {
loading.value = true
    try {
      const response = await fetch("https://localhost:7053/api/nodes/deleteroot/" + id)
      const response_data = await response.json()

      emit("refreshData")
      return response_data

    } catch ( err : any) {
      console.error(err)
      fetchingError.value = true
    } finally {
      loading.value = false
      emit("closeModal")
      emit("refreshData")
    }
}
  </script>

<template>
    <div class="modal">
        <div class="modal__overlay"></div>
        <form  class="modal__content">
            <div @click="$emit('closeModal')" class="close">X</div>
            <div class="modal__content_text" >
                <p> Do you want to delete this node only or the node and child nodes? </p>
            </div>
            <div class="modal__content_buttonsdiv">
                <button @click.native="deleteOne"> Delete One</button>
                <button @click="deleteAll"> Delete All</button>
            </div>
          </form>
      </div>
    </template>
<style lang="scss" scoped>
  .modal {
      position: fixed;
      top: 0;
      bottom: 0;
      left: 0;
      right: 0;

      &__overlay {
          position: absolute;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          backdrop-filter: blur(2px);
          background: rgba(0,0,0,.3);
      }

      &__content {
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          background: #fff;
          border-radius: 8px;
          min-width: 420px;

          display: flex;
          flex-direction: column;
          align-items: stretch;
          gap: 12px;

          padding: 12px 18px;
          color: #020202;

          &_text {
              padding: 8px 8px;
              width: 100%;
              margin: 12px 0;
          }         
          
          &_buttonsdiv {
            display: flex;
            justify-content: center;
          }

          button {
              padding: 8px 12px;
              background: rgb(236, 81, 81);
              border:0; 
              border-radius: 2px;
              margin: 4px 10px 4px 10px;
          }

          .close {
              position: absolute;
              right: 12px;
              top: 12px;
              cursor: pointer;
          }
      }
  }
  </style>