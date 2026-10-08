# Unidad 5 – HttpClientFactory y Resiliencia (.NET 10)

Repositorio de la clase de **Programación 4 – Unidad 5: Conexión con APIs externas**.

En este repositorio vamos a trabajar sobre dos prácticas:

1. **HttpClientFactory:** consumir una API pública de chistes usando `IHttpClientFactory`, respetando la arquitectura en capas (Application / Infrastructure / Presentation).
2. **Resiliencia:** hacer que nuestra API sobreviva a fallas de otra API usando **Retry** y **Circuit Breaker** con `Microsoft.Extensions.Http.Resilience` (la librería recomendada por Microsoft, construida sobre Polly v8).

---

## Requisitos

- [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2026, o VS Code con la extensión C# Dev Kit
- Git

Verificá la instalación con:

```bash
dotnet --version   # debe empezar con 10.
```

---

## Ramas

| Rama | Qué contiene |
|---|---|
| `main` | **Punto de partida.** Esqueleto de la API de chistes (para completar) + API B ya hecha. |
| `paso-1-httpclientfactory` | Práctica 1 resuelta: la API A consume la API de chistes. |
| `paso-2-resiliencia` | Práctica 2 resuelta: la API A consume a la API B con Retry + Circuit Breaker. |

¿Te quedaste atrás en la clase? Guardá tus cambios y pasate a la rama del paso siguiente:

```bash
git stash                              # guarda tus cambios (opcional)
git checkout paso-1-httpclientfactory  # o paso-2-resiliencia
```

---

## Estructura

```
Unidad5.slnx
├── ApiA/                      → nuestra API principal (puerto 5100)
│   ├── Application/           → modelos (DTOs) e interfaces
│   ├── Infrastructure/        → servicios que llaman a APIs externas
│   └── Presentation/          → controllers + Program.cs
└── ApiB/                      → API "inestable" para probar resiliencia (puerto 5200)
```

---

## Cómo correr las APIs

Desde la raíz del repositorio, en **dos terminales distintas**:

```bash
# Terminal 1 – API B
dotnet run --project ApiB

# Terminal 2 – API A
dotnet run --project ApiA/Presentation
```

- Swagger de la API A: http://localhost:5100/swagger
- Swagger de la API B: http://localhost:5200/swagger

> En Visual Studio podés elegir **"Configurar proyectos de inicio" → Varios proyectos de inicio** y marcar `Presentation` y `ApiB`.

---

## Práctica 1 – HttpClientFactory

API externa: [Official Joke API](https://github.com/15Dkatz/official_joke_api) (`https://official-joke-api.appspot.com/`).

Archivos a completar (rama `main`):

- `ApiA/Application/Models/JokeDTO.cs`
- `ApiA/Application/Interfaces/IJokeService.cs`
- `ApiA/Infrastructure/Services/JokeService.cs`
- `ApiA/Presentation/Controllers/JokesController.cs`
- `ApiA/Presentation/Program.cs`

Objetivo: que `GET /api/jokes/random` devuelva un chiste aleatorio. Después, agregá más endpoints (por id, por tipo, N chistes, etc.).

## Práctica 2 – Resiliencia

La API B (`GET /api/test/unstable`) **falla a propósito** las primeras N requests (por defecto 5). La API A la consume y aplica:

- **Retry:** reintenta automáticamente cuando la respuesta es un error transitorio (5xx, 408, 429 o error de red).
- **Circuit Breaker:** si la proporción de fallas supera un umbral, "abre el circuito" y deja de llamar a la API B durante un tiempo, devolviendo un error inmediato.

Endpoints útiles de la API B:

| Endpoint | Para qué sirve |
|---|---|
| `GET /api/test/unstable` | El endpoint que falla las primeras N veces |
| `GET /api/test/reset` | Vuelve el contador a 0 |
| `POST /api/test/umbral/{n}` | Cambia cuántas requests fallan (y resetea el contador) |

Mirá **las dos consolas** mientras probás: en la API A vas a ver los logs de reintentos y del estado del circuito; en la API B, cada request que efectivamente le llega.
