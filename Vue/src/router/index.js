import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/login', component: () => import('../views/Login.vue') },
    { path: '/register', component: () => import('../views/Register.vue') },
    {
      path: '/', component: () => import('../views/Index.vue'), meta: { requiresAuth: true },
      children: [
        { path: 'dashboard', component: () => import('../views/Dashboard.vue') },
        { path: 'numericType', component: () => import('../views/NumericType.vue') },
        { path: 'numericType/:id', component: () => import('../views/NumericTypeDetail.vue') },
        { path: '', redirect: 'dashboard' }
      ]
    },
    { path: '/:pathMatch(.*)*', redirect: '/' } // Catch-all route (must be LAST)
  ],
})

router.beforeEach((to, from, next) => {
  // const isAuthenticated = !!localStorage.getItem('authToken')
  const isAuthenticated = true
  const requiresAuth = to.meta.requiresAuth
  if (requiresAuth && !isAuthenticated) {
    next('/login')
  } else {
    next()
  }
})

export default router
