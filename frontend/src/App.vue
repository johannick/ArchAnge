<template>
  <div id="app" :class="{ 'dark': isDarkMode }">
    <div v-if="isAuthenticated && $route.meta.requiresAuth" class="app-layout">
      <!-- Navigation -->
      <nav class="sidebar">
        <div class="sidebar-header">
          <div class="logo">
            <Shield class="w-8 h-8 text-blue-600" />
            <span class="logo-text">SecretVault</span>
          </div>
        </div>

        <div class="sidebar-menu">
          <router-link to="/dashboard" class="menu-item" active-class="active">
            <LayoutDashboard class="w-5 h-5" />
            <span>Tableau de bord</span>
          </router-link>

          <router-link to="/secrets" class="menu-item" active-class="active">
            <Key class="w-5 h-5" />
            <span>Mes secrets</span>
          </router-link>

          <router-link to="/secrets/shared" class="menu-item" active-class="active">
            <Share2 class="w-5 h-5" />
            <span>Partagés avec moi</span>
          </router-link>

          <router-link to="/categories" class="menu-item" active-class="active">
            <Folder class="w-5 h-5" />
            <span>Catégories</span>
          </router-link>

          <router-link to="/notifications" class="menu-item" active-class="active">
            <Bell class="w-5 h-5" />
            <span>Notifications</span>
            <span v-if="unreadNotifications > 0" class="notification-badge">
              {{ unreadNotifications }}
            </span>
          </router-link>

          <div class="menu-divider"></div>

          <router-link v-if="authStore.isAdmin" to="/admin" class="menu-item" active-class="active">
            <Settings class="w-5 h-5" />
            <span>Administration</span>
          </router-link>

          <router-link to="/profile" class="menu-item" active-class="active">
            <User class="w-5 h-5" />
            <span>Profil</span>
          </router-link>
        </div>

        <div class="sidebar-footer">
          <div class="theme-toggle">
            <button @click="toggleTheme" class="theme-btn">
              <Sun v-if="isDarkMode" class="w-4 h-4" />
              <Moon v-else class="w-4 h-4" />
            </button>
          </div>

          <div class="user-menu">
            <div class="user-info">
              <div class="user-avatar">
                {{ authStore.user?.username?.charAt(0).toUpperCase() }}
              </div>
              <div class="user-details">
                <div class="user-name">{{ authStore.user?.username }}</div>
                <div class="user-email">{{ authStore.user?.email }}</div>
              </div>
            </div>
            <button @click="handleLogout" class="logout-btn">
              <LogOut class="w-4 h-4" />
            </button>
          </div>
        </div>
      </nav>

      <!-- Contenu principal -->
      <main class="main-content">
        <router-view />
      </main>
    </div>

    <!-- Pages publiques et d'authentification -->
    <div v-else>
      <router-view />
    </div>

    <!-- Notifications toast -->
    <div class="toast-container">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="['toast', toast.type]"
      >
        <component :is="getToastIcon(toast.type)" class="w-5 h-5" />
        <div class="toast-content">
          <div class="toast-title">{{ toast.title }}</div>
          <div class="toast-message">{{ toast.message }}</div>
        </div>
        <button @click="removeToast(toast.id)" class="toast-close">
          <X class="w-4 h-4" />
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import {
  Shield,
  LayoutDashboard,
  Key,
  Share2,
  Folder,
  Bell,
  Settings,
  User,
  Sun,
  Moon,
  LogOut,
  X,
  CheckCircle,
  AlertCircle,
  AlertTriangle,
  Info
} from 'lucide-vue-next';
import { useAuthStore } from '@/stores/auth';
import { notificationsApi } from '@/services/api';

const router = useRouter();
const authStore = useAuthStore();

// État local
const isDarkMode = ref(localStorage.getItem('theme') === 'dark');
const unreadNotifications = ref(0);
const toasts = ref<Array<{
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message: string;
}>>([]);

// Computed
const isAuthenticated = computed(() => authStore.isAuthenticated);

// Méthodes
const toggleTheme = () => {
  isDarkMode.value = !isDarkMode.value;
  localStorage.setItem('theme', isDarkMode.value ? 'dark' : 'light');
  document.documentElement.classList.toggle('dark', isDarkMode.value);
};

const handleLogout = async () => {
  if (confirm('Êtes-vous sûr de vouloir vous déconnecter ?')) {
    authStore.logout();
    router.push('/login');
  }
};

const loadUnreadNotifications = async () => {
  try {
    const response = await notificationsApi.getNotifications(true);
    if (response.success) {
      unreadNotifications.value = response.data?.length || 0;
    }
  } catch (error) {
    console.error('Erreur lors du chargement des notifications:', error);
  }
};

