<template>
  <div class="dashboard">
    <div class="dashboard-header">
      <h1 class="dashboard-title">Tableau de bord</h1>
      <div class="dashboard-actions">
        <button @click="refreshData" class="refresh-btn" :disabled="isLoading">
          <RotateCw class="w-4 h-4" :class="{ 'animate-spin': isLoading }" />
          Actualiser
        </button>
        <router-link to="/secrets/create" class="create-btn">
          <Plus class="w-4 h-4" />
          Nouveau secret
        </router-link>
      </div>
    </div>

    <!-- Statistiques -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon total">
          <Shield class="w-6 h-6" />
        </div>
        <div class="stat-content">
          <div class="stat-value">{{ stats?.totalSecrets || 0 }}</div>
          <div class="stat-label">Secrets totaux</div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon active">
          <CheckCircle class="w-6 h-6" />
        </div>
        <div class="stat-content">
          <div class="stat-value">{{ stats?.activeSecrets || 0 }}</div>
          <div class="stat-label">Secrets actifs</div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon expired">
          <Clock class="w-6 h-6" />
        </div>
        <div class="stat-content">
          <div class="stat-value">{{ stats?.expiredSecrets || 0 }}</div>
          <div class="stat-label">Secrets expirés</div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon access">
          <Eye class="w-6 h-6" />
        </div>
        <div class="stat-content">
          <div class="stat-value">{{ stats?.totalAccesses || 0 }}</div>
          <div class="stat-label">Accès totaux</div>
        </div>
      </div>
    </div>

    <div class="dashboard-content">
      <!-- Recherche et filtres -->
      <div class="search-section">
        <div class="search-bar">
          <Search class="w-5 h-5 text-gray-400" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Rechercher des secrets..."
            class="search-input"
            @input="handleSearch"
          />
        </div>
        
        <div class="filters">
          <select v-model="selectedCategory" @change="handleFilterChange" class="filter-select">
            <option value="">Toutes les catégories</option>
            <option v-for="category in categories" :key="category.id" :value="category.id">
              {{ category.name }}
            </option>
          </select>

          <select v-model="selectedStatus" @change="handleFilterChange" class="filter-select">
            <option value="">Tous les statuts</option>
            <option value="active">Actifs</option>
            <option value="expired">Expirés</option>
            <option value="deleted">Supprimés</option>
          </select>

          <button @click="clearFilters" class="clear-filters-btn">
            <X class="w-4 h-4" />
            Effacer
          </button>
        </div>
      </div>

      <!-- Liste des secrets -->
      <div class="secrets-section">
        <div class="section-header">
          <h2 class="section-title">Mes secrets</h2>
          <div class="view-toggle">
            <button
              @click="viewMode = 'grid'"
              :class="['view-btn', { active: viewMode === 'grid' }]"
            >
              <Grid3X3 class="w-4 h-4" />
            </button>
            <button
              @click="viewMode = 'list'"
              :class="['view-btn', { active: viewMode === 'list' }]"
            >
              <List class="w-4 h-4" />
            </button>
          </div>
        </div>

        <div v-if="isLoading" class="loading-state">
          <div class="loading-spinner"></div>
          <p>Chargement des secrets...</p>
        </div>

        <div v-else-if="filteredSecrets.length === 0" class="empty-state">
          <Shield class="w-16 h-16 text-gray-300" />
          <h3>Aucun secret trouvé</h3>
          <p>Commencez par créer votre premier secret sécurisé.</p>
          <router-link to="/secrets/create" class="create-first-btn">
            Créer un secret
          </router-link>
        </div>

        <div v-else :class="['secrets-grid', viewMode]">
          <SecretCard
            v-for="secret in paginatedSecrets"
            :key="secret.id"
            :secret="secret"
            @edit="handleEditSecret"
            @delete="handleDeleteSecret"
            @restore="handleRestoreSecret"
            @access="handleAccessSecret"
          />
        </div>

        <!-- Pagination -->
        <div v-if="totalPages > 1" class="pagination">
          <button
            @click="currentPage--"
            :disabled="currentPage === 1"
            class="pagination-btn"
          >
            <ChevronLeft class="w-4 h-4" />
          </button>
          
          <div class="pagination-info">
            Page {{ currentPage }} sur {{ totalPages }}
          </div>
          
          <button
            @click="currentPage++"
            :disabled="currentPage === totalPages"
            class="pagination-btn"
          >
            <ChevronRight class="w-4 h-4" />
          </button>
        </div>
      </div>

      <!-- Activité récente -->
      <div class="activity-section">
        <h2 class="section-title">Activité récente</h2>
        <div class="activity-list">
          <div
            v-for="activity in recentActivity"
            :key="activity.id"
            class="activity-item"
          >
            <div class="activity-icon" :class="activity.type.toLowerCase()">
              <component :is="getActivityIcon(activity.type)" class="w-4 h-4" />
            </div>
            <div class="activity-content">
              <div class="activity-description">
                <strong>{{ activity.userName }}</strong>
                {{ getActivityDescription(activity.type) }}
                <strong>{{ activity.secretTitle }}</strong>
              </div>
              <div class="activity-time">
                {{ formatRelativeTime(activity.timestamp) }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import {
  Shield,
  CheckCircle,
  Clock,
  Eye,
  Plus,
  RotateCw,
  Search,
  X,
  Grid3X3,
  List,
  ChevronLeft,
  ChevronRight,
  FileText,
  Trash2,
  Edit,
  Key
} from 'lucide-vue-next';
import SecretCard from '@/components/SecretCard.vue';
import { useSecretsStore } from '@/stores/secrets';
import { dashboardApi } from '@/services/api';
import type { DashboardStats, RecentActivity, SecretCategory, Secret } from '@/types';

const secretsStore = useSecretsStore();

// État local
const isLoading = ref(false);
const stats = ref<DashboardStats | null>(null);
const recentActivity = ref<RecentActivity[]>([]);
const categories = ref<SecretCategory[]>([]);

// Recherche et filtres
const searchQuery = ref('');
const selectedCategory = ref('');
const selectedStatus = ref('');

// Vue et pagination
const viewMode = ref<'grid' | 'list'>('grid');
const currentPage = ref(1);
const itemsPerPage = 12;

// Données calculées
const filteredSecrets = computed(() => {
  let secrets = secretsStore.secrets;

  // Filtre par recherche
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase();
    secrets = secrets.filter(secret =>
      secret.title.toLowerCase().includes(query) ||
      secret.description?.toLowerCase().includes(query) ||
      secret.tags.some(tag => tag.toLowerCase().includes(query))
    );
  }

  // Filtre par catégorie
  if (selectedCategory.value) {
    secrets = secrets.filter(secret => secret.category.id === selectedCategory.value);
  }

  // Filtre par statut
  if (selectedStatus.value) {
    const now = new Date();
    switch (selectedStatus.value) {
      case 'active':
        secrets = secrets.filter(secret => !secret.isDeleted && (!secret.expiresAt || secret.expiresAt > now));
        break;
      case 'expired':
        secrets = secrets.filter(secret => secret.expiresAt && secret.expiresAt <= now);
        break;
      case 'deleted':
        secrets = secrets.filter(secret => secret.isDeleted);
        break;
    }
  }

  return secrets;
});

