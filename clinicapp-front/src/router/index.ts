import { createRouter, createWebHistory, type NavigationGuardNextCallback, type RouteLocationNormalizedGeneric } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import UserListView from '@/views/UserListView.vue'
import { claimNames, useAuthStore } from '@/stores/authStore'
import AddUserView from '@/views/AddUserView.vue'
import PatientListView from '@/views/PatientListView.vue'
import PatientView from '@/views/PatientView.vue'
import DrugClassListView from '@/views/DrugClassListView.vue'
import DrugListView from '@/views/DrugListView.vue'

const routeGuard = (claim: string) => (to: RouteLocationNormalizedGeneric, from: RouteLocationNormalizedGeneric) => {
  const authStore = useAuthStore()

  if(authStore.userData?.claims.includes(claimNames.admin)) {
    return true;
  }

  if(!authStore.userData || !authStore.userData.claims.includes(claim)) {
    return { name: 'login' }
  }

  return true;
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/users',
      name: 'users',
      component: UserListView,
      beforeEnter: routeGuard(claimNames.admin)
    },
    {
      path: '/users/add',
      name: 'add-user',
      component: AddUserView,
      beforeEnter: routeGuard(claimNames.admin)
    },
    {
      path: "/patient",
      name: 'patients',
      component: PatientListView,
      beforeEnter: routeGuard(claimNames.receptionist)
    },
    {
      path: '/patient/:id',
      name: "patient",
      component: PatientView,
      beforeEnter: routeGuard(claimNames.receptionist)
    },
    {
      path: '/drug-class',
      name: 'drugClass',
      component: DrugClassListView,
      beforeEnter: routeGuard(claimNames.receptionist),
    },
    {
      path: '/drug',
      name: 'drug',
      component: DrugListView,
      beforeEnter: routeGuard(claimNames.receptionist)
    }
  ],
})
export default router
