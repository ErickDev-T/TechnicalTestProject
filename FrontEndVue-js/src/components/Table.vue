<template>
  <div class="bg-light p-4 rounded shadow-sm">
    <h4 class="mb-3 text-primary">
      <i class="bi bi-box-seam me-2"></i> Lista de Productos
    </h4>

    <button @click="goForm" class="btn btn-success mb-3">
      <i class="bi bi-plus-lg me-1"></i> Agregar Producto
    </button>

    <div class="table-responsive">
      <table class="table table-striped table-hover align-middle text-center">
        <thead class="table-dark">
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Descripción</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Eliminar</th>
            <th>Editar</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="producto in productos" :key="producto.id">
            <td>{{ producto.id }}</td>
            <td>{{ producto.nombre }}</td>
            <td>{{ producto.descripcion }}</td>
            <td>
              <span class="badge bg-info">
                ${{ producto.precio }}
              </span>
            </td>
            <td>
              <span 
                class="badge"
                :class="producto.stock > 5 ? 'bg-success' : producto.stock > 0 ? 'bg-warning' : 'bg-danger'">
                {{ producto.stock }}
              </span>
            </td>
            <td>
              <button class="btn btn-sm btn-outline-danger" @click="deleted(producto.id)">
                <i class="bi bi-trash"></i>
              </button>
            </td>
            <td>
              <button class="btn btn-sm btn-outline-primary" @click="edit(producto.id)">
                <i class="bi bi-pencil"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>



<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { API, IProducto } from '../constantes'
import Swal from 'sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'

const router = useRouter()
const goForm = () => router.push({ name: 'add-stock' })

const productos = ref<IProducto[]>([])
const loading = ref(false)
const error = ref<string | null>(null)

const deleteUrl = (id: number) => `${API}/Products/delete/${id}`

const deleted = async (id: number) => {
  const { isConfirmed } = await Swal.fire({
    title: `¿Eliminar producto #${id}?`,
    text: 'Esta acción no se puede deshacer.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar',
    reverseButtons: true,
    focusCancel: true
  })
  if (!isConfirmed) return

  try {
    const res = await fetch(deleteUrl(id), { method: 'DELETE' })
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    productos.value = productos.value.filter(p => p.id !== id)

    await Swal.fire({
      icon: 'success',
      title: 'Eliminado',
      text: 'El producto fue eliminado correctamente.',
      timer: 1500,
      showConfirmButton: false
    })
  } catch (e: any) {
    console.error(e)
    await Swal.fire({
      icon: 'error',
      title: 'No se pudo eliminar',
      text: e?.message ?? String(e)
    })
  }
}

const edit = (id: number) => router.push({ name: 'edit', params: { id } })

onMounted(async () => {
  loading.value = true
  try {
    const res = await fetch(`${API}/Products/listed`)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    productos.value = await res.json() as IProducto[]
  } catch (e: any) {
    error.value = e.message ?? String(e)
    await Swal.fire({ icon: 'error', title: 'Error', text: error.value })
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.icono { cursor: pointer; }
</style>
