<template>
  <aside class="bg-body-tertiary border-end vh-100" style="width: 240px;">
    <div class="p-3">
      <!-- título / Home -->
      <RouterLink :to="{ name: 'table' }" custom v-slot="{ navigate, isActive }">
        <a
          @click="navigate"
          class="d-block mb-2 fw-semibold text-decoration-none">
          Tiendita
        </a>
      </RouterLink>

      <!-- menu -->
      <nav class="nav flex-column">
        <RouterLink :to="{ name: 'table' }" custom v-slot="{ navigate, isActive }">
          <a @click="navigate" class="nav-link px-3" :class="isActive ? 'link-active' : 'link-default'">
            Productos
          </a>
        </RouterLink>

        <RouterLink :to="{ name: 'sales' }" custom v-slot="{ navigate, isActive }">
          <a @click="navigate" class="nav-link px-3" :class="isActive ? 'link-active' : 'link-default'">
            Ventas
          </a>
        </RouterLink>

        <RouterLink :to="{ name: 'customers' }" custom v-slot="{ navigate, isActive }">
          <a @click="navigate" class="nav-link px-3" :class="isActive ? 'link-active' : 'link-default'">
            Clientes
          </a>
        </RouterLink>

        <RouterLink :to="{ name: 'add-stock' }" custom v-slot="{ navigate, isActive }">
          <a @click="navigate" class="nav-link px-0 mt-3" :class="isActive ? 'link-active' : 'link-default'">
            ➕ Agregar producto
          </a>
        </RouterLink>
        
        <RouterLink :to="{ name: 'sell' }" custom v-slot="{ navigate, isActive }">
          <a @click="navigate" class="nav-link px-0 mt-3" :class="isActive ? 'link-active' : 'link-default'">
            🏪 Vender producto
          </a>
        </RouterLink>
      </nav>
    </div>
  </aside>
</template>

<script setup lang="ts">
</script>

<style scoped>
  .link-default { color: #343a40; }
  .link-active  { color: #000; background: #e9ecef; border-radius: .5rem; padding: .25rem .5rem; }
</style>
