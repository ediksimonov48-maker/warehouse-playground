# Склад техники

Учебное веб-приложение для учёта товаров, производителей, типов техники и стран поставки.

## Стек

- .NET 10 и Blazor Web App;
- интерактивный серверный режим Blazor;
- Entity Framework Core 10 и PostgreSQL;
- Docker Compose.

## Возможности

- CRUD для товаров, производителей и типов техники;
- добавление страны из формы товара;
- поиск по модели, производителю, типу и стране;
- фильтры по производителю, типу и цене;
- модальная форма товара;
- подтверждение удаления;
- начальные демонстрационные данные.

## Структура

- `Components` — страницы и общий интерфейс;
- `Models` — сущности приложения;
- `Data` — контекст базы, миграции и начальные данные;
- `Services` — операции с товарами и справочниками;
- `Program.cs` — настройка приложения и зависимостей.

## Локальный запуск

Требуется .NET 10 SDK и PostgreSQL 17 или совместимая версия.

Создайте базу `warehouse` и задайте строку подключения через User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:WarehouseDb" "Host=localhost;Port=5432;Database=warehouse;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```

Запустите приложение:

```powershell
dotnet run
```

Адрес приложения появится в консоли. При запуске миграции применяются автоматически.

## Docker Compose

Требуется Docker Desktop.

```powershell
docker compose up --build
```

Приложение будет доступно по адресу `http://localhost:8080`.

По умолчанию пароль PostgreSQL — `postgres`. Его можно изменить:

```powershell
$env:POSTGRES_PASSWORD = "пароль"
docker compose up --build
```

Остановить контейнеры без удаления данных:

```powershell
docker compose down
```

Удалить контейнеры вместе с данными базы:

```powershell
docker compose down -v
```

PostgreSQL и ключи Data Protection хранятся в именованных Docker volumes. Интерактивность Blazor в контейнере обеспечивается публикацией `blazor.web.js`.

## Миграции

Создать миграцию:

```powershell
dotnet ef migrations add MigrationName
```

Применить миграции вручную:

```powershell
dotnet ef database update
```

## Архитектура

Страницы Blazor обращаются к `ProductService` и `DirectoryService`, а сервисы работают с PostgreSQL через EF Core и `IDbContextFactory`. Для справочников настроены уникальные индексы, а удаление используемых записей запрещено внешними ключами.
