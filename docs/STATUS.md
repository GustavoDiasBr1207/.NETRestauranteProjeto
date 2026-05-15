# Status de Implementação — RestaurantOrders API

> Documento de referência para desenvolvimento. Atualizado em: 2026-05-15

---

## Legenda

- ✅ **Feito** — implementado e funcional
- ⚠️ **Parcial** — estrutura criada, mas com lógica pendente ou stub
- ❌ **Não feito** — ainda não iniciado

---

## Domain Layer

### Entidades

| Item | Status | Observação |
|------|--------|-----------|
| `BaseEntity` (Id, CreatedAt, UpdatedAt, DomainEvents) | ✅ | |
| `Order` — factory, transições de status, invariantes | ✅ | |
| `OrderItem` — snapshot de preço e nome | ✅ | |
| `Restaurant` — factory, activate/deactivate | ✅ | |
| `Table` — factory, QR Code, activate/deactivate | ✅ | |
| `MenuItem` — factory, availability | ✅ | |
| `MenuCategory` — factory, activate/deactivate | ✅ | |
| `Customer` — factory, update | ✅ | |

### Value Objects

| Item | Status | Observação |
|------|--------|-----------|
| `Money` — imutável, operadores, IEquatable | ✅ | |
| `TableQrCode` — imutável, Generate(), IEquatable | ✅ | |

### Domain Events

| Item | Status | Observação |
|------|--------|-----------|
| `OrderPlacedEvent` | ✅ | |
| `OrderStatusChangedEvent` | ✅ | |
| `OrderItemAddedEvent` | ✅ | Publicado mas sem handler dedicado ainda |

### Exceções

| Item | Status |
|------|--------|
| `DomainException` | ✅ |
| `NotFoundException` | ✅ |
| `InvalidOrderStatusException` | ✅ |
| `MenuItemUnavailableException` | ✅ |

### Enums

| Item | Status | Observação |
|------|--------|-----------|
| `OrderStatusEnum` | ✅ | |
| `PaymentMethodEnum` | ⚠️ | Enum definido mas sem uso em nenhuma entidade ainda |

### Interfaces

| Item | Status |
|------|--------|
| `IOrderRepository` | ✅ |
| `IMenuRepository` | ✅ |
| `ITableRepository` | ✅ |
| `IRestaurantRepository` | ✅ |
| `INotificationService` | ✅ |
| `IQrCodeService` | ✅ |
| `IStorageService` | ✅ |
| `IUnitOfWork` | ✅ |

---

## Application Layer

### Pipeline

| Item | Status | Observação |
|------|--------|-----------|
| `LoggingBehavior` | ✅ | |
| `ValidationBehavior` | ✅ | |
| `TransactionBehavior` | ✅ | Chama `IUnitOfWork.CommitAsync` — commit atômico |

### Orders — Commands

| Command | Handler | Validator | Status | Observação |
|---------|---------|-----------|--------|-----------|
| `CreateOrderCommand` | ✅ | ✅ | ✅ | Valida existência de Restaurante e Mesa |
| `AddItemToOrderCommand` | ✅ | ✅ | ✅ | |
| `RemoveItemFromOrderCommand` | ✅ | ✅ | ✅ | |
| `SubmitOrderCommand` | ✅ | ✅ | ✅ | Notificação via `OrderPlacedEventHandler` |
| `CancelOrderCommand` | ✅ | ✅ | ✅ | |
| `UpdateOrderStatusCommand` | ✅ | ✅ | ✅ | |

### Orders — Queries

| Query | Handler | Status | Observação |
|-------|---------|--------|-----------|
| `GetOrderByIdQuery` | ✅ | ✅ | |
| `GetOrdersByTableQuery` | ✅ | ✅ | |
| `GetActiveOrdersByRestaurantQuery` | ✅ | ✅ | Retorna pedidos com status ≠ Delivered/Cancelled |

### Orders — Event Handlers

| Handler | Status | Observação |
|---------|--------|-----------|
| `OrderPlacedEventHandler` | ✅ | Chama `INotificationService.NotifyKitchenAsync` |
| `OrderStatusChangedEventHandler` | ✅ | Loga a transição — estender para notificar mesa via Realtime |
| Handler para `OrderItemAddedEvent` | ❌ | Evento publicado mas sem handler |

### Menu — Commands

| Command | Handler | Validator | Status |
|---------|---------|-----------|--------|
| `CreateMenuItemCommand` | ✅ | ✅ | ✅ |
| `UpdateMenuItemAvailabilityCommand` | ✅ | ✅ | ✅ |

### Menu — Queries

| Query | Handler | Status |
|-------|---------|--------|
| `GetMenuByRestaurantQuery` | ✅ | ✅ |
| `GetMenuItemByIdQuery` | ✅ | ✅ |

