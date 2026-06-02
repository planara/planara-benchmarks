![build](https://github.com/planara/planara-benchmarks/actions/workflows/build.yml/badge.svg)
![release](https://github.com/planara/planara-benchmarks/actions/workflows/release.yml/badge.svg)
![publish-k3s](https://github.com/planara/planara-benchmarks/actions/workflows/publish-k3s.yml/badge.svg?branch=main)
![version](https://img.shields.io/github/v/tag/planara/planara-benchmarks?sort=semver)
[![Codecov](https://codecov.io/gh/planara/planara-benchmarks/branch/main/graph/badge.svg)](https://codecov.io/gh/planara/planara-benchmarks)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

## Planara.Benchmarks

Сервис хранения результатов benchmark-тестирования редактора.

Отвечает за сохранение запусков тестирования, результатов отдельных тестов,
основных метрик производительности и истории значений для построения графиков.

Один запуск benchmark-тестирования может содержать один или несколько вложенных тестов
(light, medium, heavy, mixed). Каждый тест хранит итоговые метрики и историю измерений.

Реализован как ASP.NET Core + GraphQL сервис с JWT-аутентификацией.

## Features

- Сохранение benchmark-запусков
- Хранение нескольких тестов внутри одного запуска
- Хранение итоговых метрик производительности
- Хранение истории метрик для графиков
- Получение запуска benchmark-тестирования по ID
- Получение списка запусков пользователя
- Пагинация списка запусков
- Получение общего количества запусков (`totalCount`)
- Фильтрация и сортировка запусков
- Удаление benchmark-запуска
- Проверка владельца запуска через `UserId`
- JWT авторизация (`[Authorize]`)
- Валидация входных данных (FluentValidation)
- GraphQL API (HotChocolate)
- Покрытие тестами

## GraphQL API

### Queries

- `getBenchmarkRun(request: GetBenchmarkRunRequest): BenchmarkRun`  
  Возвращает детальный отчет benchmark-запуска по ID  
  Включает список вложенных тестов и историю метрик  
  _(требует авторизации)_

- `getMyBenchmarkRuns: BenchmarkRunConnection`  
  Возвращает список benchmark-запусков текущего пользователя  
  Поддерживает пагинацию, фильтрацию, сортировку и получение `totalCount`  
  _(требует авторизации)_

### Mutations

- `saveBenchmarkRun(request: SaveBenchmarkRunRequest): BenchmarkRun`  
  Сохраняет результат benchmark-запуска  
  Поддерживает сохранение нескольких тестов внутри одного запуска  
  _(требует авторизации)_

- `deleteBenchmarkRun(request: DeleteBenchmarkRunRequest): DeleteBenchmarkRunResponse`  
  Удаляет benchmark-запуск текущего пользователя  
  _(требует авторизации)_
