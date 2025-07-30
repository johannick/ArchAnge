<template>
  <div class="landing-page">
    <!-- Hero Section -->
    <section class="hero">
      <div class="hero-content">
        <div class="logo-container">
          <img 
            :src="logoSrc" 
            alt="SecretVault Logo" 
            class="logo"
          />
        </div>
        
        <h1 class="hero-title">
          Bienvenue sur <span class="brand">SecretVault</span>
        </h1>
        
        <p class="hero-description">
          La plateforme sécurisée pour partager vos informations sensibles avec des mécanismes 
          de contrôle d'accès avancés. Protégez vos secrets avec des conditions personnalisées 
          et un chiffrement de niveau militaire.
        </p>
        
        <div class="hero-actions">
          <router-link to="/register" class="cta-primary">
            <Shield class="w-5 h-5" />
            Commencer gratuitement
          </router-link>
          
          <router-link to="/login" class="cta-secondary">
            <LogIn class="w-5 h-5" />
            Se connecter
          </router-link>
        </div>
        
        <div class="hero-stats">
          <div class="stat">
            <div class="stat-number">256-bit</div>
            <div class="stat-label">Chiffrement AES</div>
          </div>
          <div class="stat">
            <div class="stat-number">100%</div>
            <div class="stat-label">Sécurisé</div>
          </div>
          <div class="stat">
            <div class="stat-number">24/7</div>
            <div class="stat-label">Disponible</div>
          </div>
        </div>
      </div>
      
      <div class="hero-visual">
        <div class="security-grid">
          <div class="security-card" v-for="i in 9" :key="i">
            <Lock class="w-6 h-6" />
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features">
      <div class="container">
        <h2 class="section-title">Fonctionnalités avancées</h2>
        <p class="section-description">
          Des outils puissants pour protéger et contrôler l'accès à vos informations sensibles
        </p>
        
        <div class="features-grid">
          <div class="feature-card">
            <div class="feature-icon">
              <Shield class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Chiffrement militaire</h3>
            <p class="feature-description">
              Vos secrets sont protégés par un chiffrement AES-256, 
              le même standard utilisé par les gouvernements.
            </p>
          </div>
          
          <div class="feature-card">
            <div class="feature-icon">
              <Key class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Conditions d'accès</h3>
            <p class="feature-description">
              Définissez des conditions personnalisées : questions, géolocalisation, 
              horaires, ou tâches à accomplir.
            </p>
          </div>
          
          <div class="feature-card">
            <div class="feature-icon">
              <Clock class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Expiration automatique</h3>
            <p class="feature-description">
              Configurez une date d'expiration pour que vos secrets 
              se détruisent automatiquement.
            </p>
          </div>
          
          <div class="feature-card">
            <div class="feature-icon">
              <Eye class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Audit complet</h3>
            <p class="feature-description">
              Suivez qui accède à vos secrets, quand et depuis où 
              avec un journal d'audit détaillé.
            </p>
          </div>
          
          <div class="feature-card">
            <div class="feature-icon">
              <Users class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Partage sécurisé</h3>
            <p class="feature-description">
              Partagez vos secrets avec des utilisateurs spécifiques 
              ou rendez-les publics avec conditions.
            </p>
          </div>
          
          <div class="feature-card">
            <div class="feature-icon">
              <Smartphone class="w-8 h-8" />
            </div>
            <h3 class="feature-title">Multi-plateforme</h3>
            <p class="feature-description">
              Accédez à vos secrets depuis n'importe quel appareil 
              avec notre interface responsive.
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- How it works -->
    <section class="how-it-works">
      <div class="container">
        <h2 class="section-title">Comment ça marche</h2>
        
        <div class="steps">
          <div class="step">
            <div class="step-number">1</div>
            <div class="step-content">
              <h3>Créez votre secret</h3>
              <p>Saisissez votre information sensible et choisissez une catégorie</p>
            </div>
          </div>
          
          <div class="step">
            <div class="step-number">2</div>
            <div class="step-content">
              <h3>Définissez les conditions</h3>
              <p>Configurez qui peut accéder et sous quelles conditions</p>
            </div>
          </div>
          
          <div class="step">
            <div class="step-number">3</div>
            <div class="step-content">
              <h3>Partagez en sécurité</h3>
              <p>Votre secret est chiffré et accessible uniquement selon vos règles</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section">
      <div class="container">
        <div class="cta-content">
          <h2>Prêt à sécuriser vos secrets ?</h2>
          <p>Rejoignez des milliers d'utilisateurs qui font confiance à SecretVault</p>
          <router-link to="/register" class="cta-button">
            Créer un compte gratuit
          </router-link>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer class="footer">
      <div class="container">
        <div class="footer-content">
          <div class="footer-brand">
            <img :src="logoSrc" alt="SecretVault" class="footer-logo" />
            <p>La sécurité de vos secrets, notre priorité.</p>
          </div>
          
          <div class="footer-links">
            <div class="link-group">
              <h4>Produit</h4>
              <a href="#features">Fonctionnalités</a>
              <a href="#security">Sécurité</a>
              <a href="#pricing">Tarifs</a>
            </div>
            
            <div class="link-group">
              <h4>Support</h4>
              <a href="#help">Aide</a>
              <a href="#contact">Contact</a>
              <a href="#docs">Documentation</a>
            </div>
            
            <div class="link-group">
              <h4>Légal</h4>
              <a href="#privacy">Confidentialité</a>
              <a href="#terms">Conditions</a>
              <a href="#security">Sécurité</a>
            </div>
          </div>
        </div>
        
        <div class="footer-bottom">
          <p>&copy; 2024 SecretVault. Tous droits réservés.</p>
          <button @click="toggleTheme" class="theme-toggle">
            <Sun v-if="isDarkMode" class="w-4 h-4" />
            <Moon v-else class="w-4 h-4" />
          </button>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import {
  Shield,
  LogIn,
  Lock,
  Key,
  Clock,
  Eye,
  Users,
  Smartphone,
  Sun,
  Moon
} from 'lucide-vue-next';

