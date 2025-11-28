# Nubelity API

## 1. Requerimientos

* .NET 8
* Docker / Docker Compose
* PostgreSQL 15

## 2. Funcionalidades

1. Autenticación con JWT (expiración 1 hora)
2. CRUD de autores y libros
3. Normalización de textos (títulos y nombres)
4. Validación de ISBN mediante servicio SOAP externo
5. Obtención de portadas mediante API REST externa
6. Paginación en consultas
7. Búsqueda opcional por título y autor
8. Carga masiva de libros mediante CSV
9. SeedData inicial:

   * Usuario: **admin**
   * Password: **admin123**

## 3. Ejemplo de CSV para carga masiva

Formato esperado:

```
Title,ISBN,PageNumber,AuthorName
```

Ejemplo:

```
Clean Architecture,9780134494166,432,Robert Martin
Domain Driven Design,9780321125217,560,Eric Evans
Refactoring,9780201485677,448,Martin Fowler
```

## 4. Docker

### 4.1 Construir imagen manualmente

```
docker build -t nubelity-api -f Nubelity.API/Dockerfile .
```

### 4.2 Ejecutar contenedor manualmente

```
docker run -d -p 8080:8080 --name nubelity-api-container nubelity-api
```

## 5. Docker Compose

### 5.1 Ejecutar todo el entorno

En la raíz del proyecto:

```
docker compose up -d --build
```

Esto levanta:

* **nubelity-postgres** (PostgreSQL)
* **nubelity-api** (API)

### 5.2 Ver contenedores activos

```
docker ps
```

### 5.3 Detener contenedores

```
docker compose down
```

## 6. Acceso a Swagger

Una vez levantado el entorno:

```
http://localhost:8080/swagger/index.html
```

## 7. Credenciales de autenticación

El sistema se inicializa con el usuario:

* **username:** admin
* **password:** admin123

## 8. Estructura del proyecto

* Nubelity.API
* Nubelity.Application
* Nubelity.Domain
* Nubelity.Infrastructure
* Nubelity.Tests

## 9. Pruebas

Para ejecutar los tests:

```
dotnet test
```
