# Arquitetura do Projeto — RestaurantOrders API

## Visão Geral

Sistema de pedidos de restaurante via QR Code. O cliente escaneia o código da mesa, consulta o cardápio, monta o carrinho e envia o pedido. A cozinha recebe a notificação em tempo real via Supabase Realtime.

**Stack:** .NET 8 · ASP.NET Core · EF Core · Npgsql · PostgreSQL (Supabase) · MediatR · FluentValidation · AutoMapper · Supabase Auth (JWT)

---

## Estrutura da Solution

```
RestaurantOrders.sln
├── src/
│   ├── RestaurantOrders.Domain          # Núcleo do negócio — sem dependências externas
│   ├── RestaurantOrders.Application     # Casos de uso — CQRS, validações, mapeamentos
│   ├── RestaurantOrders.Infrastructure  # Persistência e serviços externos
│   └── RestaurantOrders.API             # HTTP — controllers, middlewares, configuração
└── tests/
    ├── RestaurantOrders.Domain.Tests
    └── RestaurantOrders.Application.Tests
```

**Fluxo de dependências:**

```
API → Application → Domain ← Infrastructure
```

A camada de Domain não referencia nenhuma outra camada do projeto.

---

## 1. Domain Layer

### 1.1 Entidades

Todas herdam `BaseEntity` (Id: Guid, CreatedAt, UpdatedAt, DomainEvents).

#### `Order` — Agregado Raiz Principal
```
Order
├── RestaurantId, TableId, CustomerId?
├── Status: OrderStatusEnum
├── TotalAmount: Money (Value Object)
├── Notes?, PlacedAt?, ConfirmedAt?, ReadyAt?, DeliveredAt?
└── Items: IReadOnlyCollection<OrderItem>
```

**Factory:** `Order.Create(restaurantId, tableId, customerId?, notes?)`  
**Status inicial:** `Draft`

**Fluxo de status:**
```
Draft → Pending → Confirmed → Preparing → Ready → Delivered
  ↓         ↓         ↓           ↓         ↓
Cancelled (de qualquer status exceto Delivered e Cancelled)
```

**Métodos do domínio:**
- `AddItem(menuItem, quantity, notes?)` — valida `Draft` e disponibilidade do item
- `RemoveItem(orderItemId)` — valida `Draft`
- `UpdateNotes(notes)` — valida `Draft`
- `Submit()` — Draft → Pending; publica `OrderPlacedEvent`
- `Confirm()` — Pending → Confirmed; publica `OrderStatusChangedEvent`
- `StartPreparing()` — Confirmed → Preparing; publica `OrderStatusChangedEvent`
- `MarkReady()` — Preparing → Ready; publica `OrderStatusChangedEvent`
- `Deliver()` — Ready → Delivered; publica `OrderStatusChangedEvent`
- `Cancel()` — qualquer → Cancelled (exceto Delivered); publica `OrderStatusChangedEvent`

#### `OrderItem` — Entidade de Agregado
```
OrderItem
├── OrderId, MenuItemId
├── Name: string           ← snapshot no momento do pedido
├── UnitPrice: Money       ← snapshot no momento do pedido
├── Subtotal: Money        ← snapshot no momento do pedido
├── Quantity: int
└── Notes?: string
```

> **Decisão de design:** `OrderItem` captura snapshot de nome e preço no momento do pedido. Alterações futuras no cardápio não afetam pedidos já criados.

**Factory (interno ao agregado):** `OrderItem.Create(orderId, menuItem, quantity, notes?)`

#### `Restaurant`
```
Restaurant
├── Name, Slug (único, lowercase), LogoUrl?, IsActive
├── Tables: IReadOnlyCollection<Table>
└── Categories: IReadOnlyCollection<MenuCategory>
```

**Factory:** `Restaurant.Create(name, slug, logoUrl?)`  
**Métodos:** `Update(name, logoUrl)`, `Activate()`, `Deactivate()`

#### `Table`
```
Table
├── RestaurantId, Number: int
├── QrCode: TableQrCode (Value Object) ← token único da mesa
├── QrCodeUrl?: string
└── IsActive: bool
```

**Factory:** `Table.Create(restaurantId, number)` — gera `TableQrCode` automaticamente  
**Métodos:** `RegenerateQrCode()`, `SetQrCodeUrl(url)`, `Activate()`, `Deactivate()`

