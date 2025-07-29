export interface User {
  id: string;
  email: string;
  username: string;
  role: UserRole;
  createdAt: Date;
  lastLogin?: Date;
  isActive: boolean;
}

export enum UserRole {
  USER = 'USER',
  ADMIN = 'ADMIN',
  MODERATOR = 'MODERATOR'
}

export interface Secret {
  id: string;
  title: string;
  content: string; // Contenu chiffré
  description?: string;
  category: SecretCategory;
  tags: string[];
  createdBy: string;
  createdAt: Date;
  updatedAt: Date;
  isDeleted: boolean;
  deletedAt?: Date;
  accessCount: number;
  maxAccess?: number;
  expiresAt?: Date;
  conditions: AccessCondition[];
  isPublic: boolean;
  allowedUsers?: string[];
}

export interface SecretCategory {
  id: string;
  name: string;
  color: string;
  icon: string;
}

export interface AccessCondition {
  id: string;
  type: ConditionType;
  description: string;
  value: any;
  isRequired: boolean;
  order: number;
  logicalOperator?: LogicalOperator;
}

export enum ConditionType {
  QUESTION_ANSWER = 'QUESTION_ANSWER',
  TASK_COMPLETION = 'TASK_COMPLETION',
  TIME_BASED = 'TIME_BASED',
  GEOLOCATION = 'GEOLOCATION',
  ATTEMPT_LIMIT = 'ATTEMPT_LIMIT',
  USER_SPECIFIC = 'USER_SPECIFIC',
  DATE_RANGE = 'DATE_RANGE'
}

export enum LogicalOperator {
  AND = 'AND',
  OR = 'OR'
}

export interface AccessAttempt {
  id: string;
  secretId: string;
  userId: string;
  conditionId: string;
  attempt: any;
  isSuccessful: boolean;
  attemptedAt: Date;
  ipAddress: string;
}

export interface SecretAccess {
  id: string;
  secretId: string;
  userId: string;
  accessedAt: Date;
  ipAddress: string;
  userAgent: string;
}

export interface Notification {
  id: string;
  userId: string;
  title: string;
  message: string;
  type: NotificationType;
  isRead: boolean;
  createdAt: Date;
  actionUrl?: string;
}

export enum NotificationType {
  SECRET_ACCESSED = 'SECRET_ACCESSED',
  CONDITION_MET = 'CONDITION_MET',
  SECRET_EXPIRED = 'SECRET_EXPIRED',
  SYSTEM = 'SYSTEM'
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
  errors?: string[];
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface SearchFilters {
  query?: string;
  category?: string;
  tags?: string[];
  dateFrom?: Date;
  dateTo?: Date;
  createdBy?: string;
  hasConditions?: boolean;
  isExpired?: boolean;
}

export interface DashboardStats {
  totalSecrets: number;
  activeSecrets: number;
  expiredSecrets: number;
  totalAccesses: number;
  recentActivity: RecentActivity[];
}

export interface RecentActivity {
  id: string;
  type: 'CREATE' | 'ACCESS' | 'UPDATE' | 'DELETE';
  secretTitle: string;
  userName: string;
  timestamp: Date;
}

export interface GeolocationCondition {
  latitude: number;
  longitude: number;
  radius: number; // en mètres
  address?: string;
}

export interface QuestionAnswerCondition {
  question: string;
  answer: string;
  caseSensitive: boolean;
}

export interface TaskCompletionCondition {
  taskDescription: string;
  verificationMethod: 'MANUAL' | 'AUTOMATIC';
  verificationData?: any;
}

export interface TimeBasedCondition {
  startTime?: Date;
  endTime?: Date;
  daysOfWeek?: number[];
  timeZone?: string;
}