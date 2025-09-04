# AtomicHabits app

AtomicHabits is an application that helps build and maintain small, daily habits. It allows you to track your progress, monitor consistency, and motivate yourself to implement positive changes in your life.  
A WPF project following the MVVM architecture, with separation of application layers, business logic, and unit tests.

## Features

- Create custom habits with descriptions
- Mark completed habits daily
- Track progress through charts and statistics (TODO)
- Habit reminders (push/desktop) (TODO)
- Filter habits by category or priority (TODO)
- Motivating weekly and monthly summaries (TODO)

## How it works

1. The user adds new habits with a description and (TODO: category).
2. Each day, the user marks which habits were completed. 
3. If the application is running at midnight, it resets the checkboxes and increments the streaks.
4. Upon startup, it checks whether yesterday's tasks were completed; if so, it resets the checkboxes and increments the streaks.
5. The application generates progress charts and summaries. (TODO)
6. The user receives notifications reminding them of their habits. (TODO)

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
 │   │		   └── RelayCommand.cs  # Implementacja ICommand dla MVVM
 │   │    ├── Converters
 │   │		   └── 
 │   ├── App.xaml
 │   └── App.xaml.cs
 ├── WpfApp.Core
 │   ├── Constans
 │   │    └── DbConnectionString.cs
 │   ├── Data
 │   │    └── AppDbContext.cs
 │   ├── Interfaces
 │   │    └── IAtomicHabitService.cs
 │   │    └── IDailyResetService.cs
 │   │    └── IProgressHistoryService.cs
 │   ├── Migrations
 │   │    └── FirstMigration.cs
 │   │    └── SecondMigration.cs
 │   │    └── ...
 │   ├── Models
 │   │    ├── Shared
 │   │    │    └── BaseEntityModel.cs
 │   │    │    └── BaseModel.cs
 │   │    └── AtomicHabitModel.cs
 │   │    └── ProgressHistoryModel.cs
 │   ├── Services
 │   │    └── AtomicHabitService.cs
 │   │    └── DailyResetService.cs
 │   │    └── ProgressHistoryService.cs
 │   ├── Enums
 │   │    └── 
 ├── WpfApp.Tests
 │   ├── CoreTests
 │   │    └── AtomicHabitServiceTests.cs
 │   │    └── DailyResetServiceTests.cs
 │   │    └── ProgressHistoryServiceTests.cs
 │   ├── WpfTests
 │   │    └── HomeViewModelTests.cs
```

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
```

# Task List

## Backlog 📌
- [ ] Editing the list
    - [ ] Deleting habits
    - [ ] Moving habits
    - [ ] Editing tasks
    - [ ] Filtering the list

## Todo ⏳

    
## Completed ✅ 
- [✅] Create GitHub repository
- [✅] Set up project skeleton
- [✅] Table in db based on the AtomicHabitModel
- [✅] Table in db based on the ProgressHistoryModel
- [✅] Add new habit to list and database
- [✅] Communication with the database and habitsList (checkBox update db)
- [✅] Logic for marking habits - when should be chcked and unchecked (on startapp and at midnight)
- [✅] Streak logic - when increment and reset (on startapp and at midnight)
### Tests
- [ ] Write unit tests for AtomicHabitServiceTests
- [ ] Write unit tests for DailyResetServiceTests
- [ ] Write unit tests for ProgressHistoryServiceTests