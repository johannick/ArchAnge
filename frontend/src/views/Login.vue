<template>
  <div class="auth-page">
    <div class="auth-container">
      <div class="auth-header">
        <img :src="logoSrc" alt="SecretVault" class="auth-logo" />
        <h1 class="auth-title">Connexion</h1>
        <p class="auth-subtitle">Accédez à vos secrets sécurisés</p>
      </div>

      <form @submit.prevent="handleLogin" class="auth-form">
        <div class="form-group">
          <label for="email" class="form-label">Email</label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            required
            class="form-input"
            :class="{ 'error': errors.email }"
            placeholder="votre@email.com"
          />
          <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
        </div>

        <div class="form-group">
          <label for="password" class="form-label">Mot de passe</label>
          <div class="password-input">
            <input
              id="password"
              v-model="form.password"
              :type="showPassword ? 'text' : 'password'"
              required
              class="form-input"
              :class="{ 'error': errors.password }"
              placeholder="••••••••"
            />
            <button
              type="button"
              @click="showPassword = !showPassword"
              class="password-toggle"
            >
              <Eye v-if="showPassword" class="w-4 h-4" />
              <EyeOff v-else class="w-4 h-4" />
            </button>
          </div>
          <span v-if="errors.password" class="error-message">{{ errors.password }}</span>
        </div>

        <div class="form-options">
          <label class="checkbox-label">
            <input v-model="form.rememberMe" type="checkbox" class="checkbox" />
            <span>Se souvenir de moi</span>
          </label>
          <router-link to="/forgot-password" class="forgot-link">
            Mot de passe oublié ?
          </router-link>
        </div>

        <button
          type="submit"
          :disabled="isLoading"
          class="submit-btn"
        >
          <Loader v-if="isLoading" class="w-4 h-4 animate-spin" />
          <LogIn v-else class="w-4 h-4" />
          {{ isLoading ? 'Connexion...' : 'Se connecter' }}
        </button>

        <div v-if="errorMessage" class="error-banner">
          <AlertCircle class="w-4 h-4" />
          {{ errorMessage }}
        </div>
      </form>

      <div class="auth-footer">
        <p>Pas encore de compte ?</p>
        <router-link to="/register" class="auth-link">
          Créer un compte
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { LogIn, Eye, EyeOff, Loader, AlertCircle } from 'lucide-vue-next';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const authStore = useAuthStore();

// Theme management
const isDarkMode = ref(localStorage.getItem('theme') === 'dark');

const logoSrc = computed(() => {
  return isDarkMode.value 
    ? '/src/assets/layer/AWhite.png'
    : '/src/assets/layer/A.png';
});

// Form state
const form = ref({
  email: '',
  password: '',
  rememberMe: false
});

const errors = ref<Record<string, string>>({});
const errorMessage = ref('');
const showPassword = ref(false);
const isLoading = ref(false);

// Methods
const validateForm = () => {
  errors.value = {};
  
  if (!form.value.email) {
    errors.value.email = 'L\'email est requis';
  } else if (!/\S+@\S+\.\S+/.test(form.value.email)) {
    errors.value.email = 'Format d\'email invalide';
  }
  
  if (!form.value.password) {
    errors.value.password = 'Le mot de passe est requis';
  } else if (form.value.password.length < 6) {
    errors.value.password = 'Le mot de passe doit contenir au moins 6 caractères';
  }
  
  return Object.keys(errors.value).length === 0;
};

const handleLogin = async () => {
  if (!validateForm()) return;
  
  isLoading.value = true;
  errorMessage.value = '';
  
  try {
    const response = await authStore.login(form.value.email, form.value.password);
    
    if (response.success) {
      router.push('/dashboard');
    } else {
      errorMessage.value = response.message || 'Erreur de connexion';
    }
  } catch (error) {
    errorMessage.value = 'Une erreur inattendue s\'est produite';
  } finally {
    isLoading.value = false;
  }
};

onMounted(() => {
  document.documentElement.classList.toggle('dark', isDarkMode.value);
});
</script>

<style scoped>
.auth-page {
  @apply min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center p-4;
}

.auth-container {
  @apply w-full max-w-md bg-white dark:bg-gray-800 rounded-2xl shadow-xl p-8;
}

.auth-header {
  @apply text-center mb-8;
}

.auth-logo {
  @apply h-12 w-auto mx-auto mb-6;
}

.auth-title {
  @apply text-2xl font-bold text-gray-900 dark:text-white mb-2;
}

.auth-subtitle {
  @apply text-gray-600 dark:text-gray-300;
}

.auth-form {
  @apply space-y-6;
}

.form-group {
  @apply space-y-2;
}

.form-label {
  @apply block text-sm font-medium text-gray-700 dark:text-gray-300;
}

.form-input {
  @apply w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white transition-colors duration-200;
}

.form-input.error {
  @apply border-red-500 focus:ring-red-500;
}

.password-input {
  @apply relative;
}

.password-toggle {
  @apply absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600 dark:hover:text-gray-300;
}

.error-message {
  @apply text-sm text-red-600 dark:text-red-400;
}

.form-options {
  @apply flex items-center justify-between;
}

.checkbox-label {
  @apply flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300;
}

.checkbox {
  @apply rounded border-gray-300 dark:border-gray-600 text-blue-600 focus:ring-blue-500;
}

.forgot-link {
  @apply text-sm text-blue-600 dark:text-blue-400 hover:underline;
}

.submit-btn {
  @apply w-full flex items-center justify-center gap-2 px-4 py-3 bg-blue-600 text-white rounded-lg font-medium hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200;
}

.error-banner {
  @apply flex items-center gap-2 p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg text-red-700 dark:text-red-400 text-sm;
}

.auth-footer {
  @apply text-center mt-8 pt-6 border-t border-gray-200 dark:border-gray-700;
}

.auth-footer p {
  @apply text-gray-600 dark:text-gray-300 mb-2;
}

.auth-link {
  @apply text-blue-600 dark:text-blue-400 font-medium hover:underline;
}
</style>