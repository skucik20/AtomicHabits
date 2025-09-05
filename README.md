# AtomicHabits app

AtomicHabits is an application that helps build and maintain small, daily habits. It allows you to track your progress, monitor consistency, and motivate yourself to implement positive changes in your life.  
A WPF project following the MVVM architecture, with separation of application layers, business logic, and unit tests.

## Features

- Create custom habits with descriptions ✅
- Mark completed habits daily ✅
- Autamoatic streak update ✅
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

- [ ] Logic for ProgresHistory -> some charts etc
- [ ] Notyfications
- [ ] Add info that habbit has beed added
- [ ] Add some animations in background
- [ ] Remove highliting of the list
- [ ] Make toast in MVVM

## Todo ⏳
- [ ] Editing the list
    - [ ] Filtering the list

## Bugs

- [ ] If you click on X in HabitTitle TextBox in edit mode app will crash NULL

## Completed ✅ 
- [x] Create GitHub repository
- [x] Set up project skeleton
- [x] Table in db based on the AtomicHabitModel
- [x] Table in db based on the ProgressHistoryModel
- [x] Add new habit to list and database
- [x] Communication with the database and habitsList (checkBox update db)
- [x] Logic for marking habits - when should be chcked and unchecked (on startapp and at midnight)
- [x] Streak logic - when increment and reset (on startapp and at midnight)
- [x] UI structure (hamburger menu serup)
- [x] Deleting habits 
- [x] Editing tasks
- [x] Moving habits up and down in List
- [x] Add autofocused textboxes

### Tests
- [x] Write unit tests for AtomicHabitServiceTests
- [x] Write unit tests for DailyResetServiceTests
- [x] Write unit tests for ProgressHistoryServiceTests

