# Микросервис отзывов

Небольшой веб-сервис на ASP.NET Core, который принимает отзывы о продуктах и хранит их в PostgreSQL.
Часть микросервисной системы курса PRO C#. Docker. Рядом работает products-service, а вся топология
системы (docker-compose и nginx) вынесена в отдельный инфра-репозиторий dockercourse-deploy.

## Что умеет

- принять отзыв о продукте (продукт, автор, оценка от 1 до 5, текст)
- отдать все отзывы продукта
- посчитать сводку по продукту, сколько отзывов и средняя оценка

## Структура проекта

- `Program.cs` точка входа, подключение базы, репозиторий, создание таблиц при старте
- `Models/` модели данных (Review, CreateReviewRequest, ReviewSummary)
- `Data/ReviewDbContext.cs` контекст EF Core, одна таблица отзывов
- `Services/` работа с базой (интерфейс IReviewRepository и реализация EfReviewRepository)
- `Controllers/ReviewsController.cs` HTTP-эндпоинты
- `Dockerfile` многостадийная сборка образа
- `.dockerignore` что не тащить в контекст сборки

Своих docker-compose.yml и nginx.conf у сервиса нет. Раньше, когда он был единственным сервисом, они
лежали рядом. Теперь система микросервисная, и вся топология переехала в инфра-репозиторий
dockercourse-deploy. Тут остались только код, Dockerfile и CI.

## Как хранятся отзывы

Отзывы лежат в таблице PostgreSQL, по одной строке на отзыв. Есть индекс по продукту, чтобы быстро
доставать отзывы конкретного продукта. Таблицу приложение создает само при старте. В реальном
проекте тут были бы EF-миграции, как в основной платформе, для демо хватает EnsureCreated.

## Деплой

На каждый пуш в `main` работает CI (`.github/workflows/deploy.yml`). Он собирает образ, пушит его в
GHCR и на сервере обновляет только свой контейнер.

```
docker compose pull reviews-service
docker compose up -d --no-deps reviews-service
```

Флаг `--no-deps` не трогает базу и соседние сервисы, обновляется ровно контейнер отзывов. Это
независимая выкатка. Системный docker-compose.yml лежит на сервере, его кладет туда инфра-репозиторий
dockercourse-deploy.

## Запуск всей системы

Сервис работает внутри системы. Чтобы поднять всю систему локально (база, Redis, reviews-service,
products-service, nginx), возьми инфра-репозиторий dockercourse-deploy и подними его docker-compose.
Reviews-service подтянется готовым образом из GHCR.

## Запуск только этого сервиса

Строка подключения ждет базу по имени `db` (имя сервиса в compose системы). Чтобы запустить сервис в
одиночку через `dotnet run`, укажи адрес своей базы, например поменяй `Host=db` на `Host=localhost` в
`appsettings.json` или задай переменную окружения `ConnectionStrings__Postgres`.

```
dotnet run
```

## Визуальный интерфейс API

Открой в браузере `http://localhost:8080/scalar/v1`. Это Scalar, визуальный интерфейс, где видно все
эндпоинты и можно отправлять запросы прямо со страницы, без curl.

## Проверка

Добавить отзыв
```
curl -X POST http://localhost:8080/reviews -H "Content-Type: application/json" -d "{\"productId\":\"course-1\",\"author\":\"Иван\",\"rating\":5,\"text\":\"Отличный курс\"}"
```

Все отзывы продукта
```
curl http://localhost:8080/reviews/course-1
```

Сводка по продукту
```
curl http://localhost:8080/reviews/course-1/summary
```
