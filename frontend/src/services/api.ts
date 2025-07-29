import axios, { type AxiosInstance, type AxiosResponse } from 'axios';
import type { ApiResponse, PaginatedResponse, Secret, User, SearchFilters, DashboardStats, Notification } from '@/types';
import { useAuthStore } from '@/stores/auth';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7001/api';

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json'
      }
    });

    this.setupInterceptors();
  }

  private setupInterceptors() {
    // Request interceptor pour ajouter le token
    this.client.interceptors.request.use(
      (config) => {
        const authStore = useAuthStore();
        if (authStore.token) {
          config.headers.Authorization = `Bearer ${authStore.token}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    // Response interceptor pour gérer les erreurs
    this.client.interceptors.response.use(
      (response) => response,
      async (error) => {
        const authStore = useAuthStore();
        
        if (error.response?.status === 401) {
          // Token expiré, essayer de le rafraîchir
          const refreshed = await authStore.refreshToken();
          if (!refreshed) {
            authStore.logout();
            window.location.href = '/login';
          }
        }
        
        return Promise.reject(error);
      }
    );
  }

  private async handleResponse<T>(response: AxiosResponse): Promise<ApiResponse<T>> {
    return response.data;
  }

  private async handleError(error: any): Promise<ApiResponse<any>> {
    if (error.response?.data) {
      return error.response.data;
    }
    
    return {
      success: false,
      message: error.message || 'Une erreur inattendue s\'est produite',
      errors: [error.message]
    };
  }

  async get<T>(url: string, params?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.get(url, { params });
      return this.handleResponse<T>(response);
    } catch (error) {
      return this.handleError(error);
    }
  }

  async post<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.post(url, data);
      return this.handleResponse<T>(response);
    } catch (error) {
      return this.handleError(error);
    }
  }

  async put<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.put(url, data);
      return this.handleResponse<T>(response);
    } catch (error) {
      return this.handleError(error);
    }
  }

  async delete<T>(url: string): Promise<ApiResponse<T>> {
    try {
      const response = await this.client.delete(url);
      return this.handleResponse<T>(response);
    } catch (error) {
      return this.handleError(error);
    }
  }
}

const apiClient = new ApiClient();

// API Auth
export const authApi = {
  login: (email: string, password: string) =>
    apiClient.post<{ token: string; user: User }>('/auth/login', { email, password }),
  
  register: (userData: { email: string; username: string; password: string }) =>
    apiClient.post<{ token: string; user: User }>('/auth/register', userData),
  
  refreshToken: () =>
    apiClient.post<{ token: string }>('/auth/refresh'),
  
  logout: () =>
    apiClient.post('/auth/logout'),
  
  forgotPassword: (email: string) =>
    apiClient.post('/auth/forgot-password', { email }),
  
  resetPassword: (token: string, password: string) =>
    apiClient.post('/auth/reset-password', { token, password })
};

// API Secrets
export const secretsApi = {
  getSecrets: (params: SearchFilters & { pageNumber?: number; pageSize?: number }) =>
    apiClient.get<PaginatedResponse<Secret>>('/secrets', params),
  
  getSecretById: (id: string) =>
    apiClient.get<Secret>(`/secrets/${id}`),
  
  createSecret: (secretData: Partial<Secret>) =>
    apiClient.post<Secret>('/secrets', secretData),
  
  updateSecret: (id: string, secretData: Partial<Secret>) =>
    apiClient.put<Secret>(`/secrets/${id}`, secretData),
  
  deleteSecret: (id: string) =>
    apiClient.delete<void>(`/secrets/${id}`),
  
  restoreSecret: (id: string) =>
    apiClient.post<void>(`/secrets/${id}/restore`),
  
  accessSecret: (id: string, conditionAnswers: Record<string, any>) =>
    apiClient.post<{ content: string }>(`/secrets/${id}/access`, { conditionAnswers }),
  
  getSecretHistory: (id: string) =>
    apiClient.get<any[]>(`/secrets/${id}/history`),
  
  getMySecrets: () =>
    apiClient.get<Secret[]>('/secrets/my'),
  
  getSharedWithMe: () =>
    apiClient.get<Secret[]>('/secrets/shared-with-me')
};

// API Dashboard
export const dashboardApi = {
  getStats: () =>
    apiClient.get<DashboardStats>('/dashboard/stats'),
  
  getRecentActivity: (limit = 10) =>
    apiClient.get<any[]>('/dashboard/recent-activity', { limit })
};

// API Notifications
export const notificationsApi = {
  getNotifications: (unreadOnly = false) =>
    apiClient.get<Notification[]>('/notifications', { unreadOnly }),
  
  markAsRead: (id: string) =>
    apiClient.put<void>(`/notifications/${id}/read`),
  
  markAllAsRead: () =>
    apiClient.put<void>('/notifications/read-all'),
  
  deleteNotification: (id: string) =>
    apiClient.delete<void>(`/notifications/${id}`)
};

// API Categories
export const categoriesApi = {
  getCategories: () =>
    apiClient.get<any[]>('/categories'),
  
  createCategory: (categoryData: any) =>
    apiClient.post<any>('/categories', categoryData),
  
  updateCategory: (id: string, categoryData: any) =>
    apiClient.put<any>(`/categories/${id}`, categoryData),
  
  deleteCategory: (id: string) =>
    apiClient.delete<void>(`/categories/${id}`)
};

export default apiClient;