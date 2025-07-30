import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

// Import views
import Landing from '@/views/Landing.vue';
import Login from '@/views/Login.vue';
import Register from '@/views/Register.vue';
import Dashboard from '@/views/Dashboard.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'Landing',
      component: Landing,
      meta: { requiresGuest: true }
    },
    {
      path: '/login',
      name: 'Login',
      component: Login,
      meta: { requiresGuest: true }
    },
    {
      path: '/register',
      name: 'Register',
      component: Register,
      meta: { requiresGuest: true }
    },
    {
      path: '/dashboard',
      name: 'Dashboard',
      component: Dashboard,
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets',
      name: 'Secrets',
      component: () => import('@/views/Secrets.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets/create',
      name: 'CreateSecret',
      component: () => import('@/views/CreateSecret.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets/:id',
      name: 'SecretDetail',
      component: () => import('@/views/SecretDetail.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets/:id/edit',
      name: 'EditSecret',
      component: () => import('@/views/EditSecret.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets/:id/access',
      name: 'AccessSecret',
      component: () => import('@/views/AccessSecret.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/secrets/shared',
      name: 'SharedSecrets',
      component: () => import('@/views/SharedSecrets.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/categories',
      name: 'Categories',
      component: () => import('@/views/Categories.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/notifications',
      name: 'Notifications',
      component: () => import('@/views/Notifications.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/profile',
      name: 'Profile',
      component: () => import('@/views/Profile.vue'),
      meta: { requiresAuth: true }
    },
    {
      path: '/admin',
      name: 'Admin',
      component: () => import('@/views/Admin.vue'),
      meta: { requiresAuth: true, requiresAdmin: true }
    },
    {
      path: '/forgot-password',
      name: 'ForgotPassword',
      component: () => import('@/views/ForgotPassword.vue'),
      meta: { requiresGuest: true }
    },
    {
      path: '/reset-password',
      name: 'ResetPassword',
      component: () => import('@/views/ResetPassword.vue'),
      meta: { requiresGuest: true }
    },
    // Catch all route - must be last
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('@/views/NotFound.vue')
    }
  ]
});

// Navigation guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  
  // Check if route requires authentication
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login');
    return;
  }
  
  // Check if route requires guest (not authenticated)
  if (to.meta.requiresGuest && authStore.isAuthenticated) {
    next('/dashboard');
    return;
  }
  
  // Check if route requires admin
  if (to.meta.requiresAdmin && !authStore.isAdmin) {
    next('/dashboard');
    return;
  }
  
  // Check token expiry for authenticated routes
  if (to.meta.requiresAuth && authStore.isAuthenticated) {
    const isValid = authStore.checkTokenExpiry();
    if (!isValid) {
      next('/login');
      return;
    }
  }
  
  next();
});

export default router;