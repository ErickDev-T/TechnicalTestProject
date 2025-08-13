<template>
<div>
    <table class="table">
    <thead>
        <tr>
        <th scope="col">id</th>
        <th scope="col">Nombre</th>
        <th scope="col">Desc</th>
        <th scope="col">Precio</th>
        <th scope="col">stock</th>

        </tr>
    </thead>

    <tbody>

        <tr v-for="producto in productos":key ="producto.id">
            <th scope="row">{{ producto.id }}</th>
          <td>{{ producto.nombre }}</td>
          <td>{{ producto.descripcion }}</td>
          <td>{{ producto.precio }}</td>
          <td>{{ producto.stock }}</td>
          <td>
            <span class="icono">
                ❌
            </span>
          </td>
          <td>
            <span>
                🖋️
            </span class="icono">
          </td>
        </tr>

        

    </tbody>
    
    </table>
</div>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import {API, IProducto} from "../constantes"

const productos = ref<IProducto[]>([])   
const loading = ref(false)
const error = ref<string | null>(null)

onMounted(async () => {



    
  loading.value = true
  try {
    const res = await fetch(`${API}/Products/listed`)  
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    const data = (await res.json()) as IProducto[]      
    productos.value = data                            
  } catch (e: any) {
    error.value = e.message ?? String(e)
  } finally {
    loading.value = false
  }
  
})

</script>
   
<style>
.icono{
    cursor: pointer;
}
</style>