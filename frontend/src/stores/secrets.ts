import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Secret, SearchFilters, PaginatedResponse, ApiResponse } from '@/types';
import { secretsApi } from '@/services/api';

export const useSecretsStore = defineStore('secrets', () => {
  const secrets = ref<Secret[]>([]);
  const currentSecret = ref<Secret | null>(null);
  const isLoading = ref(false);
  const searchFilters = ref<SearchFilters>({});
  const pagination = ref({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0
  });

  const filteredSecrets = computed(() => {
    let filtered = secrets.value;

    if (searchFilters.value.query) {
      const query = searchFilters.value.query.toLowerCase();
      filtered = filtered.filter(secret => 
        secret.title.toLowerCase().includes(query) ||
        secret.description?.toLowerCase().includes(query) ||
        secret.tags.some(tag => tag.toLowerCase().includes(query))
      );
    }

    if (searchFilters.value.category) {
      filtered = filtered.filter(secret => 
        secret.category.id === searchFilters.value.category
      );
    }

    if (searchFilters.value.tags?.length) {
      filtered = filtered.filter(secret =>
        searchFilters.value.tags!.some(tag => secret.tags.includes(tag))
      );
    }

    if (searchFilters.value.hasConditions !== undefined) {
      filtered = filtered.filter(secret =>
        searchFilters.value.hasConditions 
          ? secret.conditions.length > 0
          : secret.conditions.length === 0
      );
    }

    if (searchFilters.value.isExpired !== undefined) {
      const now = new Date();
      filtered = filtered.filter(secret => {
        const isExpired = secret.expiresAt && secret.expiresAt < now;
        return searchFilters.value.isExpired ? isExpired : !isExpired;
      });
    }

    return filtered;
  });

  const activeSecrets = computed(() => 
    secrets.value.filter(secret => !secret.isDeleted)
  );

  const expiredSecrets = computed(() => {
    const now = new Date();
    return secrets.value.filter(secret => 
      secret.expiresAt && secret.expiresAt < now
    );
  });

  const fetchSecrets = async (filters?: SearchFilters, page = 1, pageSize = 10) => {
    isLoading.value = true;
    try {
      const response = await secretsApi.getSecrets({
        ...filters,
        pageNumber: page,
        pageSize
      });

      if (response.success && response.data) {
        secrets.value = response.data.items;
        pagination.value = {
          pageNumber: response.data.pageNumber,
          pageSize: response.data.pageSize,
          totalCount: response.data.totalCount,
          totalPages: response.data.totalPages
        };
      }
    } finally {
      isLoading.value = false;
    }
  };

  const fetchSecretById = async (id: string): Promise<Secret | null> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.getSecretById(id);
      if (response.success && response.data) {
        currentSecret.value = response.data;
        return response.data;
      }
      return null;
    } finally {
      isLoading.value = false;
    }
  };

  const createSecret = async (secretData: Partial<Secret>): Promise<ApiResponse<Secret>> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.createSecret(secretData);
      if (response.success && response.data) {
        secrets.value.unshift(response.data);
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const updateSecret = async (id: string, secretData: Partial<Secret>): Promise<ApiResponse<Secret>> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.updateSecret(id, secretData);
      if (response.success && response.data) {
        const index = secrets.value.findIndex(s => s.id === id);
        if (index !== -1) {
          secrets.value[index] = response.data;
        }
        if (currentSecret.value?.id === id) {
          currentSecret.value = response.data;
        }
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const deleteSecret = async (id: string): Promise<ApiResponse<void>> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.deleteSecret(id);
      if (response.success) {
        const index = secrets.value.findIndex(s => s.id === id);
        if (index !== -1) {
          secrets.value[index].isDeleted = true;
          secrets.value[index].deletedAt = new Date();
        }
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const restoreSecret = async (id: string): Promise<ApiResponse<void>> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.restoreSecret(id);
      if (response.success) {
        const index = secrets.value.findIndex(s => s.id === id);
        if (index !== -1) {
          secrets.value[index].isDeleted = false;
          secrets.value[index].deletedAt = undefined;
        }
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const accessSecret = async (id: string, conditionAnswers: Record<string, any>): Promise<ApiResponse<{ content: string }>> => {
    isLoading.value = true;
    try {
      const response = await secretsApi.accessSecret(id, conditionAnswers);
      if (response.success) {
        // Incrémenter le compteur d'accès
        const secret = secrets.value.find(s => s.id === id);
        if (secret) {
          secret.accessCount++;
        }
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const setSearchFilters = (filters: SearchFilters) => {
    searchFilters.value = { ...filters };
  };

  const clearSearchFilters = () => {
    searchFilters.value = {};
  };

  return {
    secrets: readonly(secrets),
    currentSecret: readonly(currentSecret),
    isLoading: readonly(isLoading),
    searchFilters: readonly(searchFilters),
    pagination: readonly(pagination),
    filteredSecrets,
    activeSecrets,
    expiredSecrets,
    fetchSecrets,
    fetchSecretById,
    createSecret,
    updateSecret,
    deleteSecret,
    restoreSecret,
    accessSecret,
    setSearchFilters,
    clearSearchFilters
  };
});