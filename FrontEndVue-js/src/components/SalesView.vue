<template>
  <div class="bg-light p-4 rounded shadow-sm">
    <h4 class="mb-3 text-primary">
      <i class="bi bi-receipt-cutoff me-2"></i> Lista de Ventas
    </h4>

    <div class="table-responsive">
      <table class="table table-striped table-hover align-middle text-center">
        <thead class="table-dark">
          <tr>
            <th>ID</th>
            <th>Fecha</th>
            <th>Cliente</th>
            <th>Total</th>
            <th>Ver Detalles</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="venta in ventas" :key="venta.id">
            <td>{{ venta.id }}</td>
            <td>{{ formatDate(venta.fecha) }}</td>
            <td>{{ venta.cliente }}</td>
            <td>
              <span class="badge bg-info">${{ venta.total }}</span>
            </td>
            <td>
              <button class="btn btn-sm btn-outline-primary" @click="verDetalles(venta.id)">
                <i class="bi bi-eye"></i>
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
import { API } from '../constantes'
import Swal from 'sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'

//definiendo los valores y tipos de datos de las ventas
interface IVenta {
  id: number
  fecha: string
  cliente: string
  total: number
}

const ventas = ref<IVenta[]>([])
const error = ref<string | null>(null)

//cambiando el formato de la fecha
const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('es-DO', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}
//pop up con sus parametros
const verDetalles = (id: number) => {
  Swal.fire({
    title: `Detalles de la venta #${id}`,
    text: 'para mostrar un modal con los productos vendidos.',
    icon: 'info'
  })
}
// carga los datos cuando el DOM este cargado 
onMounted(async () => {
  try {
    const res = await fetch(`${API}/Ventas/listed`)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    ventas.value = await res.json() as IVenta[]
  } catch (e: any) {
    error.value = e.message ?? String(e) //almacena el error en un msg
    await Swal.fire({ icon: 'error', title: 'Error', text: error.value })
  } 
})



</script>
