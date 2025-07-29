import CryptoJS from 'crypto-js';

const ENCRYPTION_KEY = import.meta.env.VITE_ENCRYPTION_KEY || 'default-key-change-in-production';

export class EncryptionService {
  private static instance: EncryptionService;
  private key: string;

  private constructor() {
    this.key = ENCRYPTION_KEY;
  }

  public static getInstance(): EncryptionService {
    if (!EncryptionService.instance) {
      EncryptionService.instance = new EncryptionService();
    }
    return EncryptionService.instance;
  }

  /**
   * Chiffre un texte avec AES-256
   */
  encrypt(text: string, customKey?: string): string {
    try {
      const keyToUse = customKey || this.key;
      const encrypted = CryptoJS.AES.encrypt(text, keyToUse, {
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
      return encrypted.toString();
    } catch (error) {
      console.error('Erreur lors du chiffrement:', error);
      throw new Error('Échec du chiffrement');
    }
  }

  /**
   * Déchiffre un texte chiffré avec AES-256
   */
  decrypt(encryptedText: string, customKey?: string): string {
    try {
      const keyToUse = customKey || this.key;
      const decrypted = CryptoJS.AES.decrypt(encryptedText, keyToUse, {
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
      const decryptedText = decrypted.toString(CryptoJS.enc.Utf8);
      
      if (!decryptedText) {
        throw new Error('Clé de déchiffrement invalide');
      }
      
      return decryptedText;
    } catch (error) {
      console.error('Erreur lors du déchiffrement:', error);
      throw new Error('Échec du déchiffrement');
    }
  }

  /**
   * Génère une clé aléatoire sécurisée
   */
  generateSecureKey(length: number = 32): string {
    return CryptoJS.lib.WordArray.random(length).toString();
  }

  /**
   * Hache un mot de passe avec SHA-256
   */
  hashPassword(password: string, salt?: string): string {
    const saltToUse = salt || CryptoJS.lib.WordArray.random(128/8).toString();
    const hash = CryptoJS.PBKDF2(password, saltToUse, {
      keySize: 256/32,
      iterations: 10000
    });
    return `${saltToUse}:${hash.toString()}`;
  }

  /**
   * Vérifie un mot de passe haché
   */
  verifyPassword(password: string, hashedPassword: string): boolean {
    try {
      const [salt, hash] = hashedPassword.split(':');
      const computedHash = CryptoJS.PBKDF2(password, salt, {
        keySize: 256/32,
        iterations: 10000
      });
      return hash === computedHash.toString();
    } catch (error) {
      return false;
    }
  }

  /**
   * Génère un hash pour vérifier l'intégrité des données
   */
  generateIntegrityHash(data: string): string {
    return CryptoJS.SHA256(data).toString();
  }

  /**
   * Vérifie l'intégrité des données
   */
  verifyIntegrity(data: string, hash: string): boolean {
    const computedHash = this.generateIntegrityHash(data);
    return computedHash === hash;
  }
}

export const encryptionService = EncryptionService.getInstance();