# Docker Course

Учебные проекты к курсу PRO C#. Docker (школа IRON PROGRAMMER).

## reviews-service

Микросервис отзывов на ASP.NET Core плюс PostgreSQL. Поднимается одним docker-compose и служит
примером контейнеризации в курсе. Приложение принимает отзывы о продуктах, хранит их в базе и
показывает сводку по продукту.

### Быстрый старт

```
cd reviews-service
docker compose up --build
```

Приложение открывается на `http://localhost:8080`, визуальный интерфейс API на
`http://localhost:8080/scalar/v1`. Полное описание внутри `reviews-service/README.md`.
