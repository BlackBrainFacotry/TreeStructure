<script setup lang="ts">
    import { onMounted, ref } from "vue"
    import {VueDraggableNext as Draggable} from 'vue-draggable-next';
    import UpdateModal from './UpdateNode.vue';
    import DeleteModal from './DeleteNode.vue';

    interface NodeInterface {
        id: number,
        name: string,
        parentId: number | null,
        childrens: NodeInterface[]
    }

    const isExpanded = ref(false);
    const props = defineProps<{nodes: NodeInterface[]}>() 
    const nodes = props.nodes
    const emit = defineEmits(['refreshData'])
    const log = (data: any) => console.log(data)
    const update = (data: any, data2: any) => console.log(data, data2)

    const showUpdateModal = ref<{value: number | null, isVisible: boolean}>({value: null,isVisible:false})
    const toggleShowUpdateModal = (value: number | null) => {
        showUpdateModal.value = value !== null ?  { value, isVisible: true} : { value: null, isVisible: false }
    }
    
    const showDeleteModal = ref<{value: number | null, isVisible: boolean}>({value: null,isVisible:false})
    const toggleShowDeleteModal = (value: number | null) => {
        showDeleteModal.value = value !== null ?  { value, isVisible: true} : { value: null, isVisible: false }
    }

    const toggleExpanded = () => {
        isExpanded.value = !isExpanded.value
    }

    const refreshData = () => {
        emit("refreshData")
    }
    
    const updateMovedNode = async (data: any) => {
        if(data.added && data.added.element){
            const { id } = data.added.element

            
            const movedElement = document.querySelector(`#element-${id}`)
            console.log(id)
            const parentIdtarget = movedElement?.parentElement?.parentElement?.parentElement?.id.replace('element-', '')
            console.log(parentIdtarget)
            
            const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ 
                            id: parentIdtarget && parentIdtarget.length ? parentIdtarget : null,
                            name: "newName.value",
                            parentId: null,
                            childrens: [] })
            };
            try {
                const response = await fetch("https://localhost:7053/api/nodes/move/" + id, requestOptions)
                const response_data = await response.json()
                console.log(response_data)
                return response_data

            } catch ( err : any) {
                console.error(err)
                
            } 
            finally {
                emit("refreshData")
            }
        }
    }

</script>

<template>
    <draggable tag="ul" :list="nodes" :group="{ name: 'g1' }" @change="updateMovedNode" >
        <template v-for="(item) in nodes" :key="item.id">
            <div class="nodes--element" :id="`element-${item.id.toString()}`" >
                <div class="nodes--element__name" @click="toggleExpanded" > {{ item.name}} 
                    <div class="nodes--element__name_buttons">
                        <button @click="toggleShowUpdateModal(item.id)">Update</button>
                        <button @click="toggleShowDeleteModal(item.id)">Delete</button>
                    </div>
                </div>
                <div class="nodes--element_childs" v-if="item.childrens">
                    <nodes-list :nodes="item.childrens" class="ml-10" @refreshData="refreshData"/>
                </div>
            </div>
        </template>
    </draggable>
    <delete-modal v-if="showDeleteModal.isVisible " :id="showDeleteModal.value"  @closeModal="toggleShowDeleteModal(null)" @refreshData="refreshData"/>
    <update-modal v-if="showUpdateModal.isVisible " :id="showUpdateModal.value"  @closeModal="toggleShowUpdateModal(null)" @refreshData="refreshData"/>
</template>

<style lang="scss" scoped>
    .nodes--element {
        width: 100%;
        display: flex;
        flex-direction: column;
        &__name {
            position: relative;
            background: rgb(130, 164, 236);
            padding: 8px 12px;
            border-radius: 4px;
            color: #000;
            cursor: pointer;
            &_buttons{
                position: absolute;
                right: 0;
                top: 50%;
                transform: translateY(-50% )
            }
        }

        &_childs {
            padding-bottom: 2px;
            margin-bottom: 4px;
        }
        
    }

    button {
        color: rgb(0, 0, 0);
        padding: 6px 14px;
        border-radius: 6px;
        background: rgb(255, 255, 255);
        margin: 4px 4px 4px 0;
      }
</style>

<style>
    ul {
        padding: 4px;
    }
</style>