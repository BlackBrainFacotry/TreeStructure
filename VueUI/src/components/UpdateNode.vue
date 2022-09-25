<script setup lang="ts">
import { onMounted, provide, ref } from 'vue'

const loading = ref(false)
const fetchingError = ref(false)    
const newName = ref("")
    
const { nodes, id} = defineProps<{nodes: any,id: number | null}>()    

const emit = defineEmits(['refreshData','closeModal'])

const postUpdateNode = async () => {
    loading.value = true
    const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ 
                    id,
                    name: newName.value,
                    parentId: null,
                    childrens: [] })
    };
    try {
        const response = await fetch("https://localhost:7053/api/nodes/update", requestOptions)
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

</script>

<template>
    <div class="modal">
        <div class="modal__overlay"></div>
        <form :onSubmit="postUpdateNode" class="modal__content">
                <div @click="$emit('closeModal')" class="close">X</div>
            <div>
                <h3>Update name</h3>
                <input v-model="newName" placeholder="Enter new name" />
            </div>
        <button type="submit" :disabled="loading" @click="postUpdateNode"> Update </button>
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

            input {
                padding: 4px 8px;
                width: 100%;
                margin: 12px 0;
            }

            button {
                padding: 8px 12px;
                background: lightgreen;
                border:0; 
                border-radius: 2px;
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