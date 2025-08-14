<template>
  <div class="card mx-auto mt-5 shadow-lg border-0" style="max-width: 400px; width: 100%;">
    <div class="card-header bg-primary text-white text-center py-3">
      <i class="bi bi-box-seam fs-3"></i>
      <h4 class="mt-2 mb-0">Agregar Producto</h4>
    </div>

    <form @submit.prevent="formProcesing" class="card-body">
      <div class="form-floating mb-3">
        <input type="text" class="form-control" id="nombre" placeholder="Nombre"
               v-model.trim="formSave.nombre" required>
        <label for="nombre">Nombre</label>
      </div>

      <div class="form-floating mb-3">
        <input type="text" class="form-control" id="descripcion" placeholder="Descripción"
               v-model.trim="formSave.descripcion" required>
        <label for="descripcion">Descripción</label>
      </div>

      <div class="form-floating mb-3">
        <input class="form-control" id="precio" placeholder="Precio"
               v-model="formSave.precio" required>
        <label for="precio">Precio</label>
      </div>

      <div class="form-floating mb-4">
        <input class="form-control" id="stock" placeholder="Stock"
               v-model="formSave.stock" required>
        <label for="stock">Stock</label>
      </div>

      <button class="btn btn-success w-100 py-2" :disabled="loading">
        <i class="bi bi-check-circle me-1"></i>
        {{ loading ? 'Guardando...' : 'Guardar' }}
      </button>
          <!--se muestra solo si el igual es verdadero--> 
      <p v-if="errorMsg" class="text-danger mt-3">{{ errorMsg }}</p>
      <p v-if="okMsg" class="text-success mt-3">{{ okMsg }}</p>
    </form>
  </div>
</template>


<script setup lang="ts">
import {API, IProducto} from '../constantes';

import { onMounted, reactive, ref } from 'vue'

type ProductoDTO = {
  nombre: string
  descripcion: string
  precio: string | number
  stock: string | number
}

const formSave = reactive<ProductoDTO>({
  nombre: '',
  descripcion: '',
  precio: '',
  stock: ''
})

const loading = ref(false)
const errorMsg = ref<string | null>(null)
const okMsg = ref<string | null>(null)

onMounted(() =>{
  
})

const formProcesing = async () => {
  loading.value = true
  errorMsg.value = null
  okMsg.value = null

  try {
    // normaliza tipos numéricos
    const payload = {
      nombre: formSave.nombre,
      descripcion: formSave.descripcion,
      precio: Number(formSave.precio),
      stock: Number.parseInt(String(formSave.stock), 10)
    }

    const res = await fetch(`${API}/Products/SaveProduct`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    })

    if (!res.ok) {
      const text = await res.text()
      throw new Error(`HTTP ${res.status}: ${text}`)
    }

    okMsg.value = 'Producto guardado con éxito ✅'

    formSave.nombre = ''
    formSave.descripcion = ''
    formSave.precio = ''
    formSave.stock = ''
  } catch (e: any) {
    errorMsg.value = e?.message ?? 'Error al guardar'
    console.error(e)
  } finally {
    loading.value = false
  }
}
</script>