const showToast = (type: 'success' | 'error' | 'warning' | 'info', title: string, message: string) => {
  const id = Math.random().toString(36).substr(2, 9);
  toasts.value.push({ id, type, title, message });
  
  setTimeout(() => {
    removeToast(id);
  }, 5000);
};

const removeToast = (id: string) => {
  const index = toasts.value.findIndex(toast => toast.id === id);
  if (index > -1) {
    toasts.value.splice(index, 1);
  }
};

const getToastIcon = (type: string) => {
  const icons = {
    success: CheckCircle,
    error: AlertCircle,
    warning: AlertTriangle,
    info: Info
  };
  return icons[type as keyof typeof icons] || Info;
};

// Watchers
watch(isAuthenticated, (newValue) => {
  if (newValue) {
    loadUnreadNotifications();
  }
});

// Lifecycle
onMounted(() => {
  // Appliquer le thème au chargement
  document.documentElement.classList.toggle('dark', isDarkMode.value);
  
  // Vérifier l'authentification
  if (isAuthenticated.value) {
    authStore.checkTokenExpiry();
    loadUnreadNotifications();
  }
});

// Exposer la méthode showToast globalement
window.showToast = showToast;
</script>

<style scoped>
.app-layout {
  @apply flex min-h-screen bg-gray-50 dark:bg-gray-900;
}

.sidebar {
  @apply w-64 bg-white dark:bg-gray-800 border-r border-gray-200 dark:border-gray-700 flex flex-col;
}

.sidebar-header {
  @apply p-6 border-b border-gray-200 dark:border-gray-700;
}

.logo {
  @apply flex items-center gap-3;
}

.logo-text {
  @apply text-xl font-bold text-gray-900 dark:text-white;
}

.sidebar-menu {
  @apply flex-1 p-4 space-y-2;
}

.menu-item {
  @apply flex items-center gap-3 px-3 py-2 text-gray-700 dark:text-gray-300 rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors duration-200 relative;
}

.menu-item.active {
  @apply bg-blue-50 dark:bg-blue-900/20 text-blue-600 dark:text-blue-400;
}

.menu-divider {
  @apply h-px bg-gray-200 dark:bg-gray-700 my-4;
}

.notification-badge {
  @apply absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center;
}

.sidebar-footer {
  @apply p-4 border-t border-gray-200 dark:border-gray-700 space-y-4;
}

.theme-toggle {
  @apply flex justify-center;
}

.theme-btn {
  @apply p-2 text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-200 transition-colors duration-200;
}

.user-menu {
  @apply flex items-center gap-3;
}

.user-info {
  @apply flex items-center gap-3 flex-1;
}

.user-avatar {
  @apply w-8 h-8 bg-blue-600 text-white rounded-full flex items-center justify-center text-sm font-medium;
}

.user-details {
  @apply flex-1 min-w-0;
}

.user-name {
  @apply text-sm font-medium text-gray-900 dark:text-white truncate;
}

.user-email {
  @apply text-xs text-gray-500 dark:text-gray-400 truncate;
}

.logout-btn {
  @apply p-2 text-gray-500 dark:text-gray-400 hover:text-red-600 dark:hover:text-red-400 transition-colors duration-200;
}

.main-content {
  @apply flex-1 overflow-auto;
}

.auth-layout {
  @apply min-h-screen bg-gray-50 dark:bg-gray-900;
}

.toast-container {
  @apply fixed top-4 right-4 z-50 space-y-2;
}

.toast {
  @apply flex items-start gap-3 p-4 bg-white dark:bg-gray-800 border rounded-lg shadow-lg max-w-sm;
}

.toast.success {
  @apply border-green-200 dark:border-green-800;
}

.toast.error {
  @apply border-red-200 dark:border-red-800;
}

.toast.warning {
  @apply border-yellow-200 dark:border-yellow-800;
}

.toast.info {
  @apply border-blue-200 dark:border-blue-800;
}

.toast-content {
  @apply flex-1;
}

.toast-title {
  @apply font-medium text-gray-900 dark:text-white;
}

.toast-message {
  @apply text-sm text-gray-600 dark:text-gray-300 mt-1;
}

.toast-close {
  @apply text-gray-400 hover:text-gray-600 dark:hover:text-gray-200 transition-colors duration-200;
}

/* Animations */
.toast {
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
    opacity: 0;
  }
  to {
    transform: translateX(0);
    opacity: 1;
  }
}
</style>