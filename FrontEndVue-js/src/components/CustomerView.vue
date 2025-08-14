<template>
  <div class="bg-light p-4 rounded shadow-sm">
    <h4 class="mb-3 text-primary">
      <i class="bi bi-people me-2"></i> Lista de Usuarios
    </h4>

    <div class="d-flex gap-2 mb-3">
      <input v-model.trim="q" class="form-control" placeholder="Buscar por nombre, apellido o correo…" />
      <button class="btn btn-outline-secondary" @click="loadUsuarios" :disabled="loading">
        <i class="bi bi-arrow-clockwise me-1"></i> Actualizar
      </button>
    </div>

    <div v-if="loading" class="alert alert-info py-2 mb-3">Cargando usuarios…</div>
    <div v-if="error" class="alert alert-danger py-2 mb-3">{{ error }}</div>

    <div class="table-responsive">
      <table class="table table-striped table-hover align-middle text-center">
        <thead class="table-dark">
          <tr>
            <th>ID</th>
            <th class="text-start">Nombre</th>
            <th class="text-start">Correo</th>
            <th>Usuario</th>
            <th>Creado</th>
            <th>Detalles</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="u in filtered" :key="u.id">
            <td>{{ u.id }}</td>
            <td class="text-start">{{ u.nombres }} {{ u.apellidos }}</td>
            <td class="text-start">{{ u.correo }}</td>
            <td><span class="badge bg-secondary">{{ u.username }}</span></td>
            <td>{{ formatDateTime(u.fechaCreacion) }}</td>
            <td>
              <button class="btn btn-sm btn-outline-primary" @click="verDetalles(u)">
                <i class="bi bi-eye"></i>
              </button>
            </td>
          </tr>
          <tr v-if="!loading && !filtered.length"> <!--solo carga si  no hay nada en el loadUsuarios>loading es falso y si la lista de busccar es 0 -->
            <td colspan="6" class="text-muted py-3">No hay usuarios.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
  //ref para detectar cambios // computed actualiza el DOM ccuando hay cambios // onMouned para traer datos despues que se cargue el DOM
  import { ref, computed, onMounted } from 'vue'
  import Swal from 'sweetalert2'
  import 'sweetalert2/dist/sweetalert2.min.css'
  import { API } from '../constantes'

  interface IUsuario {
    id: number
    nombres: string
    apellidos: string
    correo: string
    username: string
    fechaCreacion: string
  }

  const usuarios = ref<IUsuario[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const q = ref('')

  //cambiar el formato de la fecha  default: 2025-08-13 21:53:38.587
  const formatDateTime = (s: string) =>
    new Date(s).toLocaleString('es-DO', {
      year: 'numeric', month: 'short', day: '2-digit',
      hour: '2-digit', minute: '2-digit'
    })
//filtar usuario... buscador // computed actualiza cada que cambie el DOM
  const filtered = computed(() => {
    if (!q.value) return usuarios.value // si no hay nada retorna todo
    const needle = q.value.toLowerCase()  //convierte a minusculas
    return usuarios.value.filter(u =>       
      (u.nombres + ' ' + u.apellidos).toLowerCase().includes(needle) ||
      u.correo.toLowerCase().includes(needle) ||
      u.username.toLowerCase().includes(needle)
    )
  })
  // mostrar la sweet alerta con los datos
  const verDetalles = async (u: IUsuario) => {
    await Swal.fire({
      title: `Usuario #${u.id}`,
      html: `
        <div class="text-start">
          <p class="mb-1"><strong>Nombre:</strong> ${u.nombres} ${u.apellidos}</p>
          <p class="mb-1"><strong>Correo:</strong> ${u.correo}</p>
          <p class="mb-1"><strong>Usuario:</strong> ${u.username}</p>
          <p class="mb-1"><strong>Fecha creación:</strong> ${formatDateTime(u.fechaCreacion)}</p>
        </div>
      `,
      icon: 'info',
      width: 600,
      confirmButtonText: 'Cerrar'
    })
  }
// cargar los usuarios desde la API
  const loadUsuarios = async () => {
    loading.value = true
    error.value = null
    try {
      const res = await fetch(`${API}/Usuarios/listed`)
      if (!res.ok) throw new Error(`HTTP ${res.status}`)
      usuarios.value = await res.json() as IUsuario[]
    } catch (e: any) {
      error.value = e?.message ?? String(e)
      await Swal.fire({ icon: 'error', title: 'Error', text: error.value })
    } finally {
      loading.value = false
    }
  }
//cuando se monte todo el DOM carda los usuarios
  onMounted(loadUsuarios)

</script>
