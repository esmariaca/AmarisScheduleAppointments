Amaris - Sistema de Turnos (API Backend)
Esta es la API robusta encargada de la lógica de negocio para la gestión de turnos de Amaris. Construida con .NET 8/9, utiliza una arquitectura por capas para garantizar la escalabilidad y mantenibilidad del sistema.

Características Técnicas
Arquitectura Limpia: Separación de responsabilidades entre Dominio, Aplicación, Infraestructura y API.

Persistencia: Uso de Entity Framework Core con SQLite para facilitar la portabilidad.

Auto-Seeding: La base de datos se crea y se llena con sucursales iniciales automáticamente al ejecutar el proyecto.

Stack Tecnológico
Framework: .NET (ASP.NET Core Web API)
Base de Datos: SQLite
ORM: Entity Framework Core
Documentación: Swagger / OpenAPI
Pruebas: xUnit + FluentAssertions + InMemoryDatabase

Configuración Local
Requisitos previos
.NET SDK (Versión 8 o superior).

Clonar el repositorio: git clone https://github.com/esmariaca/AmarisScheduleAppointments.git
