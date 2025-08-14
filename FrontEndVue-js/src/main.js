import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'
import { router } from './router/index.js'
import "bootstrap-icons/font/bootstrap-icons.css";


createApp(App)
  .use(router)
  .mount('#app')
