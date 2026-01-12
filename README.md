# Система управления сотрудниками (Employee Management System)

![Статус](https://img.shields.io/badge/Статус-Завершён-brightgreen)
![Лицензия](https://img.shields.io/badge/Лицензия-MIT-синий)
![Версия](https://img.shields.io/badge/Версия-1.0.0-фиолетовый)
![Тесты](https://img.shields.io/badge/Тесты-20%20проходят-зелёный)
![Покрытие](https://img.shields.io/badge/Покрытие-100%25-зелёный)

Комплексное **full-stack веб-приложение** для управления сотрудниками с современной аутентификацией, полными CRUD операциями, расширенным поиском и адаптивным интерфейсом.

---

## 📋 Оглавление

- [Обзор проекта](#обзор-проекта)
- [Технологический стек](#технологический-стек)
- [Структура проекта](#структура-проекта)
- [Начало работы](#начало-работы)
- [Документация API](#документация-api)
- [Тестирование](#тестирование)
- [Архитектура](#архитектура)
- [Ключевые концепции](#ключевые-концепции)
- [Безопасность](#безопасность)
- [Автор](#автор)

---

## Обзор проекта

**Система управления сотрудниками** — это **production-ready full-stack приложение**, которое демонстрирует современные практики разработки ПО. Написано на **C# ASP.NET Core** (backend) и **React** (frontend), предоставляет полное решение для управления данными сотрудников.

---

### Основной функционал

- **Управление сотрудниками** — Создание, чтение, обновление, удаление
- **Поиск и фильтрация** — Поиск по имени/email, фильтр по отделу
- **Пагинация** — 10 сотрудников на страницу с навигацией
- **Аутентификация пользователей** — Регистрация и логин с JWT токенами
- **Защищённые эндпоинты** — Контроль доступа по ролям
- **Валидация форм** — Клиентская и серверная валидация
- **Обработка ошибок** — Подробные сообщения об ошибках

### Технические возможности

- **20+ Unit тестов** — xUnit с мок-объектами
- **Документация API** — Интерактивный Swagger/OpenAPI
- **Clean Architecture** — Слоистая архитектура
- **Миграции БД** — EF Core контроль версий
- **Адаптивный дизайн** — Mobile-first CSS
- **JWT аутентификация** — Токены с 24-часовым сроком действия
- **CORS** — Поддержка фронт/бек коммуникации

---

## Технологический стек

### Backend
| Технология | Версия | Назначение |
|------------|--------|------------|
| **ASP.NET Core** | 10.0+ | Фреймворк Web API |
| **C#** | 12.0 | Язык программирования |
| **Entity Framework Core** | 10.0 | ORM и работа с БД |
| **SQL Server** | LocalDB | База данных |
| **JWT** | System.IdentityModel | Аутентификация |
| **xUnit** | 2.9.3 | Unit тестирование |
| **AutoMapper** | 12.0.1 | Маппинг объектов |

### Frontend
| Технология | Назначение |
|------------|------------|
| **React** | 18+ UI фреймворк |
| **React Router** | Навигация страниц |
| **Axios** | HTTP клиент |
| **CSS3** | Стилизация |
| **JavaScript (ES6+)** | Программирование |

### База данных
| Компонент | Детали |
|-----------|---------|
| **База данных** | SQL Server LocalDB |
| **Таблицы** | Employees, Users |
| **Миграции** | EF Core |

---

## Структура проекта
```
EmployeeManagement/
│
├── EmployeeManagement.Api/ # ASP.NET Core API
│ ├── Controllers/
│ │ ├── EmployeesController.cs # Employee CRUD эндпоинты
│ │ └── AuthController.cs # Login/Register эндпоинты
│ ├── Services/
│ │ ├── EmployeeService.cs # Бизнес-логика сотрудников
│ │ └── AuthService.cs # Логика аутентификации
│ ├── Program.cs # Конфигурация запуска
│ └── appsettings.json # Настройки окружения
│
├── EmployeeManagement.Core/ # Слой домена/моделей
│ ├── Models/
│ │ ├── Employee.cs # Сущность сотрудника
│ │ └── User.cs # Сущность пользователя
│ ├── DTOs/
│ │ ├── CreateEmployeeDto.cs # DTO для создания
│ │ ├── UpdateEmployeeDto.cs # DTO для обновления
│ │ ├── LoginRequestDto.cs # DTO для логина
│ │ ├── RegisterRequestDto.cs # DTO для регистрации
│ │ └── AuthResponseDto.cs # DTO ответа аутентификации
│ ├── Services/
│ │ ├── IEmployeeService.cs # Интерфейс сервиса
│ │ └── IAuthService.cs # Интерфейс аутентификации
│ └── Repositories/
│ └── IEmployeeRepository.cs # Интерфейс доступа к данным
│
├── EmployeeManagement.Data/ # Слой доступа к данным
│ ├── Repositories/
│ │ └── EmployeeRepository.cs # Реализация доступа к данным
│ ├── Migrations/ # Миграции БД
│ └── ApplicationDbContext.cs # EF Core DbContext
│
├── EmployeeManagement.Tests/ # Unit тесты
│ ├── Services/
│ │ ├── EmployeeServiceTests.cs # 13 тестов
│ │ └── AuthServiceTests.cs # 7 тестов
│ └── Mocks/
│ └── MockEmployeeRepository.cs # Тестовый двойник
│
└── employee_management.frontend/ # React Frontend
├── src/
│ ├── components/
│ │ ├── EmployeeList.jsx # Список с пагинацией
│ │ ├── EmployeeForm.jsx # Форма создания/редактирования
│ │ ├── Login.jsx # Страница логина
│ │ └── Register.jsx # Страница регистрации
│ ├── services/
│ │ └── api.js # Axios клиент
│ ├── context/
│ │ └── AuthContext.jsx # Глобальное состояние auth
│ ├── App.jsx # Главный компонент
│ └── index.js # Точка входа
└── package.json # Зависимости
```
---

## Начало работы

### Требования

**Backend:**
- Установлен **.NET SDK 10.0+**
- Установлена **Visual Studio 2022** или **VS Code**
- Установлен **SQL Server 2022 / LocalDB**
- Установлен глобальный инструмент **dotnet-ef**:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

**Frontend:**
- Установлен **Node.js 16+** и **npm 7+**
- Любой редактор кода (рекомендуется VS Code)

***

### Запуск backend (API)

```bash
# 1. Перейти в папку с API
cd EmployeeManagement.Api

# 2. Восстановить NuGet пакеты
dotnet restore

# 3. Применить миграции к базе данных
dotnet ef database update

# 4. Запустить API
dotnet run
```

После запуска:

- API будет доступен по адресу, например:  
  `https://localhost:7xxx`
- Swagger UI (документация) будет доступен по адресу:  
  `https://localhost:7xxx/swagger`

> **Примечание:** порт `7xxx` будет отличаться, смотрите вывод `dotnet run` или профиль запуска в Visual Studio (`launchSettings.json`).

***

### Запуск frontend (React)

```bash
# 1. Перейти в папку фронтенда
cd employee_management.frontend

# 2. Установить зависимости
npm install

# 3. Запустить dev-сервер
npm start
```

После запуска:

- Фронтенд откроется автоматически в браузере по адресу:  
  `http://localhost:3000` (или `3001`, если 3000 уже занят)

> **Важно:** убедитесь, что **backend запущен**, иначе React-приложение не сможет загрузить список сотрудников и выполнит запросы с ошибками.

***

### Настройка URL API для фронтенда

По умолчанию базовый URL API задаётся в файле:

`employee_management.frontend/src/services/api.js`

```javascript
const API_BASE_URL = 'https://localhost:7xxx/api'; // Замените на ваш реальный порт API
```

Если порт API другой — обновите его здесь.

***

### Запуск backend и frontend одновременно

**Вариант 1: через Visual Studio + терминал**

- Backend: запустить из Visual Studio (F5)
- Frontend: запустить через `npm start` в терминале

**Вариант 2: оба через терминал**

```bash
# Окно 1
cd EmployeeManagement.Api
dotnet run

# Окно 2
cd employee_management.frontend
npm start
```

***

## Документация API

### Базовый URL

- **Development (локально):**  
  `https://localhost:7xxx/api`

Все примеры ниже предполагают этот базовый адрес.

***

### Аутентификация (Auth)

#### Регистрация пользователя

**Запрос:**

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "johnsmith",
  "email": "john@example.com",
  "password": "SecurePassword123",
  "confirmPassword": "SecurePassword123"
}
```

**Успешный ответ (200 OK):**

```json
{
  "success": true,
  "message": "Registration successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "johnsmith",
    "email": "john@example.com",
    "role": "Employee"
  }
}
```

***

#### Логин пользователя

**Запрос:**

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "johnsmith",
  "password": "SecurePassword123"
}
```

**Успешный ответ (200 OK):**

```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "johnsmith",
    "email": "john@example.com",
    "role": "Employee"
  }
}
```

> **Важно:** полученный `token` нужно сохранять на фронтенде (localStorage) и передавать в заголовке `Authorization` для всех запросов к `/api/employees`.

***

### Работа с сотрудниками (Employees)

Все эндпоинты ниже **требуют авторизации**:

```http
Authorization: Bearer {ваш_JWT_токен}
```

***

#### Получить список сотрудников (с пагинацией, поиском и фильтрацией)

**Запрос:**

```http
GET /api/employees?pageNumber=1&pageSize=10&department=IT&searchTerm=john
Authorization: Bearer {token}
```

Параметры:

- `pageNumber` — номер страницы (по умолчанию 1)
- `pageSize` — размер страницы (по умолчанию 10)
- `department` — фильтр по отделу (опционально)
- `searchTerm` — поиск по имени/фамилии/email (опционально)

**Ответ (200 OK):**

```json
{
  "data": [
    {
      "id": 1,
      "fullName": "John Doe",
      "email": "john@example.com",
      "salary": 75000,
      "department": "IT",
      "isActive": true,
      "hireDate": "2026-01-11T17:08:02Z"
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

***

#### Получить сотрудника по ID

```http
GET /api/employees/1
Authorization: Bearer {token}
```

**Ответ (200 OK):**

```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "+1-555-0100",
  "salary": 75000,
  "department": "IT",
  "isActive": true,
  "hireDate": "2026-01-11T17:08:02Z",
  "createdAt": "2026-01-11T17:08:02Z",
  "updatedAt": null,
  "fullName": "John Doe"
}
```

***

#### Создать сотрудника

```http
POST /api/employees
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "phoneNumber": "+1-555-0101",
  "salary": 85000,
  "department": "HR"
}
```

**Успешный ответ (201 Created):**

```json
{
  "id": 2,
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "phoneNumber": "+1-555-0101",
  "salary": 85000,
  "department": "HR",
  "hireDate": "2026-01-11T18:00:00Z",
  "isActive": true,
  "createdAt": "2026-01-11T18:00:00Z",
  "updatedAt": null,
  "fullName": "Jane Smith"
}
```

***

#### Обновить сотрудника

```http
PUT /api/employees/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.updated@example.com",
  "phoneNumber": "+1-555-0100",
  "salary": 80000,
  "department": "IT",
  "isActive": true
}
```

**Ответ (204 No Content):** без тела.

***

#### Удалить сотрудника

```http
DELETE /api/employees/1
Authorization: Bearer {token}
```

**Ответ (204 No Content)**

---

## Тестирование

### Покрытие тестами

| Слой | Количество тестов | Покрытие |
|------|-------------------|----------|
| **EmployeeService** | 13 тестов | 100% |
| **AuthService** | 7 тестов | 100% |
| **Итого** | **20 тестов** | **100%** |

### Сценарии тестирования

**Тесты EmployeeService:**
- ✅ Создание сотрудника с валидными данными
- ✅ Блокировка дублирования email
- ✅ Получение сотрудника по ID
- ✅ Получение всех сотрудников
- ✅ Пагинация (10 на страницу)
- ✅ Фильтрация по отделу
- ✅ Обновление сотрудника
- ✅ Удаление сотрудника
- ✅ Обработка ошибок

**Тесты AuthService:**
- ✅ Регистрация с валидными данными
- ✅ Блокировка дублирования username/email
- ✅ Проверка совпадения паролей
- ✅ Логин с правильными учетными данными
- ✅ Отклонение неверных паролей
- ✅ Генерация JWT токена
- ✅ Хеширование пароля

### Запуск тестов

```bash
# Запуск всех тестов
dotnet test

# Запуск с подробным выводом
dotnet test --verbosity=detailed

# Запуск конкретного класса тестов
dotnet test --filter "EmployeeServiceTests"

# Запуск с покрытием кода
dotnet test /p:CollectCoverage=true
```

**Ожидаемый результат:**
```
Test run finished: 20 Tests (20 Passed, 0 Failed, 0 Skipped)
```

***

## Архитектура

### Слои архитектуры

```
┌─────────────────────────────────────────┐
│      API Layer (Контроллеры)            │
│  - HTTP запросы/ответы                  │
│  - Валидация                            │
│  - Аутентификация/авторизация           │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Service Layer (Бизнес-логика)        │
│  - Управление сотрудниками              │
│  - Аутентификация                       │
│  - Валидация и обработка ошибок         │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Repository Layer (Доступ к данным)   │
│  - SQL запросы                          │
│  - CRUD операции                        │
│  - Абстракции EF Core                   │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│    Data Layer (База данных)             │
│  - SQL Server                           │
│  - Маппинг сущностей                    │
│  - Миграции                             │
└─────────────────────────────────────────┘
```

### Используемые паттерны проектирования

1. **Repository Pattern** — абстрагирует доступ к данным
2. **Service Layer Pattern** — инкапсулирует бизнес-логику
3. **Dependency Injection** — слабая связанность компонентов
4. **DTO Pattern** — разделяет API-контракты и модели
5. **Factory Pattern** — генерация токенов (AuthService)

### Поток данных (Data Flow)

```
React Frontend  →  HTTP Request  →  API Controller
                    ↓
                Service Layer     →  Business Logic
                    ↓
              Repository Layer    →  Database Query
                    ↓
               ApplicationDbContext→  SQL Server
                    ↓
                Database Response →  JSON Response → React UI
```

### Принципы SOLID

- **S** — Single Responsibility: каждый класс имеет одну задачу
- **O** — Open/Closed: открыт для расширения, закрыт для модификации
- **L** — Liskov Substitution: подтипы заменяемы на базовые
- **I** — Interface Segregation: узкие интерфейсы
- **D** — Dependency Inversion: зависеть от абстракций


***

## Ключевые концепции

### Backend концепции
- **Clean Architecture** — разделение ответственности по слоям
- **SOLID принципы** — каждый класс имеет одну ответственность
- **Async/Await** — асинхронное программирование
- **Entity Framework Core** — ORM и миграции базы данных
- **JWT аутентификация** — токенная безопасность
- **Input Validation** — валидация данных на всех уровнях
- **Error Handling** — глобальная обработка ошибок
- **Unit Testing** — xUnit и Moq фреймворки
- **AutoMapper** — маппинг объектов

### Frontend концепции
- **Компонентная архитектура** — переиспользуемые React компоненты
- **React Hooks** — `useState`, `useEffect`, `useCallback`
- **Context API** — глобальное состояние приложения
- **React Router** — клиентская маршрутизация
- **HTTP клиент** — Axios для работы с API
- **Обработка форм** — контролируемые компоненты
- **Error Handling** — пользовательские сообщения об ошибках
- **Адаптивный дизайн** — mobile-first подход

### DevOps концепции
- **Database Migrations** — контроль версий схемы БД
- **Environment Configuration** — `appsettings.json`
- **API Documentation** — Swagger/OpenAPI
- **Git Version Control** — осмысленные коммиты

***

## Безопасность (Security)

### Реализованные меры безопасности

| Функция | Статус | Описание |
|---------|--------|----------|
| **Хеширование паролей** | ✅ SHA256 | Пароли сохраняются только в хешированном виде |
| **JWT токены** | ✅ 24 часа | Токены с истечением срока действия |
| **CORS политика** | ✅ AllowAll | Настроена для фронтенда |
| **HTTPS** | ✅ Production | Принудительно в продакшене |
| **Валидация ввода** | ✅ Server-side | Защита от некорректных данных |
| **SQL Injection** | ✅ EF Core | Параметризованные запросы |
| **Авторизация** | ✅ Role-based | Защищенные эндпоинты |
| **Токен в Header** | ✅ Bearer | Стандартная передача токенов |

***

## 👤 Автор

**SkyGet0**

***
