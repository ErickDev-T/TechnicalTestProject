<template>
  <div class="container py-3">
    <h3 class="mb-3">Nueva venta</h3>

    <div v-if="loading" class="alert alert-info">Cargando productos…</div>
    <div v-if="error" class="alert alert-danger">{{ error }}</div>

    <div v-if="!loading" class="row g-3">
      <!-- Catálogo / búsqueda -->
      <div class="col-lg-6">
        <div class="card shadow-sm">
          <div class="card-body">
            <h5 class="card-title">Productos</h5>

            <input
              v-model.trim="q"
              type="text"
              class="form-control my-3"
              placeholder="Buscar por nombre o descripción…"
            />

            <div class="list-group list-group-flush" style="max-height: 380px; overflow-y: auto;">
              <button
                v-for="p in filtered"
                :key="p.id"
                class="list-group-item list-group-item-action d-flex justify-content-between align-items-center"
                @click="select(p)"
              >
                <div>
                  <div class="fw-semibold">{{ p.nombre }}</div>
                  <small class="text-muted">{{ p.descripcion }}</small>
                </div>
                <div class="text-end">
                  <div class="fw-semibold">{{ money(p.precio) }}</div>
                  <small class="text-muted">Stock: {{ p.stock }}</small>
                </div>
              </button>

              <div v-if="!filtered.length" class="text-center text-muted py-3">
                Sin resultados
              </div>
            </div>

            <div v-if="picked" class="border-top pt-3 mt-3 d-flex align-items-center gap-2">
              <div class="flex-grow-1">
                <div class="small text-muted">Seleccionado</div>
                <div class="fw-semibold">{{ picked.nombre }}</div>
              </div>
              <input type="number" min="1" :max="picked.stock" v-model.number="qty" class="form-control" style="width:120px" />
              <button class="btn btn-outline-primary" @click="add()">Añadir</button>
            </div>
          </div>
        </div>
      </div>

      <!-- Carrito / totales / acciones -->
      <div class="col-lg-6">
        <div class="card shadow-sm mb-3">
          <div class="card-body">
            <h5 class="card-title">Carrito</h5>

            <div v-if="!cart.length" class="text-muted text-center py-3">
              Agrega productos para iniciar la venta.
            </div>

            <div v-else class="table-responsive">
              <table class="table table-sm align-middle">
                <thead>
                  <tr>
                    <th>Producto</th>
                    <th class="text-end">Precio</th>
                    <th class="text-end">Cantidad</th>
                    <th class="text-end">Importe</th>
                    <th></th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(it, i) in cart" :key="it.productoId">
                    <td>
                      <div class="fw-semibold">{{ it.nombre }}</div>
                      <small class="text-muted">Stock: {{ it.stock }}</small>
                    </td>
                    <td class="text-end">{{ money(it.precio) }}</td>
                    <td class="text-end">
                      <input
                        type="number"
                        min="1"
                        :max="it.stock"
                        v-model.number="it.cantidad"
                        class="form-control form-control-sm text-end"
                        @change="fixQty(i)"
                      />
                    </td>
                    <td class="text-end">{{ money(it.cantidad * it.precio) }}</td>
                    <td class="text-end">
                      <button class="btn btn-sm btn-outline-danger" @click="removeAt(i)">❌</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div class="d-flex justify-content-end">
              <div style="min-width: 320px;">
                <div class="d-flex justify-content-between">
                  <span>Subtotal</span>
                  <strong>{{ money(subtotal) }}</strong>
                </div>
                <div class="d-flex justify-content-between">
                  <span>Impuestos ({{ (taxRate*100).toFixed(0) }}%)</span>
                  <strong>{{ money(tax) }}</strong>
                </div>
                <hr class="my-2" />
                <div class="d-flex justify-content-between fs-5">
                  <span>Total</span>
                  <strong>{{ money(total) }}</strong>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="card shadow-sm">
          <div class="card-body">
            <h5 class="card-title">Cliente</h5>
            <div class="row g-2">
              <div class="col-md-8">
                <input v-model.trim="cliente" class="form-control" placeholder="Nombre del cliente (opcional)" />
              </div>
              <div class="col-md-4">
                <input v-model.number="taxRate" type="number" min="0" step="0.01" class="form-control" placeholder="Impuesto %" />
                <div class="form-text">Ej: 0.18 para 18%</div>
              </div>
            </div>

            <div class="mt-3 d-flex gap-2">
              <button class="btn btn-secondary" :disabled="saving || !cart.length" @click="clear()">Vaciar</button>
              <button class="btn btn-primary" :disabled="saving || !cart.length" @click="submit()">
                <span v-if="saving" class="spinner-border spinner-border-sm me-2"></span>
                Confirmar venta
              </button>
              <router-link :to="{ name: 'table' }" class="btn btn-outline-secondary ms-auto">Volver</router-link>
            </div>
          </div>
        </div>

      </div>
    </div>
  </div>
