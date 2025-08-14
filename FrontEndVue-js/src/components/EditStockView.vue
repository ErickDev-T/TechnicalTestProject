<template>
  <div class="container py-3">
    <h3 class="mb-3">Editar producto #{{ id }}</h3>

    <div v-if="loading" class="alert alert-info">Cargando producto…</div>
    <div v-if="error" class="alert alert-danger">{{ error }}</div>

    <form v-if="producto" @submit.prevent="save" novalidate class="needs-validation">
      <div class="row g-3">
        <div class="col-md-6">
          <label class="form-label">Nombre</label>
          <input
            v-model.trim="producto.nombre"
            class="form-control"
            required
            :disabled="saving"
          />
        </div>

        <div class="col-md-6">
          <label class="form-label">Descripción</label>
          <input
            v-model.trim="producto.descripcion"
            class="form-control"
            :disabled="saving"
          />
        </div>

        <div class="col-md-6">
          <label class="form-label">Precio</label>
          <input
            type="number"
            v-model.number="producto.precio"
            class="form-control"
            min="0"
            step="0.01"
            required
            :disabled="saving"
          />
        </div>

        <div class="col-md-6">
          <label class="form-label">Stock</label>
          <input
            type="number"
            v-model.number="producto.stock"
            class="form-control"
            min="0"
            step="1"
            required
            :disabled="saving"
          />
        </div>
      </div>

      <div class="mt-4 d-flex gap-2">
        <button type="submit" class="btn btn-primary" :disabled="saving">
          <span v-if="saving" class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
          Guardar cambios
        </button>
        <router-link :to="{ name: 'stock' }" class="btn btn-secondary" :class="{ disabled: saving }">
          Cancelar
        </router-link>
      </div>
    </form>

    <!-- si no lo encuentra-->
    <div v-else-if="!loading && !error" class="alert alert-warning">
      No se encontró el producto #{{ id }}.
      <router-link :to="{ name: 'stock' }">Volver</router-link>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { API, IProducto } from '../constantes'

const route = useRoute()
const router = useRouter()
const id = Number(route.params.id)

const producto = ref<IProducto | null>(null)
const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)

// toma los datos dasde la tabla
const stateProd = (history.state && (history.state as any).producto) as IProducto | undefined
if (stateProd && stateProd.id === id) {
  producto.value = { ...stateProd }
}

onMounted(async () => {
  if (producto.value) return
  loading.value = true
  error.value = null
  try {
    const res = await fetch(`${API}/Products/listed`)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    const list = (await res.json()) as IProducto[]
    const found = list.find(p => p.id === id)
    if (!found) {
      return
    }
    producto.value = { ...found }
  } catch (e: any) {
    error.value = e?.message ?? String(e)
  } finally {
    loading.value = false
  }
})

// 3) Guardar 
const save = async () => {
  if (!producto.value) return
  // Validaciones 
  if (!producto.value.nombre?.trim()) {
    alert('Nombre es requerido')
    return
  }
  if (producto.value.precio == null || producto.value.precio < 0) {
    alert('Precio inválido')
    return
  }
  if (producto.value.stock == null || producto.value.stock < 0) {
    alert('Stock inválido')
    return
  }

  saving.value = true
  error.value = null
  try {
    // asegura que el body tenga el id correcto
    const body: IProducto = {
      ...producto.value,
      id 
    }

    const res = await fetch(`${API}/Products/update/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    })

    if (!res.ok) {
      // devuelven 200 con el objeto otros 204 si no es ok avisa
      throw new Error(`HTTP ${res.status}`)
    }


    alert('Producto actualizado correctamente')
    router.push({ name: 'stock' })
  } catch (e: any) {
    error.value = e?.message ?? String(e)
    alert(`No se pudo actualizar: ${error.value}`)
  } finally {
    saving.value = false
  }
}
</script>

<style scoped>
.container { max-width: 800px; }
</style>