// Theme management
const isDarkMode = ref(localStorage.getItem('theme') === 'dark');

const logoSrc = computed(() => {
  return isDarkMode.value 
    ? '/src/assets/layer/AWhite.png'
    : '/src/assets/layer/A.png';
});

const toggleTheme = () => {
  isDarkMode.value = !isDarkMode.value;
  localStorage.setItem('theme', isDarkMode.value ? 'dark' : 'light');
  document.documentElement.classList.toggle('dark', isDarkMode.value);
};

onMounted(() => {
  document.documentElement.classList.toggle('dark', isDarkMode.value);
});
</script>

<style scoped>
.landing-page {
  @apply min-h-screen bg-white dark:bg-gray-900;
}

/* Hero Section */
.hero {
  @apply min-h-screen flex items-center justify-between px-6 lg:px-12 max-w-7xl mx-auto;
}

.hero-content {
  @apply flex-1 max-w-2xl;
}

.logo-container {
  @apply mb-8;
}

.logo {
  @apply h-16 w-auto;
}

.hero-title {
  @apply text-4xl lg:text-6xl font-bold text-gray-900 dark:text-white mb-6 leading-tight;
}

.brand {
  @apply text-blue-600 dark:text-blue-400;
}

.hero-description {
  @apply text-xl text-gray-600 dark:text-gray-300 mb-8 leading-relaxed;
}

.hero-actions {
  @apply flex flex-col sm:flex-row gap-4 mb-12;
}

.cta-primary {
  @apply flex items-center justify-center gap-3 px-8 py-4 bg-blue-600 text-white rounded-xl font-semibold hover:bg-blue-700 transition-all duration-200 shadow-lg hover:shadow-xl;
}

.cta-secondary {
  @apply flex items-center justify-center gap-3 px-8 py-4 border-2 border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 rounded-xl font-semibold hover:border-blue-600 hover:text-blue-600 dark:hover:border-blue-400 dark:hover:text-blue-400 transition-all duration-200;
}