### Tables — Queries

| Query | Handler | Status |
|-------|---------|--------|
| `GetTableByQrCodeQuery` | ✅ | ✅ |

### Não implementados (Application)

| Item | Observação |
|------|-----------|
| Queries de `Restaurant` | Nenhuma query de restaurante existe ainda |
| Queries de `Table` por restaurante | Não há query para listar mesas de um restaurante |
| Commands de `Restaurant` | Criar, atualizar, ativar/desativar restaurante |
| Commands de `Table` | Criar mesa, regenerar QR Code, ativar/desativar |
| Commands de `MenuCategory` | Criar, atualizar, ativar/desativar categoria |
| Commands de `Customer` | Criar e atualizar cliente |
| Handler para `OrderItemAddedEvent` | Evento publicado mas ignorado |

### DTOs

| DTO | Status | Observação |
|-----|--------|-----------|
| `OrderDto` | ✅ | Com XML docs |
| `OrderItemDto` | ✅ | Com XML docs |
| `MenuCategoryDto` | ✅ | Com XML docs |
| `MenuItemDto` | ✅ | Com XML docs |
| `TableDto` | ✅ | Com XML docs |
| `RestaurantDto` | ❌ | Não criado |
| `CustomerDto` | ❌ | Não criado |

### Mapeamentos (AutoMapper)

| Mapeamento | Status |
|-----------|--------|
| `Order → OrderDto` | ✅ |
| `OrderItem → OrderItemDto` | ✅ |
| `MenuCategory → MenuCategoryDto` | ✅ |
| `MenuItem → MenuItemDto` | ✅ |
| `Table → TableDto` | ✅ |
| `Restaurant → RestaurantDto` | ❌ |
| `Customer → CustomerDto` | ❌ |

---

## Infrastructure Layer

### ApplicationDbContext

| Item | Status | Observação |
|------|--------|-----------|
| DbSets configurados | ✅ | |
| `ApplyConfigurationsFromAssembly` | ✅ | |
| Timestamps automáticos (`SaveChangesAsync`) | ✅ | |
| Dispatch de domain events | ✅ | |
| Implementa `IUnitOfWork` | ✅ | |

### Configurações EF Core

| Entidade | Status | Observação |
|----------|--------|-----------|
| `RestaurantConfiguration` | ✅ | |
| `TableConfiguration` | ✅ | |
| `MenuCategoryConfiguration` | ✅ | |
| `MenuItemConfiguration` | ✅ | Money como OwnsOne |
| `OrderConfiguration` | ✅ | Status como string, TotalAmount OwnsOne |
| `OrderItemConfiguration` | ✅ | UnitPrice e Subtotal como OwnsOne |
| `CustomerConfiguration` | ✅ | |

### Repositórios

| Repositório | Status | Observação |
|-------------|--------|-----------|
| `OrderRepository` | ✅ | Sem SaveChangesAsync (commit via UoW) |
| `MenuRepository` | ✅ | Sem SaveChangesAsync |
| `TableRepository` | ✅ | Sem SaveChangesAsync |
| `RestaurantRepository` | ✅ | Sem SaveChangesAsync |

### Serviços

| Serviço | Status | O que falta |
|---------|--------|------------|
| `QrCodeService` | ✅ | Gera PNG via QRCoder |
| `SupabaseNotificationService` | ⚠️ | Stub — implementar Supabase Realtime Broadcast quando adicionar o client |
| `SupabaseStorageService` | ⚠️ | Stub — implementar upload/delete no Supabase Storage |

### Migrations

| Item | Status | Observação |
|------|--------|-----------|
| Migration inicial | ❌ | Nenhuma migration gerada ainda — rodar `dotnet ef migrations add Inicial` |
| Banco criado/atualizado | ❌ | Depende da migration |

---

## API Layer

### Controllers — Endpoints

| Endpoint | Status | Observação |
|----------|--------|-----------|
| `POST /api/orders` | ✅ | |
| `GET /api/orders/{id}` | ✅ | |
| `GET /api/orders?tableId=&restaurantId=` | ✅ | |
| `POST /api/orders/{id}/items` | ✅ | |
| `DELETE /api/orders/{id}/items/{itemId}` | ✅ | |
| `POST /api/orders/{id}/submit` | ✅ | |
| `PATCH /api/orders/{id}/status` | ✅ | Requer JWT |
| `DELETE /api/orders/{id}` | ✅ | |
| `GET /api/menu?restaurantId=` | ✅ | |
| `POST /api/menu-items` | ✅ | Requer JWT |
| `PATCH /api/menu-items/{id}/availability` | ✅ | Requer JWT |
| `GET /api/tables/by-qrcode/{token}` | ✅ | |

### Controllers — A implementar

