# WpfApp

Projekt WPF z architekturą MVVM, z podziałem na warstwy aplikacji, logiki biznesowej oraz testów jednostkowych.


# Struktura projektu
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

### Opis folderów

- **WpfApp.Wpf** – warstwa prezentacji (UI), zawiera widoki, viewmodel oraz pomocnicze klasy jak komendy i konwertery.
- **WpfApp.Core** – logika biznesowa, modele, usługi, baza danych, stałe i enumeracje.
- **WpfApp.Tests** – testy jednostkowe dla logiki biznesowej i warstwy UI.

## Aktualizacja bazy danych

Aby dodać nową migrację i zaktualizować bazę danych, wykonaj poniższe kroki:

```bash
cd WpfApp.Core
dotnet ef migrations add <NazwaMigracji>
dotnet ef database update