#### `MenuItem`
```
MenuItem
├── RestaurantId, CategoryId
├── Name, Description?, Price: Money, ImageUrl?
├── IsAvailable: bool (padrão: true)
└── DisplayOrder: int
```

**Factory:** `MenuItem.Create(restaurantId, categoryId, name, price, description?, imageUrl?, displayOrder?)`  
**Métodos:** `Update(...)`, `MakeAvailable()`, `MakeUnavailable()`

#### `MenuCategory`
```
MenuCategory
├── RestaurantId, Name, DisplayOrder, IsActive
└── Items: IReadOnlyCollection<MenuItem>
```

#### `Customer`
```
Customer
└── Name?, Phone?   ← ambos opcionais (cliente anônimo é suportado)
```

---

### 1.2 Value Objects

#### `Money`
```csharp
Money { Amount: decimal, Currency: string = "BRL" }
```
- Imutável, implementa `IEquatable<Money>`
- Operadores: `+`, `-`, `*` (por int) — validam moedas iguais
- `Money.Zero()` → `new Money(0, "BRL")`
- EF Core: mapeado como `OwnsOne` com colunas `amount` e `currency`

#### `TableQrCode`
```csharp
TableQrCode { Token: string }  // 32 chars hex (Guid.NewGuid().ToString("N"))
```
- Imutável, implementa `IEquatable<TableQrCode>`
- `TableQrCode.Generate()` → novo token UUID
- EF Core: mapeado como `OwnsOne` com coluna `qr_code_token` (unique index)

---

### 1.3 Domain Events

Publicados internamente pelas entidades via `BaseEntity.AddDomainEvent()`. Despachados pelo `ApplicationDbContext.SaveChangesAsync` via MediatR antes de persistir.

| Evento | Quando | Dados |
|--------|--------|-------|
| `OrderPlacedEvent` | `order.Submit()` | OrderId, RestaurantId, TableId, TotalAmount |
| `OrderStatusChangedEvent` | Toda transição de status | OrderId, OldStatus, NewStatus |
| `OrderItemAddedEvent` | `order.AddItem()` | OrderId, MenuItemId, Quantity |

---

### 1.4 Exceções de Domínio

| Exceção | Situação | HTTP mapeado |
|---------|----------|--------------|
| `DomainException` | Base — regra de negócio violada | 422 |
| `NotFoundException` | Entidade não encontrada | 404 |
| `InvalidOrderStatusException` | Transição de status inválida | 422 |
| `MenuItemUnavailableException` | Item indisponível no carrinho | 422 |

---

### 1.5 Interfaces do Domain

**Repositórios** (`Domain/Interfaces/Repositories/`):
- `IOrderRepository` — CRUD + queries por mesa e restaurante
- `IMenuRepository` — categorias com itens, item por ID
- `ITableRepository` — por ID e por token QR
- `IRestaurantRepository` — por ID, slug e listagem de ativos

**Serviços** (`Domain/Interfaces/Services/`):
- `INotificationService` — notifica cozinha e mesa (Supabase Realtime)
- `IQrCodeService` — gera PNG e valida token
- `IStorageService` — upload/delete no Supabase Storage

**Unit of Work** (`Domain/Interfaces/`):
- `IUnitOfWork` — `CommitAsync()` — commit atômico centralizado

---

## 2. Application Layer

### 2.1 Pipeline de Behaviors (MediatR)

Ordem de execução para cada request:

```
Request
  └─► LoggingBehavior       ← loga início e tempo total
        └─► ValidationBehavior  ← executa todos os validators FluentValidation
              └─► TransactionBehavior ← chama IUnitOfWork.CommitAsync ao final
                    └─► Handler        ← lógica do caso de uso
```

**`LoggingBehavior`:** loga `"Executando {RequestName}"` antes e `"{RequestName} concluído em {Ms}ms"` depois.

**`ValidationBehavior`:** coleta todos os `IValidator<TRequest>` registrados, agrega os erros e lança `ValidationException` se houver falhas. O middleware converte em HTTP 400.

**`TransactionBehavior`:** chama `IUnitOfWork.CommitAsync()` após o handler. Para queries (sem mudanças rastreadas) é um no-op seguro. Garante que todos os repositórios do mesmo escopo são persistidos atomicamente em um único `SaveChangesAsync`.

---

### 2.2 Commands

