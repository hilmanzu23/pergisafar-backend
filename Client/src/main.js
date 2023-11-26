import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import "vuetify/dist/vuetify.min.css";
import Vue3EasyDataTable from 'vue3-easy-data-table';
import 'vue3-easy-data-table/dist/style.css';
import axios from 'axios'
import 'sweetalert2/src/sweetalert2.scss'
import '@mdi/font/css/materialdesignicons.css'

const vuetify = createVuetify({
  components,
  directives,
})
const BASEAPI = import.meta.env.VITE_APP_BASEAPI
const token = localStorage.getItem('access_token')
  ? localStorage.getItem('access_token')
  : null
axios.defaults.baseURL = BASEAPI
axios.interceptors.request.use((config) => {
  if (token != null) {
    config.headers['Authorization'] = `Bearer ${token}`
  }
  return config
})
const app = createApp(App)

app.use(router)
app.component('EasyDataTable', Vue3EasyDataTable);
app.use(vuetify)
app.mount('#app')