| Endpoint | Observação |
|----------|-----------|
| `GET /api/restaurants/{id}` | Dados do restaurante |
| `POST /api/restaurants` | Criar restaurante |
| `PUT /api/restaurants/{id}` | Atualizar restaurante |
| `GET /api/restaurants/{id}/tables` | Listar mesas |
| `POST /api/restaurants/{id}/tables` | Criar mesa |
| `PATCH /api/tables/{id}/qrcode` | Regenerar QR Code |
| `GET /api/tables/{id}/qrcode/image` | Imagem PNG do QR Code |
| `GET /api/menu-items/{id}` | Detalhe do item |
| `PUT /api/menu-items/{id}` | Atualizar item do cardápio |
| `DELETE /api/menu-items/{id}` | Remover item |
| `POST /api/menu/categories` | Criar categoria |
| `PUT /api/menu/categories/{id}` | Atualizar categoria |
| `DELETE /api/menu/categories/{id}` | Remover categoria |
| `RestaurantsController` | Classe criada mas vazia |

### Middlewares

| Item | Status |
|------|--------|
| `ExceptionHandlingMiddleware` | ✅ |
| `RequestLoggingMiddleware` | ✅ |

### OpenAPI / Swagger

| Item | Status | Observação |
|------|--------|-----------|
| Swashbuckle configurado | ✅ | |
| JWT security no Swagger | ✅ | Apenas endpoints `[Authorize]` |
| XML comments na API | ✅ | `GenerateDocumentationFile = true` |
| XML comments no Application | ✅ | `GenerateDocumentationFile = true` |
| Tags com descrição | ✅ | |
| Enum como string | ✅ | |
| Tipos de resposta corretos | ✅ | `CreateOrderResponse`, `CreateMenuItemResponse` |

### Autenticação / Autorização

| Item | Status | Observação |
|------|--------|-----------|
| JWT Bearer configurado | ✅ | |
| `[Authorize]` nos endpoints sensíveis | ✅ | Status update e gestão de cardápio |
| CORS configurável via appsettings | ✅ | |
| Roles / Policies por perfil | ❌ | Atualmente qualquer JWT autenticado acessa endpoints protegidos |

---

## Testes

| Item | Status | Observação |
|------|--------|-----------|
| `RestaurantOrders.Domain.Tests` | ⚠️ | Projeto criado — verificar cobertura atual |
| `RestaurantOrders.Application.Tests` | ⚠️ | Projeto criado — verificar cobertura atual |
| Testes de integração | ❌ | Nenhum projeto criado — recomendado TestContainers com PostgreSQL |
| Testes de API (E2E) | ❌ | |

---

## Infraestrutura / DevOps

| Item | Status | Observação |
|------|--------|-----------|
| Supabase Auth configurado | ✅ | |
| Connection String no appsettings | ✅ | Mover para user-secrets em dev |
| Credenciais no controle de versão | ⚠️ | appsettings.json tem a senha do banco — usar user-secrets ou variáveis de ambiente |
| `.gitignore` para user-secrets | ❌ | |
| Migrations EF geradas | ❌ | |
| CI/CD | ❌ | |
| Rate Limiting | ❌ | Nativo no ASP.NET 8 (`AddRateLimiter`) |

---

## Próximos Passos Sugeridos (em ordem de prioridade)

### 1. Banco de dados (bloqueante)
```bash
dotnet ef migrations add Inicial \
  --project src/RestaurantOrders.Infrastructure \
  --startup-project src/RestaurantOrders.API

dotnet ef database update \
  --project src/RestaurantOrders.Infrastructure \
  --startup-project src/RestaurantOrders.API
```

### 2. Credenciais seguras
Mover connection string e secrets para `user-secrets`:
```bash
dotnet user-secrets init --project src/RestaurantOrders.API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<conn>" --project src/RestaurantOrders.API
dotnet user-secrets set "Supabase:JwtSecret" "<secret>" --project src/RestaurantOrders.API
```

### 3. Controllers restantes
Implementar `RestaurantsController`, endpoints de `Tables` e `MenuCategories`.

### 4. Supabase Realtime
Implementar `SupabaseNotificationService` com o client Supabase após configurar as credenciais.

### 5. Autorização por perfil
Adicionar roles (ex: `gerente`, `cozinheiro`) e `[Authorize(Roles = "...")]` nos endpoints administrativos.

### 6. Testes
- Completar testes unitários de domínio
- Adicionar testes da camada de Application com NSubstitute
- Criar projeto de testes de integração com TestContainers

### 7. Produção
- Rate limiting (`AddRateLimiter`)
- Health checks (`AddHealthChecks`)
- `AllowedOrigins` configurado no appsettings de produção
- `RequireHttpsMetadata = true` em `AuthExtensions.cs`
