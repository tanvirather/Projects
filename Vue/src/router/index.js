import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/account', meta: { requiresAuth: false },
      children: [
        { path: '/account/login', component: () => import('../views/account/Login.vue') },
        { path: '/account/register', component: () => import('../views/account/Register.vue') },
        { path: '', redirect: '/account/register' }
      ]
    },

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
  const isAuthenticated = false
  const requiresAuth = to.meta.requiresAuth
  if (requiresAuth && !isAuthenticated) {
    next('/account/register')
  } else {
    next()
  }
})

export default router