Cada command fica em `Commands/<NomeCommand>/` com três arquivos:
- `<Nome>Command.cs` — classe/record com os dados de entrada
- `<Nome>CommandHandler.cs` — lógica de execução
- `<Nome>CommandValidator.cs` — regras FluentValidation

**Commands void** usam `IRequest` (não `IRequest<Unit>`).  
**Commands com retorno** usam `IRequest<Guid>`.

| Command | Entrada | Saída | Descrição |
|---------|---------|-------|-----------|
| `CreateOrderCommand` | RestaurantId, TableId, CustomerId? | Guid (orderId) | Cria pedido Draft; valida existência de restaurante e mesa |
| `AddItemToOrderCommand` | OrderId¹, MenuItemId, Quantity, Notes? | void | Adiciona item ao carrinho |
| `RemoveItemFromOrderCommand` | OrderId, OrderItemId | void | Remove item do carrinho |
| `SubmitOrderCommand` | OrderId | void | Draft → Pending; dispara `OrderPlacedEvent` |
| `CancelOrderCommand` | OrderId | void | Qualquer → Cancelled (exceto Delivered) |
| `UpdateOrderStatusCommand` | OrderId¹, NewStatus | void | Avança status (uso da cozinha) |
| `CreateMenuItemCommand` | RestaurantId, CategoryId, Name, Price, ... | Guid (menuItemId) | Cria item no cardápio |
| `UpdateMenuItemAvailabilityCommand` | MenuItemId, IsAvailable | void | Habilita/desabilita item |

> ¹ Marcado com `[JsonIgnore]` — preenchido via rota, não aparece no body do Swagger.

---

### 2.3 Queries

| Query | Entrada | Saída |
|-------|---------|-------|
| `GetOrderByIdQuery` | OrderId | `OrderDto?` |
| `GetOrdersByTableQuery` | TableId | `List<OrderDto>` |
| `GetActiveOrdersByRestaurantQuery` | RestaurantId | `List<OrderDto>` (Status ≠ Delivered, Cancelled) |
| `GetMenuByRestaurantQuery` | RestaurantId | `List<MenuCategoryDto>` |
| `GetMenuItemByIdQuery` | MenuItemId | `MenuItemDto?` |
| `GetTableByQrCodeQuery` | QrCodeToken | `TableDto?` |

---

### 2.4 Event Handlers

| Handler | Evento | Ação |
|---------|--------|------|
| `OrderPlacedEventHandler` | `OrderPlacedEvent` | Chama `INotificationService.NotifyKitchenAsync` |
| `OrderStatusChangedEventHandler` | `OrderStatusChangedEvent` | Loga a transição de status |

---

### 2.5 DTOs

| DTO | Campos |
|-----|--------|
| `OrderDto` | Id, RestaurantId, TableId, Status (string), TotalAmount, Currency, Notes?, PlacedAt?, CreatedAt, Items |
| `OrderItemDto` | Id, MenuItemId, MenuItemName, Quantity, UnitPrice, Subtotal, Notes? |
| `MenuCategoryDto` | Id, Name, Items |
| `MenuItemDto` | Id, Name, Description, Price, ImageUrl?, IsAvailable |
| `TableDto` | Id, RestaurantId, Number, QrCodeToken |

---

### 2.6 MappingProfile (AutoMapper)

| Origem | Destino | Mapeamentos especiais |
|--------|---------|----------------------|
| `Order` | `OrderDto` | `Status.ToString()`, `TotalAmount.Amount`, `TotalAmount.Currency` |
| `OrderItem` | `OrderItemDto` | `Name → MenuItemName`, `UnitPrice.Amount`, `Subtotal.Amount` |
| `MenuItem` | `MenuItemDto` | `Price.Amount` |
| `Table` | `TableDto` | `QrCode.Token → QrCodeToken` |
| `MenuCategory` | `MenuCategoryDto` | mapeamento direto |

---

## 3. Infrastructure Layer

### 3.1 ApplicationDbContext

Herda `DbContext` e implementa `IUnitOfWork`.

**`SaveChangesAsync` / `CommitAsync`:**
1. Atualiza `CreatedAt` (EntityState.Added) e `UpdatedAt` (Added ou Modified) em todas as `BaseEntity`
2. Coleta domain events de entidades rastreadas e limpa os eventos
3. Publica cada event via `IMediator.Publish()` (antes do persist)
4. Chama `base.SaveChangesAsync()` para persistir no banco

