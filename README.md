# 🧾 Sistema de Gestión de Productos y Presupuestos

Aplicación web desarrollada con **ASP.NET MVC** que permite administrar productos y generar presupuestos asociados a dichos productos.

El sistema permite registrar productos en una base de datos y utilizarlos para crear presupuestos que pueden contener múltiples ítems.

---

## 🚀 Funcionalidades

* 📦 Gestión de productos (CRUD)
* 🧾 Creación de presupuestos
* ➕ Agregar productos a un presupuesto
* 📋 Visualización de presupuestos creados
* ❌ Eliminación de productos o presupuestos
* 💾 Persistencia de datos en base de datos SQL

---

## 🛠 Tecnologías utilizadas

* **C#**
* **ASP.NET MVC**
* **SQLite / SQL**
* **HTML**
* **CSS**
* **Razor Views**

---

## 🧩 Arquitectura

El proyecto sigue el patrón **Model - View - Controller (MVC)** para organizar la aplicación:

* **Model:** Representa las entidades del sistema como Productos y Presupuestos.
* **View:** Interfaces de usuario desarrolladas con Razor.
* **Controller:** Gestiona las peticiones HTTP y la lógica de la aplicación.

El acceso a los datos se realiza mediante un **Repository Pattern**, separando la lógica de persistencia del resto de la aplicación.

---

## 📦 Instalación y ejecución

1. Clonar el repositorio

```bash
git clone https://github.com/Agust1nSc/Proyecto-de-mvc-con-sql-Sistema-de-productos-y-presupuestos-.git
```

2. Acceder al directorio del proyecto

```bash id="o6jv1n"
cd Proyecto-de-mvc-con-sql-Sistema-de-productos-y-presupuestos-
```

3. Ejecutar la aplicación

```bash id="1u7e8c"
dotnet run
```

4. Abrir el navegador en la dirección indicada por la aplicación.

---

## 📚 Objetivo del proyecto

Este proyecto fue desarrollado con fines educativos para practicar:

* Desarrollo de aplicaciones web con **ASP.NET MVC**
* Manejo de relaciones entre entidades
* Persistencia de datos en base de datos
* Implementación del patrón **Repository**
* Organización del código siguiendo buenas prácticas

---

## 👨‍💻 Autor

Proyecto desarrollado por **Agustín Saccone** como parte de su aprendizaje en desarrollo web con **C# y ASP.NET MVC** en la materia Taller de lenguajes 2.
