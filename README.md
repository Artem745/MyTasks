# MyTasks

MyTasks is a simple cross-platform desktop task manager built with .NET 9, Avalonia UI, and SQLite.

## What it is

MyTasks lets you create basic tasks with a title, description, difficulty level and category (tabs). Data is stored in a local SQLite database (`tasks.db`). The UI is implemented with Avalonia and ViewModels use CommunityToolkit.Mvvm.

## Quick start

Build and run for development:

```bash
cd MyTasks
dotnet restore
dotnet build
dotnet run --project MyTasks.csproj
```

Note: the database file is created automatically on startup (`tasks.db` in the app working directory).

## Features

- Create tasks with title, description, difficulty and category
- Delete tasks
- Toggle task done/undone
- Progress indicator for the current category