> **Atenção:** events são despachados **antes** do persist. Event handlers não devem consultar o banco para dados recém-inseridos.

**DbSets:** `Restaurants`, `Tables`, `MenuCategories`, `MenuItems`, `Customers`, `Orders`, `OrderItems`

**Configurações:** carregadas via `ApplyConfigurationsFromAssembly` — uma classe por entidade em `Persistence/Configurations/`.

---

### 3.2 Configurações EF Core

| Tabela | Destaques |
|--------|-----------|
| `restaurants` | Slug com unique index; cascade delete em Tables e Categories |
| `tables` | `qr_code_token` unique; unique (RestaurantId, Number) |
| `menu_categories` | Index em RestaurantId |
| `menu_items` | `Price` como `OwnsOne` com precision(10,2); índice (RestaurantId, IsAvailable) |
| `orders` | `Status` como string (HasConversion); `TotalAmount` como `OwnsOne`; índice (RestaurantId, Status) |
| `order_items` | `UnitPrice` e `Subtotal` como `OwnsOne`; cascade de Order |
| `customers` | Tabela simples sem relacionamentos explícitos com fk |

---

### 3.3 Repositórios

**Padrão:** os métodos de escrita (`AddAsync`, `UpdateAsync`, `DeleteAsync`) apenas rastreiam mudanças no `ChangeTracker` — **não chamam `SaveChangesAsync`**. O commit é centralizado no `TransactionBehavior`.

| Repositório | Queries disponíveis |
|-------------|-------------------|
| `OrderRepository` | Por Id, com items; ativos por restaurante; por mesa |
| `MenuRepository` | Categorias com items; item por Id; items por restaurante |
| `TableRepository` | Por Id; por token QR (filtra IsActive) |
| `RestaurantRepository` | Por Id; por slug (filtra IsActive); todos ativos |

---

### 3.4 Serviços de Infraestrutura

| Serviço | Implementação | Status |
|---------|---------------|--------|
| `QrCodeService` | Gera PNG via `QRCoder` library | ✅ Implementado |
| `SupabaseNotificationService` | Broadcast Supabase Realtime | ⚠️ Stub (log placeholder) |
| `SupabaseStorageService` | Upload/delete Supabase Storage | ⚠️ Stub (log placeholder) |

---

## 4. API Layer

### 4.1 Middleware (ordem de execução)

```
Request
  └─► ExceptionHandlingMiddleware    ← captura todas as exceções e retorna ProblemDetails
        └─► RequestLoggingMiddleware ← loga method, path, status e tempo
              └─► HTTPS Redirection
                    └─► CORS
                          └─► Authentication (JWT Bearer)
                                └─► Authorization
                                      └─► Controllers
```

**`ExceptionHandlingMiddleware` — mapeamento:**

| Exceção | HTTP |
|---------|------|
| `ValidationException` | 400 — com lista de erros |
| `NotFoundException` | 404 |
| `DomainException` (e subclasses) | 422 |
| `UnauthorizedAccessException` | 401 |
| Qualquer outra | 500 |

Retorna sempre `ProblemDetails` com `Content-Type: application/problem+json`.

---

### 4.2 Controllers

| Controller | Rota Base | Auth |
|------------|-----------|------|
| `OrdersController` | `/api/orders` | Misto |
| `MenuController` | `/api/menu` | Anon |
| `MenuItemsController` | `/api/menu-items` | JWT (todos) |
| `TablesController` | `/api/tables` | Anon |
| `RestaurantsController` | `/api/restaurants` | — (sem endpoints ainda) |

**Endpoints implementados:**

```
POST   /api/orders                          → 201 CreateOrderResponse
GET    /api/orders/{id}                     → 200 OrderDto | 404
GET    /api/orders?tableId=&restaurantId=  → 200 List<OrderDto>
POST   /api/orders/{id}/items               → 204 | 400 | 404 | 422
DELETE /api/orders/{id}/items/{itemId}      → 204 | 404 | 422
POST   /api/orders/{id}/submit              → 204 | 404 | 422
PATCH  /api/orders/{id}/status  [JWT]       → 204 | 400 | 401 | 404 | 422
DELETE /api/orders/{id}                     → 204 | 404 | 422
GET    /api/menu?restaurantId=              → 200 List<MenuCategoryDto>
POST   /api/menu-items          [JWT]       → 201 CreateMenuItemResponse
PATCH  /api/menu-items/{id}/availability [JWT] → 204 | 401 | 404
GET    /api/tables/by-qrcode/{token}        → 200 TableDto | 404
```

