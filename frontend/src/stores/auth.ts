import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { User, ApiResponse } from '@/types';
import { authApi } from '@/services/api';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  sub: string;
  email: string;
  username: string;
  role: string;
  exp: number;
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null);
  const token = ref<string | null>(localStorage.getItem('token'));
  const isLoading = ref(false);

  const isAuthenticated = computed(() => !!token.value && !!user.value);
  const isAdmin = computed(() => user.value?.role === 'ADMIN');
  const isModerator = computed(() => user.value?.role === 'MODERATOR' || isAdmin.value);

  const setToken = (newToken: string) => {
    token.value = newToken;
    localStorage.setItem('token', newToken);
    
    try {
      const decoded = jwtDecode<JwtPayload>(newToken);
      user.value = {
        id: decoded.sub,
        email: decoded.email,
        username: decoded.username,
        role: decoded.role as any,
        createdAt: new Date(),
        isActive: true
      };
    } catch (error) {
      console.error('Erreur lors du décodage du token:', error);
      logout();
    }
  };

  const login = async (email: string, password: string): Promise<ApiResponse<any>> => {
    isLoading.value = true;
    try {
      const response = await authApi.login(email, password);
      if (response.success && response.data?.token) {
        setToken(response.data.token);
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const register = async (userData: {
    email: string;
    username: string;
    password: string;
  }): Promise<ApiResponse<any>> => {
    isLoading.value = true;
    try {
      const response = await authApi.register(userData);
      if (response.success && response.data?.token) {
        setToken(response.data.token);
      }
      return response;
    } finally {
      isLoading.value = false;
    }
  };

  const logout = () => {
    user.value = null;
    token.value = null;
    localStorage.removeItem('token');
  };

  const refreshToken = async (): Promise<boolean> => {
    try {
      const response = await authApi.refreshToken();
      if (response.success && response.data?.token) {
        setToken(response.data.token);
        return true;
      }
      return false;
    } catch (error) {
      logout();
      return false;
    }
  };

  const checkTokenExpiry = () => {
    if (!token.value) return false;
    
    try {
      const decoded = jwtDecode<JwtPayload>(token.value);
      const currentTime = Date.now() / 1000;
      
      if (decoded.exp < currentTime) {
        logout();
        return false;
      }
      
      // Rafraîchir le token s'il expire dans moins de 5 minutes
      if (decoded.exp - currentTime < 300) {
        refreshToken();
      }
      
      return true;
    } catch (error) {
      logout();
      return false;
    }
  };

  // Initialiser l'utilisateur si un token existe
  if (token.value) {
    checkTokenExpiry();
  }

  return {
    user: readonly(user),
    token: readonly(token),
    isLoading: readonly(isLoading),
    isAuthenticated,
    isAdmin,
    isModerator,
    login,
    register,
    logout,
    refreshToken,
    checkTokenExpiry
  };
}, {
  persist: {
    paths: ['token']
  }
});