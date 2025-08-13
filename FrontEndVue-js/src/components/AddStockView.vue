<template>
  <div class="card mx-auto mt-5 p-3" style="max-width: 350px; width: 100%;">
    <h1 class="text-center">Agregar producto</h1>

    <form @submit.prevent="formProcesing" class="card-body">
      <input
        class="form-control mt-2"
        type="text"
        v-model.trim="formSave.nombre"
        placeholder="Nombre"
        required
      />

      <input
        class="form-control mt-2"
        type="text"
        v-model.trim="formSave.descripcion"
        placeholder="Descripción"
        required
      />

      <input
        class="form-control mt-2"
        type="text"
        v-model="formSave.precio"
        placeholder="Precio"
        required
      />

      <input
        class="form-control mt-2"
        type="text"
        v-model="formSave.stock"
        placeholder="Stock"
        required
      />

      <button class="btn btn-primary w-100 mt-3" :disabled="loading">
        {{ loading ? 'Guardando...' : 'Guardar' }}
      </button>

      <p v-if="errorMsg" class="text-danger mt-2">{{ errorMsg }}</p>
      <p v-if="okMsg" class="text-success mt-2">{{ okMsg }}</p>
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