---

### 4.3 Modelos da API

| Modelo | Uso |
|--------|-----|
| `ErrorResponse` | Corpo de erro retornado pelo middleware |
| `CreateOrderResponse` | Resposta do POST /api/orders |
| `CreateMenuItemResponse` | Resposta do POST /api/menu-items |
| `UpdateAvailabilityRequest` | Body do PATCH /api/menu-items/{id}/availability |

---

### 4.4 Autenticação

JWT Bearer via Supabase Auth. Dois modos configurados em `AuthExtensions.cs`:

1. **OIDC Discovery (padrão ativo):** `Authority = {Supabase:Url}/auth/v1`, `Audience = "authenticated"`
2. **JWT Secret direto (comentado):** usa `SymmetricSecurityKey` com o secret do Supabase

Configuração em `appsettings.json`:
```json
{
  "Supabase": {
    "Url": "https://<projeto>.supabase.co",
    "JwtSecret": "<secret>"
  }
}
```

---

### 4.5 CORS

Configurável via `appsettings.json`:
```json
{
  "Cors": {
    "AllowedOrigins": ["https://meuapp.com"]
  }
}
```

Array vazio → `AllowAnyOrigin` (desenvolvimento).  
Array preenchido → `WithOrigins(...)` (produção).

---

### 4.6 OpenAPI / Swagger

Disponível em `/docs`. JSON spec em `/openapi/v1/openapi.json`.

- Segurança JWT documentada via `AuthorizeOperationFilter` — apenas endpoints `[Authorize]` exibem o cadeado
- XML comments das camadas API e Application incluídos nos schemas
- Tags com descrição: Orders, Menu, Tables, MenuItems, Restaurants

---

## 5. Fluxo Completo — Envio de Pedido

```
Cliente HTTP
    │
    ▼
POST /api/orders/{id}/submit
    │
    ▼
ExceptionHandlingMiddleware (try/catch)
    │
    ▼
RequestLoggingMiddleware (Stopwatch.Start)
    │
    ▼
OrdersController.Submit(id)
    │  cria SubmitOrderCommand { OrderId = id }
    ▼
IMediator.Send(command)
    │
    ├─► LoggingBehavior.Handle         → loga início
    │
    ├─► ValidationBehavior.Handle      → valida OrderId (NotEmpty)
    │
    ├─► TransactionBehavior.Handle     → aguarda handler completar
    │
    └─► SubmitOrderCommandHandler.Handle
            │
            ├─► orderRepository.GetByIdWithItemsAsync(orderId)
            │       (EF: SELECT Orders + Items WHERE Id = ?)
            │
            ├─► order.Submit()
            │       ├─► valida Status == Draft
            │       ├─► valida Items.Count > 0
            │       ├─► Status = Pending; PlacedAt = UtcNow
            │       └─► AddDomainEvent(OrderPlacedEvent)
            │
            └─► orderRepository.UpdateAsync(order)
                    (EF: Entry.State = Modified — sem SaveChanges)
    │
    ▼ (volta para TransactionBehavior)
IUnitOfWork.CommitAsync()
    │
    ▼
ApplicationDbContext.SaveChangesAsync()
    │
    ├─► Atualiza UpdatedAt
    │
    ├─► Coleta e limpa OrderPlacedEvent
    │
    ├─► IMediator.Publish(OrderPlacedEvent)
    │       └─► OrderPlacedEventHandler.Handle
    │               └─► INotificationService.NotifyKitchenAsync(restaurantId, orderId)
    │
    └─► base.SaveChangesAsync()   ← persist no PostgreSQL
    │
    ▼
204 No Content
```

---

## 6. Comandos Úteis

```bash
# Build
dotnet build

# Executar API (Swagger em /docs)
dotnet run --project src/RestaurantOrders.API

# Todos os testes
dotnet test

# EF Core — nova migration
dotnet ef migrations add <NomeDaMigration> \
  --project src/RestaurantOrders.Infrastructure \
  --startup-project src/RestaurantOrders.API

# EF Core — aplicar migrations
dotnet ef database update \
  --project src/RestaurantOrders.Infrastructure \
  --startup-project src/RestaurantOrders.API
```
