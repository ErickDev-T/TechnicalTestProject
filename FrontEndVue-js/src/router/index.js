import { createRouter, createWebHistory } from 'vue-router';

// Vistas (ajusta las rutas de import si tus archivos están en otra carpeta)
import StockView from '../components/StockView.vue';
import AddStockView from '../components/AddStockView.vue';
import SalesView from '../components/SalesView.vue';
import CustomerView from '../components/CustomerView.vue';
import Table from '../components/Table.vue'
import Edit from '../components/EditStockView.vue'
import Sell from '../components/SellProductView.vue'

const routes = [
  { path: '/', name: 'stock', component: StockView },
  { path: '/add-stock', name: 'add-stock', component: AddStockView },
  { path: '/sales', name: 'sales', component: SalesView },
  { path: '/customers', name: 'customers', component: CustomerView },
  { path: '/table', name: 'table', component: Table},
  { path: '/edit/:id', name: 'edit', component: Edit, props: true },
   { path: '/sell', name: 'sell', component: Sell},
];

export const router = createRouter({
  history: createWebHistory(), // usa hash si hospedas en estático sin fallback: createWebHashHistory()
  routes,
});
