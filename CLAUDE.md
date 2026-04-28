# ◆ knotless

> chaos refined into order. everything in its place, all in lowercase.

## ⚒ project vision

a desktop application built with **c#** and **avalonia ui** that weaves your messy workspace into a structured tapestry. no shouting, no caps, just clean accents and pure logic.

## ✧ aesthetic & ui

**Typography:** strictly lowercase (except for system paths/folders).

**colors:**
- base: dark or light gray
- accents: neon pink and deep purple

**themes:**
- **dim**: dark mode for late-night coding
- **bright**: light mode for focused mornings

**vibe:** minimal, non-intrusive, focused on accents rather than splashes of color.

## ◉ core features

### stage: foundation
- setup avalonia ui project under wsl
- create a basic json config reader (`settings.json`)
- implement a manual "clean now" button

### stage: the weaver (logic)
- file sorting engine (move files based on extension)
- date-based nesting (e.g., `images/2026/april/`)
- error handling (skip files that are currently in use)

### stage: live watch
- integrate `filesystemwatcher`
- silent background operation (minimize to tray)

### stage: polishing
- english localization (primary)
- russian localization (secondary)
- "the black hole" feature (auto-delete temporary files after 24h)

## ⚙ technical stack

- **Language:** c# (.net 10 sdk)
- **Framework:** avalonia ui (for the "not just a console" feel)
- **Config:** `system.text.json`
- **I/O:** `system.io` & `system.io.filesystemwatcher`

## ▶ how to start

### installation

```bash
dotnet new install Avalonia.Templates
```

### create the project

```bash
dotnet new avalonia.mvvm -o knotless
```

### remember

keep it simple: don't overthink the architecture yet. we'll start with just one button that moves one file.

## ◇ dev notes

- stay lowkey
- focus on the "lower case" soul of the project
- if it feels too heavy, we cut the fluff