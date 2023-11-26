import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import MasterView from '@views/master/MasterView.vue'
import LoginView from '@views/auth/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      children:[
        {
          path: '/Master',
          name: 'Master',
          component: MasterView
        },
      ]
    },
    {
      path: '/Login',
      name: 'Login',
      component: LoginView,
    },
  ]
})

export default router
