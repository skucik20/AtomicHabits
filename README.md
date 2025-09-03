# AtomicHabits app

AtomicHabits is an application that helps build and maintain small, daily habits. It allows you to track your progress, monitor consistency, and motivate yourself to implement positive changes in your life.  
A WPF project following the MVVM architecture, with separation of application layers, business logic, and unit tests.

## Features

- Create custom habits with descriptions
- Mark completed habits daily
- Track progress through charts and statistics
- Habit reminders (push/desktop)
- Filter habits by category or priority
- Motivating weekly and monthly summaries

## How it works

1. The user adds new habits with a description and (TODO: category).
2. Each day, the user marks which habits were completed.
3. If the application is running at midnight, it resets the checkboxes and increments the streaks.
4. Upon startup, it checks whether yesterday's tasks were completed; if so, it resets the checkboxes and increments the streaks.
5. The application generates progress charts and summaries.
6. The user receives notifications reminding them of their habits.

## Instalation

```bash
git clone https://github.com/skucik20/AtomicHabits.git
cd src
dotnet build
dotnet run --project WpfApp.Wpf
```

## Project structure

```bash
/src
 ├── WpfApp.Wpf
 │   ├── Bootstrapper
 │   │    └── Bootstrapper.cs
 │   ├── ViewModels
 │   │    └── MainViewModel.cs
 │   ├── Views
 │   │    └── MainWindow.xaml
 │   ├── Helpers
 │   │    ├── Commands
 │   │		   └── RelayCommand.cs
 │   │    ├── Converters
 │   │		   └── 
 │   ├── App.xaml
 │   └── App.xaml.cs
 ├── WpfApp.Core
 │   ├── Data
 │   │    └── AppDbContext.cs
 │   ├── Services
 │   │    └── PersonService.cs
 │   ├── Models
 │   │    └── Person.cs
 │   ├── Enums
 │   │    └── ePeople.cs
 │   ├── Constants
 │   │    └── Const1.cs
 ├── WpfApp.Tests
 │   ├── CoreTests
 │   │    └── PersonServiceTests.cs
 │   ├── WpfTests
 │   │    └── MainViewModelTests.cs

### Folder Structure

- **WpfApp.Wpf** – presentation layer (UI), contains views, viewmodels, and helper classes such as commands and converters.
- **WpfApp.Core** – business logic, models, services, database, constants, and enums.
- **WpfApp.Tests** – unit tests for business logic and the UI layer.

## Database Update

To add a new migration and update the database, follow these steps:

```bash
cd WpfApp.Core
dotnet ef migrations add <MigrationName>
dotnet ef database update