</template>

<script setup lang="ts">

//ref para detectar cambios // computed recalcula funciones en el DOM ccuando hay cambios // onMouned para traer datos despues que se cargue el DOM
import { computed, onMounted, ref } from 'vue'
import { API } from '../constantes'

type Producto = {
  id: number
  nombre: string
  descripcion?: string
  precio: number
  stock: number
}

type CartItem = {
  productoId: number
  nombre: string
  precio: number
  cantidad: number
  stock: number
}

const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)

const productos = ref<Producto[]>([])
const q = ref('')
const picked = ref<Producto | null>(null)
const qty = ref(1)

const cart = ref<CartItem[]>([])
const cliente = ref('')
const taxRate = ref(0) // 0.18 = 18%

const money = (n: number) => n.toLocaleString(undefined, { style: 'currency', currency: 'DOP' })

const filtered = computed(() => {
  const s = q.value.toLowerCase()
  if (!s) return productos.value
  return productos.value.filter(p =>
    p.nombre.toLowerCase().includes(s) ||
    (p.descripcion ?? '').toLowerCase().includes(s)
  )
})

const subtotal = computed(() => cart.value.reduce((acc, it) => acc + it.precio * it.cantidad, 0))
const tax = computed(() => subtotal.value * taxRate.value)
const total = computed(() => subtotal.value + tax.value)

const select = (p: Producto) => {
  picked.value = p
  qty.value = 1
}

const add = () => {
  const p = picked.value
  if (!p) return
  if (qty.value < 1) qty.value = 1
  if (qty.value > p.stock) {
    alert(`Sin stock suficiente. Disponible: ${p.stock}`)
    qty.value = p.stock
    return
  }
  const i = cart.value.findIndex(x => x.productoId === p.id)
  if (i >= 0) {
    const next = cart.value[i].cantidad + qty.value
    if (next > p.stock) {
      alert(`Sin stock suficiente. Disponible: ${p.stock}`)
      return
    }
    cart.value[i].cantidad = next
  } else {
    cart.value.push({ productoId: p.id, nombre: p.nombre, precio: p.precio, cantidad: qty.value, stock: p.stock })
  }
  picked.value = null
  qty.value = 1
}

const fixQty = (idx: number) => {
  const it = cart.value[idx]
  if (it.cantidad < 1) it.cantidad = 1
  if (it.cantidad > it.stock) it.cantidad = it.stock
}

const removeAt = (idx: number) => cart.value.splice(idx, 1)
const clear = () => { if (confirm('¿Vaciar carrito?')) cart.value = [] }

const load = async () => {
  loading.value = true
  error.value = null
  try {
    const res = await fetch(`${API}/Products/listed`)
    if (!res.ok) throw new Error(`HTTP ${res.status}`)
    productos.value = await res.json()
  } catch (e: any) {
    error.value = e?.message ?? String(e)
  } finally {
    loading.value = false
  }
}

const submit = async () => {
  if (!cart.value.length) return
  // payload que espera tu API
  const payload = {
    cliente: cliente.value || 'Consumidor final',
    items: cart.value.map(i => ({ productoId: i.productoId, cantidad: i.cantidad }))
  }

  saving.value = true
  error.value = null
  try {
    const res = await fetch(`${API}/CreateSale/ventas`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    })
    if (!res.ok) throw new Error(`No se pudo crear la venta (HTTP ${res.status})`)
    const venta = await res.json()

    // descontar stock localmente para que el UI se actualice sin recargar
    for (const i of cart.value) {
      const p = productos.value.find(p => p.id === i.productoId)
      if (p) p.stock = Math.max(0, p.stock - i.cantidad)
    }

    alert(`Venta #${venta.id} creada. Total: ${money(venta.total)}`)
    cart.value = []
    cliente.value = ''
  } catch (e: any) {
    error.value = e?.message ?? String(e)
    alert(error.value)
  } finally {
    saving.value = false
  }
}

onMounted(load)
</script>

<style scoped>
.card { border-radius: 16px; }
.list-group-item { cursor: pointer; }
.list-group-item:hover { background: #f7f7f7; }
</style>
