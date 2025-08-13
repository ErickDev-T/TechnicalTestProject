<!-- Table.vue -->
<template>
  <div class="bg-light p-3">
    <button @click="goForm" class="btn btn-success my-2">Agregar</button>

    <table class="table table-bordered table-hover table-sm align-middle">
      <thead>
        <tr>
          <th scope="col">id</th>
          <th scope="col">Nombre</th>
          <th scope="col">Desc</th>
          <th scope="col">Precio</th>
          <th scope="col">Stock</th>
          <th scope="col">Eliminar</th>
          <th scope="col">Editar</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="producto in productos" :key="producto.id">
          <th scope="row">{{ producto.id }}</th>
          <td>{{ producto.nombre }}</td>
          <td>{{ producto.descripcion }}</td>
          <td>{{ producto.precio }}</td>
          <td>{{ producto.stock }}</td>
          <td>
            <span class="icono" @click="deleted(producto.id)">❌</span>
          </td>
          <td>
            <span class="icono" @click="edit(producto.id)">🖋️</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { API, IProducto } from '../constantes'

const router = useRouter()
const goForm = () => router.push({ name: 'add-stock' })

const productos = ref<IProducto[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

//   DELETE /api/Products/{id}
//   DELETE /api/Products/DeleteProduct/{id}
const deleteUrl = (id: number) => `${API}/Products/delete/${id}`
// const deleteUrl = (id: number) => `${API}/Products/DeleteProduct/${id}`

const deleted = async (id: number) => {
  if (!confirm(`¿Eliminar producto #${id}?`)) return
  try {
    const res = await fetch(deleteUrl(id), { method: 'DELETE' })
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    // quita el item localmente
    productos.value = productos.value.filter(p => p.id !== id)
  } catch (e: any) {
    console.error(e)
    alert(`No se pudo eliminar: ${e?.message ?? e}`)
  }
}

const edit = (id: number) => {
  router.push({ name: 'edit', params: { id } })
}


onMounted(async () => {
  loading.value = true
  try {
    const res = await fetch(`${API}/Products/listed`)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    productos.value = await res.json() as IProducto[]
  } catch (e: any) {
    error.value = e.message ?? String(e)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.icono { cursor: pointer; }
</style>
