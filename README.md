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

- [ ] Settings
	- [ ] Notyfications
	- [ ] Open with PC start
    - [ ] Categories customize
- [ ] Make toast in MVVM
- [ ] Convert animations to MVVM
- [ ] Unable to click on date button in calendar -> now there is workeround with border
- [ ] Under left button delate-edit in list
- [ ] Add tabs witch categories -> if theere is a lot of tasks it will be easier for user to navigate


## Todo ⏳

- [ ] Add win habits
- [ ] Edit history, restore streak

## Bugs

- [ ] If you click on X in HabitTitle TextBox in edit mode app will crash NULL
- [x] Scroll does not work on list in Progrss history
- [ ] Remove highliting of the list
- [ ] If you change list arreangement it restes after restart

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
- [x] Add some animations in background
- [x] Add info that habbit has beed added
- [x] Created list of archive actions, displayes by combobox
- [x] About page
- [x] Settings page
- [x] Fix toast to the app corner not the window
- [x] Create widget 
- [x] Update about page
- [x] Ask user if he is sure to delate habbit
- [x] Table in db based on the CategoryModel
- [x] Add cetegories to habits
- [x] Logic for ProgresHistory -> some charts etc
- [x] Fix problem with close button in widget - sholud exit whole app
- [x] Add picture if habbit list is empty
- [x] Stylize widget
- [x] Add icons
- [x] Unable to add empty or duplicated habit title
- [x] If you delate habbit in combobox in progress history it still remains

### Tests
- [x] Write unit tests for AtomicHabitServiceTests
- [x] Write unit tests for DailyResetServiceTests
- [x] Write unit tests for ProgressHistoryServiceTests
- [ ] Write unit tests for CategoriesServiceTests

