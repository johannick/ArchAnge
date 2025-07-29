# SecretVault - Plateforme de Partage de Secrets Sécurisée

Une plateforme web moderne permettant aux utilisateurs de partager des informations sensibles avec des mécanismes de contrôle d'accès basés sur des conditions prédéfinies.

## 🚀 Stack Technique

### Frontend
- **Vue.js 3** avec Composition API
- **TypeScript** pour la sécurité des types
- **Tailwind CSS** pour le design
- **Pinia** pour la gestion d'état
- **Vue Router** pour la navigation
- **Vite** comme bundler

### Backend (À implémenter)
- **.NET 6+** Web API
- **PostgreSQL** comme base de données
- **JWT** pour l'authentification
- **AES-256** pour le chiffrement des secrets

## 🔧 Installation et Configuration

### Prérequis
- Node.js 18+
- npm ou yarn
- .NET 6+ SDK
- PostgreSQL 13+

### Frontend

```bash
# Cloner le repository
git clone <repository-url>
cd secret-vault

# Installer les dépendances frontend
cd frontend
npm install

# Copier et configurer les variables d'environnement
cp .env.example .env
# Éditer le fichier .env avec vos configurations

# Lancer le serveur de développement
npm run dev
```

### Backend (À implémenter)

```bash
# Créer le projet .NET
dotnet new webapi -n SecretVault.Api
cd SecretVault.Api

# Installer les packages NuGet nécessaires
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package BCrypt.Net-Next
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

# Configurer la base de données
dotnet ef migrations add InitialCreate
dotnet ef database update

# Lancer l'API
dotnet run
```

## 🏗️ Architecture

### Structure Frontend
```
frontend/
├── src/
│   ├── components/          # Composants réutilisables
│   ├── views/              # Pages/vues principales
│   ├── stores/             # Stores Pinia
│   ├── services/           # Services API et utilitaires
│   ├── types/              # Définitions TypeScript
│   ├── router/             # Configuration des routes
│   └── assets/             # Ressources statiques
├── public/                 # Fichiers publics
└── dist/                   # Build de production
```

### Structure Backend (Recommandée)
```
SecretVault.Api/
├── Controllers/            # Contrôleurs API
├── Models/                 # Modèles de données
├── Services/               # Logique métier
├── Data/                   # Contexte EF et migrations
├── DTOs/                   # Objets de transfert de données
├── Middleware/             # Middlewares personnalisés
├── Extensions/             # Extensions et configurations
└── Security/               # Services de sécurité et chiffrement
```

## 🔐 Fonctionnalités de Sécurité

### Chiffrement
- **AES-256** pour le chiffrement des secrets
- **PBKDF2** pour le hachage des mots de passe
- **SHA-256** pour l'intégrité des données
- Clés de chiffrement générées aléatoirement

### Authentification
- **JWT** avec expiration automatique
- **Refresh tokens** pour la sécurité
- **Rate limiting** sur les tentatives de connexion
- **Rôles utilisateurs** (USER, MODERATOR, ADMIN)

### Contrôle d'accès
- **Conditions multiples** pour l'accès aux secrets
- **Validation automatique** des conditions
- **Audit trail** complet des accès
- **Soft delete** avec possibilité de restauration

## 📋 Fonctionnalités Principales

### Gestion des Secrets
- ✅ CRUD complet avec validation
- ✅ Chiffrement automatique avant stockage
- ✅ Système de catégories et tags
- ✅ Historique des modifications
- ✅ Soft delete avec restauration

### Conditions d'Accès
- ✅ Questions/réponses personnalisées
- ✅ Tâches à accomplir
- ✅ Critères temporels
- ✅ Géolocalisation
- ✅ Limitation du nombre de tentatives
- ✅ Conditions composées (ET/OU)

### Interface Utilisateur
- ✅ Dashboard responsive avec statistiques
- ✅ Recherche avancée avec filtres
- ✅ Système de notifications
- ✅ Mode sombre/clair
- ✅ Design moderne et intuitif

## 🚀 Déploiement

### Docker (Recommandé)

```dockerfile
# Dockerfile frontend
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

```dockerfile
# Dockerfile backend
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SecretVault.Api.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretVault.Api.dll"]
```

### Docker Compose

```yaml
version: '3.8'
services:
  frontend:
    build: ./frontend
    ports:
      - "3000:80"
    depends_on:
      - backend

  backend:
    build: ./backend
    ports:
      - "7001:80"
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Database=secretvault;Username=postgres;Password=password
    depends_on:
      - db

  db:
    image: postgres:13
    environment:
      POSTGRES_DB: secretvault
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"

volumes:
  postgres_data:
```

## 🧪 Tests

```bash
# Tests frontend
cd frontend
npm run test
npm run test:coverage

# Tests backend
cd backend
dotnet test
dotnet test --collect:"XPlat Code Coverage"
```

## 📚 Documentation API

L'API sera documentée avec Swagger/OpenAPI. Une fois le backend déployé, accédez à :
- Développement : `https://localhost:7001/swagger`
- Production : `https://your-domain.com/swagger`

## 🤝 Contribution

1. Fork le projet
2. Créer une branche feature (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📄 Licence

Ce projet est sous licence MIT. Voir le fichier `LICENSE` pour plus de détails.

## 🔒 Sécurité

Pour signaler une vulnérabilité de sécurité, veuillez envoyer un email à security@secretvault.com au lieu d'utiliser le système d'issues public.

## 📞 Support

- Documentation : [Wiki du projet](wiki-url)
- Issues : [GitHub Issues](issues-url)
- Email : support@secretvault.com

---

**⚠️ Important :** Cette application gère des données sensibles. Assurez-vous de suivre les meilleures pratiques de sécurité en production :
- Utilisez HTTPS uniquement
- Configurez des clés de chiffrement robustes
- Implémentez une sauvegarde régulière
- Surveillez les logs de sécurité
- Effectuez des audits de sécurité réguliers