const totalPages = computed(() => Math.ceil(filteredSecrets.value.length / itemsPerPage));

const paginatedSecrets = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage;
  const end = start + itemsPerPage;
  return filteredSecrets.value.slice(start, end);
});

// Méthodes
const refreshData = async () => {
  isLoading.value = true;
  try {
    await Promise.all([
      loadStats(),
      loadRecentActivity(),
      secretsStore.fetchSecrets()
    ]);
  } finally {
    isLoading.value = false;
  }
};

const loadStats = async () => {
  const response = await dashboardApi.getStats();
  if (response.success) {
    stats.value = response.data!;
  }
};

const loadRecentActivity = async () => {
  const response = await dashboardApi.getRecentActivity(10);
  if (response.success) {
    recentActivity.value = response.data!;
  }
};

const handleSearch = () => {
  currentPage.value = 1;
};

const handleFilterChange = () => {
  currentPage.value = 1;
};

const clearFilters = () => {
  searchQuery.value = '';
  selectedCategory.value = '';
  selectedStatus.value = '';
  currentPage.value = 1;
};

const handleEditSecret = (secret: Secret) => {
  // Navigation vers la page d'édition
  // router.push(`/secrets/${secret.id}/edit`);
};

const handleDeleteSecret = async (secret: Secret) => {
  if (confirm('Êtes-vous sûr de vouloir supprimer ce secret ?')) {
    await secretsStore.deleteSecret(secret.id);
  }
};

const handleRestoreSecret = async (secret: Secret) => {
  await secretsStore.restoreSecret(secret.id);
};

const handleAccessSecret = (secret: Secret) => {
  // Navigation vers la page d'accès
  // router.push(`/secrets/${secret.id}/access`);
};

const getActivityIcon = (type: string) => {
  const icons: Record<string, any> = {
    CREATE: FileText,
    ACCESS: Key,
    UPDATE: Edit,
    DELETE: Trash2
  };
  return icons[type] || FileText;
};

const getActivityDescription = (type: string) => {
  const descriptions: Record<string, string> = {
    CREATE: 'a créé le secret',
    ACCESS: 'a accédé au secret',
    UPDATE: 'a modifié le secret',
    DELETE: 'a supprimé le secret'
  };
  return descriptions[type] || 'a interagi avec le secret';
};

