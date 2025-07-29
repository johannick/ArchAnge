<template>
  <div class="secret-card" :class="{ 'expired': isExpired, 'deleted': secret.isDeleted }">
    <div class="secret-header">
      <div class="secret-category" :style="{ backgroundColor: secret.category.color }">
        <component :is="getCategoryIcon(secret.category.icon)" class="w-4 h-4" />
      </div>
      <div class="secret-actions">
        <button
          v-if="canEdit"
          @click="$emit('edit', secret)"
          class="action-btn edit"
          title="Modifier"
        >
          <Edit2 class="w-4 h-4" />
        </button>
        <button
          v-if="canDelete && !secret.isDeleted"
          @click="$emit('delete', secret)"
          class="action-btn delete"
          title="Supprimer"
        >
          <Trash2 class="w-4 h-4" />
        </button>
        <button
          v-if="secret.isDeleted && canRestore"
          @click="$emit('restore', secret)"
          class="action-btn restore"
          title="Restaurer"
        >
          <RotateCcw class="w-4 h-4" />
        </button>
      </div>
    </div>

    <div class="secret-content">
      <h3 class="secret-title">{{ secret.title }}</h3>
      <p v-if="secret.description" class="secret-description">
        {{ secret.description }}
      </p>

      <div class="secret-tags" v-if="secret.tags.length">
        <span
          v-for="tag in secret.tags"
          :key="tag"
          class="tag"
        >
          {{ tag }}
        </span>
      </div>

      <div class="secret-conditions" v-if="secret.conditions.length">
        <div class="conditions-header">
          <Lock class="w-4 h-4" />
          <span>{{ secret.conditions.length }} condition(s)</span>
        </div>
        <div class="conditions-list">
          <div
            v-for="condition in secret.conditions.slice(0, 2)"
            :key="condition.id"
            class="condition-item"
          >
            <component :is="getConditionIcon(condition.type)" class="w-3 h-3" />
            <span>{{ condition.description }}</span>
          </div>
          <div v-if="secret.conditions.length > 2" class="condition-more">
            +{{ secret.conditions.length - 2 }} autres
          </div>
        </div>
      </div>
    </div>

    <div class="secret-footer">
      <div class="secret-meta">
        <div class="meta-item">
          <Eye class="w-4 h-4" />
          <span>{{ secret.accessCount }}</span>
        </div>
        <div class="meta-item">
          <Calendar class="w-4 h-4" />
          <span>{{ formatDate(secret.createdAt) }}</span>
        </div>
        <div v-if="secret.expiresAt" class="meta-item" :class="{ 'expired': isExpired }">
          <Clock class="w-4 h-4" />
          <span>{{ formatDate(secret.expiresAt) }}</span>
        </div>
      </div>

      <button
        @click="$emit('access', secret)"
        class="access-btn"
        :disabled="secret.isDeleted || (secret.maxAccess && secret.accessCount >= secret.maxAccess)"
      >
        <Key class="w-4 h-4" />
        Accéder
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import {
  Edit2,
  Trash2,
  RotateCcw,
  Lock,
  Eye,
  Calendar,
  Clock,
  Key,
  HelpCircle,
  MapPin,
  Timer,
  User,
  CheckSquare,
  Shield
} from 'lucide-vue-next';
import type { Secret, ConditionType } from '@/types';
import { useAuthStore } from '@/stores/auth';

interface Props {
  secret: Secret;
}

const props = defineProps<Props>();
const emit = defineEmits<{
  edit: [secret: Secret];
  delete: [secret: Secret];
  restore: [secret: Secret];
  access: [secret: Secret];
}>();

const authStore = useAuthStore();

const isExpired = computed(() => {
  return props.secret.expiresAt && new Date(props.secret.expiresAt) < new Date();
});

const canEdit = computed(() => {
  return props.secret.createdBy === authStore.user?.id || authStore.isAdmin;
});

const canDelete = computed(() => {
  return props.secret.createdBy === authStore.user?.id || authStore.isAdmin;
});

const canRestore = computed(() => {
  return props.secret.createdBy === authStore.user?.id || authStore.isAdmin;
});

const getCategoryIcon = (iconName: string) => {
  const icons: Record<string, any> = {
    shield: Shield,
    lock: Lock,
    key: Key,
    user: User
  };
  return icons[iconName] || Shield;
};

const getConditionIcon = (type: ConditionType) => {
  const icons: Record<ConditionType, any> = {
    QUESTION_ANSWER: HelpCircle,
    GEOLOCATION: MapPin,
    TIME_BASED: Timer,
    USER_SPECIFIC: User,
    TASK_COMPLETION: CheckSquare,
    ATTEMPT_LIMIT: Shield,
    DATE_RANGE: Calendar
  };
  return icons[type] || Lock;
};

const formatDate = (date: Date | string) => {
  return new Date(date).toLocaleDateString('fr-FR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
};
</script>

<style scoped>
.secret-card {
  @apply bg-white rounded-lg shadow-md border border-gray-200 overflow-hidden transition-all duration-200 hover:shadow-lg hover:border-gray-300;
}

.secret-card.expired {
  @apply border-red-200 bg-red-50;
}

.secret-card.deleted {
  @apply opacity-60 border-gray-300 bg-gray-50;
}

.secret-header {
  @apply flex items-center justify-between p-4 pb-2;
}

.secret-category {
  @apply flex items-center justify-center w-8 h-8 rounded-full text-white;
}

.secret-actions {
  @apply flex items-center gap-1;
}

.action-btn {
  @apply p-1.5 rounded-md transition-colors duration-200;
}

.action-btn.edit {
  @apply text-gray-500 hover:text-blue-600 hover:bg-blue-50;
}

.action-btn.delete {
  @apply text-gray-500 hover:text-red-600 hover:bg-red-50;
}

.action-btn.restore {
  @apply text-gray-500 hover:text-green-600 hover:bg-green-50;
}

.secret-content {
  @apply px-4 pb-2;
}

.secret-title {
  @apply text-lg font-semibold text-gray-900 mb-2;
}

.secret-description {
  @apply text-sm text-gray-600 mb-3 line-clamp-2;
}

.secret-tags {
  @apply flex flex-wrap gap-1 mb-3;
}

.tag {
  @apply px-2 py-1 bg-gray-100 text-gray-700 text-xs rounded-full;
}

.secret-conditions {
  @apply mb-3;
}

.conditions-header {
  @apply flex items-center gap-2 text-sm font-medium text-gray-700 mb-2;
}

.conditions-list {
  @apply space-y-1;
}

.condition-item {
  @apply flex items-center gap-2 text-xs text-gray-600;
}

.condition-more {
  @apply text-xs text-gray-500 italic;
}

.secret-footer {
  @apply flex items-center justify-between p-4 pt-2 border-t border-gray-100;
}

.secret-meta {
  @apply flex items-center gap-4;
}

.meta-item {
  @apply flex items-center gap-1 text-xs text-gray-500;
}

.meta-item.expired {
  @apply text-red-500;
}

.access-btn {
  @apply flex items-center gap-2 px-3 py-1.5 bg-blue-600 text-white text-sm rounded-md hover:bg-blue-700 transition-colors duration-200 disabled:opacity-50 disabled:cursor-not-allowed;
}
</style>