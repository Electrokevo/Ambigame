# 🌱 AmbiGame

**Videojuego educativo para niños con enfoque ambiental**

## 📖 Descripción

AmbiGame es un videojuego infantil inspirado en el clásico *Snake*, diseñado para promover la conciencia ambiental en niños. El jugador controla a un personaje que recoge basura, fomentando de forma lúdica el hábito de mantener limpios los entornos. El objetivo es enseñar valores ecológicos mientras los niños se divierten.

Este proyecto forma parte del curso de **Lenguajes de Programación** y busca integrar la educación ambiental con la tecnología y el entretenimiento digital.

---

## 🎯 Objetivos

### Objetivo general

Diseñar un videojuego infantil con temática ecológica que promueva la conciencia ambiental en niños.

### Objetivos específicos

* Desarrollar una narrativa basada en la recolección de basura.
* Fomentar el aprendizaje de los valores ecológicos.
* Evaluar el impacto del videojuego en la percepción ambiental de los niños.

---

## 📦 Alcance

* **Back-end (Ruby on Rails)**:

  * Almacena historial de partidas.
  * Gestiona base de datos de usuarios.
* **Front-end (Godot con C#)**:

  * Juego interactivo tipo *Snake* con temática ambiental.
  * Varios niveles disponibles.
* **Características principales**:

  * Inicio de sesión y registro.
  * Ranking de puntuaciones.
  * Historial de partidas.
  * Tutorial y reglas del juego.

---

## 🛠️ Tecnologías utilizadas

### Back-end

* Lenguaje: **Ruby**
* Framework: **Ruby on Rails**

### Front-end

* Motor: **Godot Engine**
* Lenguaje: **C#**

---

## 🏗️ Arquitectura

* **Tipo de aplicación**: Escritorio (con posibilidad de exportación a web o móvil usando Godot).
* **Patrón arquitectónico**: MVC.

---

## 🚀 Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/ambigame.git
cd ambigame
```

### 2. Configurar el back-end

```bash
cd backend
bundle install
rails db:create db:migrate
rails server
```

### 3. Ejecutar el front-end

Abrir el proyecto en Godot-mono (con soporte para C#) y ejecutar la escena principal (LoginScene.tscn).

---

## 🎮 Cómo jugar

1. Regístrate o inicia sesión.
3. Mueve al personaje para recolectar basura.
4. Evita chocar contra ti mismo.
5. Acumula puntos y compite en el ranking.

---

## 👥 Autores

* **Kevin Mejía**
* **Levin Moran**

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT. Puedes usarlo, modificarlo y distribuirlo libremente, siempre dando crédito a los autores.

---
