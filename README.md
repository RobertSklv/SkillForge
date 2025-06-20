# SkillForge

SkillForge is a knowledge-sharing platform for developers and tech enthusiasts. It enables registered users to write, share, and discover tutorials, articles, and technical guides on a wide range of programming and software development topics.

The platform features:

- **User-generated content**: Developers can publish articles, interact with articles written by other users, and also follow users and tags.

- **Rich discovery**: Articles can be browsed and filtered by categories and tags, helping users find topics that match their interests.

- **Interactive community**: Users can leave comments, engage in discussions, and show appreciation for insightful articles.

- **Personalized experience**: Users have profiles displaying their published articles and engagement.

- **Admin panel**: Admins can moderate content, manage categories and tags, and oversee platform activity.

SkillForge aims to foster a community-driven environment where developers can both learn from others and showcase their own expertise.

**Tech stack**:
- ASP.NET Core MVC
- Entity Framework Core
- SvelteKit
- TypeScript
- Bootstrap

Features:
- Headless SPA
- Authentication & Authorization
- Testable Service and Repository layers
- CRUD functionality
- Background Tasks for periodic aggregation and XML sitemap generation
- SEO

---

## Installation

### 1. Clone the repository

```powershell
git clone https://github.com/RobertSklv/SkillForge.git
```

### 2. Setup Svelte

- Navigate to Svelte's root directory and instantiate the environment file.
```powershell
cd Frontend/skillforge
copy .env.example .env
```

- Open the newly created `.env` file and fill in the frontend and backend base URLs.

- Install the dependencies and run the project.

```powershell
npm install
npm run dev -- --open
```

### 3. Install SQL Server

- Download SQL Server from the [official website](https://www.microsoft.com/en/sql-server/sql-server-downloads) (Choose the free Express edition)
- Open the installation wizard
- Choose basic installation
- Copy the connection string
- Click 'Install SSMS'

### 4. Install SQL Server Management Studio (SSMS)

- Download SSMS from the [official website](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16&redirectedfrom=MSDN)
- Open the installation wizard and install SSMS
- Open SSMS, and create a new database by right clicking the 'Databases' folder in the hierarchy and clicking 'New Database'
- Set the database name to 'SkillForge', or any other name, and click 'OK'

### 5. Create and setup the `appsettings.json` file

- Open the `SkillForge.sln` in Visual Studio and open the terminal
- Enter the following commands:
```powershell
copy appsettings.json.sample appsettings.json
```
- Open the newly created `appsettings.json` file
- Replace `<connection_string>` with the connection string you copied in step 3. Replace `master` with the database name you have set in step 4. Also add `TrustServerCertificate=True;` at the end of the connection string.
- Replace `<secret_key>` with a secure string which will be used in creating an admin account later.
- Fill in `<backend_domain>` and `<frontend_domain>` with the corresponding values.
- Save the file

### 6. Install the DB schema

```powershell
dotnet ef database update
```

### 7. Create admin user account

Run the project, enter the secret key you saved in `appsettings.json`, and create your admin user account.