.hero-stats {
  @apply flex gap-8;
}

.stat {
  @apply text-center;
}

.stat-number {
  @apply text-2xl font-bold text-blue-600 dark:text-blue-400;
}

.stat-label {
  @apply text-sm text-gray-600 dark:text-gray-400;
}

.hero-visual {
  @apply hidden lg:flex flex-1 justify-center items-center;
}

.security-grid {
  @apply grid grid-cols-3 gap-4 p-8;
}

.security-card {
  @apply w-16 h-16 bg-blue-50 dark:bg-blue-900/20 rounded-xl flex items-center justify-center text-blue-600 dark:text-blue-400 hover:scale-110 transition-transform duration-200;
}

/* Features Section */
.features {
  @apply py-20 bg-gray-50 dark:bg-gray-800;
}

.container {
  @apply max-w-7xl mx-auto px-6 lg:px-12;
}

.section-title {
  @apply text-3xl lg:text-4xl font-bold text-center text-gray-900 dark:text-white mb-4;
}

.section-description {
  @apply text-xl text-center text-gray-600 dark:text-gray-300 mb-16 max-w-3xl mx-auto;
}

.features-grid {
  @apply grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8;
}

.feature-card {
  @apply bg-white dark:bg-gray-900 p-8 rounded-2xl shadow-sm hover:shadow-lg transition-shadow duration-200;
}

.feature-icon {
  @apply w-16 h-16 bg-blue-100 dark:bg-blue-900/20 rounded-xl flex items-center justify-center text-blue-600 dark:text-blue-400 mb-6;
}

.feature-title {
  @apply text-xl font-semibold text-gray-900 dark:text-white mb-4;
}

.feature-description {
  @apply text-gray-600 dark:text-gray-300 leading-relaxed;
}

/* How it works */
.how-it-works {
  @apply py-20;
}

.steps {
  @apply max-w-4xl mx-auto;
}

.step {
  @apply flex items-start gap-6 mb-12 last:mb-0;
}

.step-number {
  @apply w-12 h-12 bg-blue-600 text-white rounded-full flex items-center justify-center font-bold text-lg flex-shrink-0;
}

.step-content h3 {
  @apply text-xl font-semibold text-gray-900 dark:text-white mb-2;
}

.step-content p {
  @apply text-gray-600 dark:text-gray-300;
}

/* CTA Section */
.cta-section {
  @apply py-20 bg-blue-600 dark:bg-blue-700;
}

.cta-content {
  @apply text-center;
}

.cta-content h2 {
  @apply text-3xl lg:text-4xl font-bold text-white mb-4;
}

.cta-content p {
  @apply text-xl text-blue-100 mb-8;
}

.cta-button {
  @apply inline-block px-8 py-4 bg-white text-blue-600 rounded-xl font-semibold hover:bg-gray-100 transition-colors duration-200 shadow-lg;
}

/* Footer */
.footer {
  @apply bg-gray-900 dark:bg-gray-950 text-white py-16;
}

.footer-content {
  @apply flex flex-col lg:flex-row justify-between gap-12;
}

.footer-brand {
  @apply max-w-sm;
}

.footer-logo {
  @apply h-8 w-auto mb-4;
}

.footer-brand p {
  @apply text-gray-400;
}

.footer-links {
  @apply flex flex-col sm:flex-row gap-8;
}

.link-group h4 {
  @apply font-semibold mb-4;
}

.link-group a {
  @apply block text-gray-400 hover:text-white transition-colors duration-200 mb-2;
}

.footer-bottom {
  @apply flex flex-col sm:flex-row justify-between items-center pt-8 mt-8 border-t border-gray-800;
}

.footer-bottom p {
  @apply text-gray-400;
}

.theme-toggle {
  @apply p-2 text-gray-400 hover:text-white transition-colors duration-200;
}

/* Responsive */
@media (max-width: 1024px) {
  .hero {
    @apply flex-col text-center gap-12;
  }
  
  .hero-visual {
    @apply flex;
  }
}
</style>