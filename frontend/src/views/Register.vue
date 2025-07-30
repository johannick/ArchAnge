<template>
  <div class="auth-page">
    <div class="auth-container">
      <div class="auth-header">
        <img :src="logoSrc" alt="SecretVault" class="auth-logo" />
        <h1 class="auth-title">Créer un compte</h1>
        <p class="auth-subtitle">Rejoignez SecretVault et sécurisez vos secrets</p>
      </div>

      <form @submit.prevent="handleRegister" class="auth-form">
        <div class="form-group">
          <label for="username" class="form-label">Nom d'utilisateur</label>
          <input
            id="username"
            v-model="form.username"
            type="text"
            required
            class="form-input"
            :class="{ 'error': errors.username }"
            placeholder="votre_nom_utilisateur"
          />
          <span v-if="errors.username" class="error-message">{{ errors.username }}</span>
        </div>

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

        <div class="form-group">
          <label for="confirmPassword" class="form-label">Confirmer le mot de passe</label>
          <div class="password-input">
            <input
              id="confirmPassword"
              v-model="form.confirmPassword"
              :type="showConfirmPassword ? 'text' : 'password'"
              required
              class="form-input"
              :class="{ 'error': errors.confirmPassword }"
              placeholder="••••••••"
            />
            <button
              type="button"
              @click="showConfirmPassword = !showConfirmPassword"
              class="password-toggle"
            >
              <Eye v-if="showConfirmPassword" class="w-4 h-4" />
              <EyeOff v-else class="w-4 h-4" />
            </button>
          </div>
          <span v-if="errors.confirmPassword" class="error-message">{{ errors.confirmPassword }}</span>
        </div>

        <div class="form-group">
          <label class="checkbox-label">
            <input v-model="form.acceptTerms" type="checkbox" class="checkbox" required />
            <span>
              J'accepte les 
              <a href="#" class="terms-link">conditions d'utilisation</a>
              et la 
              <a href="#" class="terms-link">politique de confidentialité</a>
            </span>
          </label>
          <span v-if="errors.acceptTerms" class="error-message">{{ errors.acceptTerms }}</span>
        </div>

        <button
          type="submit"
          :disabled="isLoading"
          class="submit-btn"
        >
          <Loader v-if="isLoading" class="w-4 h-4 animate-spin" />
          <UserPlus v-else class="w-4 h-4" />
          {{ isLoading ? 'Création...' : 'Créer mon compte' }}
        </button>

        <div v-if="errorMessage" class="error-banner">
          <AlertCircle class="w-4 h-4" />
          {{ errorMessage }}
        </div>
      </form>

      <div class="auth-footer">
        <p>Déjà un compte ?</p>
        <router-link to="/login" class="auth-link">
          Se connecter
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { UserPlus, Eye, EyeOff, Loader, AlertCircle } from 'lucide-vue-next';
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
  username: '',
  email: '',
  password: '',
  confirmPassword: '',
  acceptTerms: false
});

const errors = ref<Record<string, string>>({});
const errorMessage = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const isLoading = ref(false);

// Methods
const validateForm = () => {
  errors.value = {};
  
  if (!form.value.username) {
    errors.value.username = 'Le nom d\'utilisateur est requis';
  } else if (form.value.username.length < 3) {
    errors.value.username = 'Le nom d\'utilisateur doit contenir au moins 3 caractères';
  }
  
  if (!form.value.email) {
    errors.value.email = 'L\'email est requis';
  } else if (!/\S+@\S+\.\S+/.test(form.value.email)) {
    errors.value.email = 'Format d\'email invalide';
  }
  
  if (!form.value.password) {
    errors.value.password = 'Le mot de passe est requis';
  } else if (form.value.password.length < 8) {
    errors.value.password = 'Le mot de passe doit contenir au moins 8 caractères';
  } else if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/.test(form.value.password)) {
    errors.value.password = 'Le mot de passe doit contenir au moins une minuscule, une majuscule et un chiffre';
  }
  
  if (!form.value.confirmPassword) {
    errors.value.confirmPassword = 'La confirmation du mot de passe est requise';
  } else if (form.value.password !== form.value.confirmPassword) {
    errors.value.confirmPassword = 'Les mots de passe ne correspondent pas';
  }
  
  if (!form.value.acceptTerms) {
    errors.value.acceptTerms = 'Vous devez accepter les conditions d\'utilisation';
  }
  
  return Object.keys(errors.value).length === 0;
};

const handleRegister = async () => {
  if (!validateForm()) return;
  
  isLoading.value = true;
  errorMessage.value = '';
  
  try {
    const response = await authStore.register({
      username: form.value.username,
      email: form.value.email,
      password: form.value.password
    });
    
    if (response.success) {
      router.push('/dashboard');
    } else {
      errorMessage.value = response.message || 'Erreur lors de la création du compte';
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

.checkbox-label {
  @apply flex items-start gap-2 text-sm text-gray-600 dark:text-gray-300;
}

.checkbox {
  @apply mt-0.5 rounded border-gray-300 dark:border-gray-600 text-blue-600 focus:ring-blue-500;
}

.terms-link {
  @apply text-blue-600 dark:text-blue-400 hover:underline;
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