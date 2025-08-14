<script setup lang="ts">
import { RouterView } from 'vue-router'
import SideNav from './components/NavBar.vue'
</script>

<template>
  <div class="d-flex min-vh-100">
    <!-- Sidebar -->
    <SideNav />

    <!-- Contenido -->
    <main class="flex-grow-1 p-3">
      <RouterView />
    </main>
  </div>
</template>
