# Arquitectura por capas en .NET 6 con ASP.NET Core Web API

Proyecto de ejemplo desarrollado con **C#**, **.NET 6** y **ASP.NET Core Web API** para practicar una arquitectura separada por responsabilidades, con capas de dominio, aplicación, infraestructura y presentación.

El proyecto implementa una API de productos utilizando **Entity Framework Core**, el patrón **Repository**, servicios de aplicación, **AutoMapper**, inyección de dependencias y documentación de endpoints con **Swagger / OpenAPI**.

## Objetivo del proyecto

El objetivo principal es mostrar cómo organizar una solución .NET evitando concentrar toda la lógica en la capa web.

La solución separa:

- Las **entidades y contratos** del dominio.
- La **lógica de aplicación**.
- El **acceso a datos**.
- La **API HTTP**.
- Los **DTOs y mapeos** utilizados para exponer información al cliente.

Este enfoque facilita el mantenimiento, la evolución y las pruebas de una aplicación.

## Tecnologías utilizadas

- **C#**
- **.NET 6**
- **ASP.NET Core Web API**
- **Entity Framework Core 6**
- **Entity Framework Core InMemory**
- **AutoMapper**
- **Swagger / OpenAPI**
- **Dependency Injection**
- **Repository Pattern**
- **Visual Studio**

## Arquitectura de la solución

La solución está dividida en cuatro proyectos principales:

```text
CursoArquitecturaNet-Core/
├── CursoArquitecturaNet.Core/
├── CursoArquitecturaNet.Application/
├── CursoArquitecturaNet.Infraestructure/
├── CursoArquitecturaNet/
└── CursoArquitecturaNet.sln
```

### `CursoArquitecturaNet.Core`

Contiene los elementos centrales del dominio y no depende de la infraestructura ni de la API.

Incluye:

- Entidades.
- Clases base.
- Interfaces de repositorio.
- Contratos específicos para productos.

Elementos destacados:

```text
Entities/
├── Base/
│   └── BaseEntity.cs
└── Product.cs

Repositories/
├── Base/
│   └── IRepository.cs
└── IProductRepository.cs
```

### `CursoArquitecturaNet.Application`

Contiene la lógica de aplicación y los servicios que trabajan con las abstracciones definidas en `Core`.

Incluye:

```text
Interfaces/
└── IProductService.cs

Services/
└── ProductsService.cs
```

`ProductsService` utiliza `IProductRepository`, manteniendo desacoplada la lógica de aplicación de la implementación concreta del acceso a datos.

### `CursoArquitecturaNet.Infraestructure`

Implementa el acceso a datos mediante **Entity Framework Core**.

Incluye:

```text
Data/
└── CursoArquitecturaNetContext.cs

Repository/
├── Base/
│   └── Repository.cs
└── ProductRepository.cs
```

`Repository<T>` proporciona operaciones genéricas asíncronas para:

- Obtener todos los registros.
- Buscar mediante expresiones.
- Buscar por identificador.
- Crear.
- Actualizar.
- Eliminar.

`ProductRepository` amplía el repositorio genérico con consultas específicas para productos.

### `CursoArquitecturaNet`

Es la capa de presentación y contiene la **ASP.NET Core Web API**.

Incluye:

```text
Controllers/
└── ProductController.cs

DTOs/
├── Base/
│   └── BaseDTO.cs
└── ProductDTO.cs

Mapper/
└── ProductMapperProfiles.cs

Program.cs
```

En esta capa se configuran:

- Controladores.
- Inyección de dependencias.
- Repositorios.
- Servicios.
- Entity Framework Core.
- AutoMapper.
- Swagger.

## Modelo `Product`

El proyecto trabaja con una entidad de producto que permite practicar operaciones de consulta y mantenimiento de datos.

La API expone operaciones para:

- Obtener productos.
- Buscar productos por nombre.
- Buscar un producto por identificador.
- Crear productos.
- Actualizar productos.
- Eliminar productos.

## Endpoints principales

El controlador `ProductController` define endpoints como:

```text
GET    /Product/GetProductByName/{productName}
GET    /Product/GetProductById/{productId}
POST   /Product
PUT    /Product
DELETE /Product
```

## Entity Framework Core InMemory

Para facilitar la ejecución del ejemplo, el proyecto utiliza una base de datos en memoria:

```csharp
builder.Services.AddDbContext<CursoArquitecturaNetContext>(
    c => c.UseInMemoryDatabase("CursoArquitecturaNetConnection")
);
```

Esto permite probar la arquitectura sin instalar ni configurar un servidor de base de datos.

> Los datos almacenados en la base de datos InMemory se pierden al detener la aplicación.

## Inyección de dependencias

Los componentes se registran en `Program.cs` utilizando el contenedor de dependencias integrado de ASP.NET Core.

Entre otros:

```csharp
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductsService>();
```

De esta forma, las capas trabajan con interfaces y no necesitan conocer directamente las implementaciones concretas.

## AutoMapper

El proyecto utiliza **AutoMapper** para separar las entidades internas de los objetos expuestos por la API.

Esto permite trabajar con:

```text
Product
   ↕
ProductDTO
```

y evita exponer directamente el modelo de dominio como contrato externo de la API.

## Swagger / OpenAPI

Durante la ejecución en entorno de desarrollo se habilita Swagger:

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

Esto permite explorar y probar los endpoints desde el navegador.

## Puesta en marcha

### Requisitos

- **.NET 6 SDK**
- Visual Studio 2022, Visual Studio Code o un IDE compatible con .NET.

### Clonar el repositorio

```bash
git clone https://github.com/smarinwm/CursoArquitecturaNet-Core.git
cd CursoArquitecturaNet-Core
```

### Restaurar dependencias

```bash
dotnet restore
```

### Ejecutar la API

```bash
dotnet run --project CursoArquitecturaNet
```

Una vez iniciada la aplicación, consulta la URL indicada en la consola para acceder a Swagger.

Normalmente será una dirección similar a:

```text
https://localhost:<puerto>/swagger
```

## Conceptos que permite practicar

Este repositorio puede utilizarse como ejemplo para estudiar:

- Arquitectura por capas.
- Separación de responsabilidades.
- ASP.NET Core Web API.
- Programación orientada a objetos.
- Inyección de dependencias.
- Inversión de dependencias.
- Patrón Repository.
- Repositorios genéricos.
- Servicios de aplicación.
- Entity Framework Core.
- Consultas LINQ.
- Programación asíncrona con `Task`.
- DTOs.
- AutoMapper.
- Swagger y OpenAPI.

## Consideraciones

Este repositorio tiene un objetivo principalmente **didáctico**.

Antes de utilizar una arquitectura similar en un proyecto de producción conviene revisar aspectos como:

- Persistencia en una base de datos real.
- Validación de DTOs.
- Gestión centralizada de excepciones.
- Respuestas HTTP y códigos de estado.
- Logging.
- Autenticación y autorización.
- Tests unitarios y de integración.
- Versionado de la API.
- Gestión de configuración por entornos.

## Autor

**Silverio Marín** — Docente TIC en Valencia, especializado en programación y desarrollo de software.

Más contenidos sobre **programación y desarrollo de software**:

**[silveriomarin.com/programacion](https://silveriomarin.com/programacion/)**

GitHub: **[@smarinwm](https://github.com/smarinwm)**
