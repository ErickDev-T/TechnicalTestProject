
<template>
  <nav class="navbar bg-body-tertiary">
    <div class="container-fluid">
       <RouterLink :to="{ name: 'table' }" class="me-2" active-class="activo">Tiendita</RouterLink>

      <div>
        <RouterLink :to="{ name: 'sales' }" class="me-2" active-class="activo">Ventas</RouterLink>
        <RouterLink :to="{ name: 'customers' }" active-class="activo">Clientes</RouterLink>
      </div>
    </div>
  </nav>

  

</template>

<script setup lang="ts">

</script>

<style scoped>
.activo{
  color: black;
}
</style>