const formatRelativeTime = (date: Date) => {
  const now = new Date();
  const diff = now.getTime() - new Date(date).getTime();
  const minutes = Math.floor(diff / 60000);
  const hours = Math.floor(minutes / 60);
  const days = Math.floor(hours / 24);

  if (days > 0) return `il y a ${days} jour${days > 1 ? 's' : ''}`;
  if (hours > 0) return `il y a ${hours} heure${hours > 1 ? 's' : ''}`;
  if (minutes > 0) return `il y a ${minutes} minute${minutes > 1 ? 's' : ''}`;
  return 'à l\'instant';
};

// Watchers
watch([searchQuery, selectedCategory, selectedStatus], () => {
  currentPage.value = 1;
});

// Lifecycle
onMounted(() => {
  refreshData();
});
</script>

<style scoped>
.dashboard {
  @apply min-h-screen bg-gray-50 p-6;
}

.dashboard-header {
  @apply flex items-center justify-between mb-8;
}

.dashboard-title {
  @apply text-3xl font-bold text-gray-900;
}

.dashboard-actions {
  @apply flex items-center gap-3;
}

.refresh-btn {
  @apply flex items-center gap-2 px-4 py-2 text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors duration-200;
}

.create-btn {
  @apply flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200;
}

.stats-grid {
  @apply grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8;
}

.stat-card {
  @apply bg-white p-6 rounded-lg shadow-sm border border-gray-200 flex items-center gap-4;
}

.stat-icon {
  @apply p-3 rounded-full;
}

.stat-icon.total {
  @apply bg-blue-100 text-blue-600;
}

.stat-icon.active {
  @apply bg-green-100 text-green-600;
}

.stat-icon.expired {
  @apply bg-red-100 text-red-600;
}

.stat-icon.access {
  @apply bg-purple-100 text-purple-600;
}

.stat-value {
  @apply text-2xl font-bold text-gray-900;
}

.stat-label {
  @apply text-sm text-gray-600;
}

.dashboard-content {
  @apply space-y-8;
}

.search-section {
  @apply bg-white p-6 rounded-lg shadow-sm border border-gray-200;
}

.search-bar {
  @apply relative mb-4;
}

.search-bar svg {
  @apply absolute left-3 top-1/2 transform -translate-y-1/2;
}

.search-input {
  @apply w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent;
}

.filters {
  @apply flex items-center gap-4 flex-wrap;
}

.filter-select {
  @apply px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent;
}

.clear-filters-btn {
  @apply flex items-center gap-2 px-3 py-2 text-gray-600 hover:text-gray-800 transition-colors duration-200;
}

.secrets-section {
  @apply bg-white p-6 rounded-lg shadow-sm border border-gray-200;
}

.section-header {
  @apply flex items-center justify-between mb-6;
}

.section-title {
  @apply text-xl font-semibold text-gray-900;
}

.view-toggle {
  @apply flex items-center bg-gray-100 rounded-lg p-1;
}

.view-btn {
  @apply p-2 rounded-md transition-colors duration-200;
}

.view-btn.active {
  @apply bg-white text-blue-600 shadow-sm;
}

.loading-state {
  @apply flex flex-col items-center justify-center py-12 text-gray-500;
}

.loading-spinner {
  @apply w-8 h-8 border-4 border-gray-200 border-t-blue-600 rounded-full animate-spin mb-4;
}

.empty-state {
  @apply flex flex-col items-center justify-center py-12 text-center;
}

.empty-state h3 {
  @apply text-lg font-medium text-gray-900 mt-4 mb-2;
}

.empty-state p {
  @apply text-gray-600 mb-6;
}

.create-first-btn {
  @apply px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200;
}

.secrets-grid {
  @apply grid gap-6;
}

.secrets-grid.grid {
  @apply grid-cols-1 md:grid-cols-2 lg:grid-cols-3;
}

.secrets-grid.list {
  @apply grid-cols-1;
}

.pagination {
  @apply flex items-center justify-center gap-4 mt-8;
}

.pagination-btn {
  @apply p-2 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed;
}

.pagination-info {
  @apply text-sm text-gray-600;
}

.activity-section {
  @apply bg-white p-6 rounded-lg shadow-sm border border-gray-200;
}

.activity-list {
  @apply space-y-4;
}

.activity-item {
  @apply flex items-start gap-3;
}

.activity-icon {
  @apply p-2 rounded-full;
}

.activity-icon.create {
  @apply bg-green-100 text-green-600;
}

.activity-icon.access {
  @apply bg-blue-100 text-blue-600;
}

.activity-icon.update {
  @apply bg-yellow-100 text-yellow-600;
}

.activity-icon.delete {
  @apply bg-red-100 text-red-600;
}

.activity-content {
  @apply flex-1;
}

.activity-description {
  @apply text-sm text-gray-900;
}

.activity-time {
  @apply text-xs text-gray-500 mt-1;
}
</style>