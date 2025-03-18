# 💻 Calculator WPF Application

## ✍️ Description
This is a calculator application developed in C# using WPF. The application offers two operation modes: Standard and Programmer, each having its own window (`MainWindow` for Standard mode and `ProgrammerMainWindow` for Programmer mode). The MVVM architecture is used to separate the application logic from the graphical interface.

## ⚙️ Features
- 📐 **Standard Mode:** Basic arithmetic operations (addition, subtraction, multiplication, division).
- 🔢 **Programmer Mode:** Operations on different numerical bases (Bin, Oct, Dec, Hex).
- 💾 **Settings Persistence:** The application remembers the last used mode, numerical base, and whether the `Digit Grouping` option was enabled or not, using `Properties.Settings`.
- 🎨 **Modern Interface:** The graphical interface is developed using XAML and follows the MVVM principles.

## 🚀 How to Use
1. 📥 Clone this repository:
```bash
 git clone https://github.com/utilizator/calculator-wpf.git
```
2. 📝 Open the solution in Visual Studio.
3. ▶️ Compile and run the application.

## 📁 Project Structure
- `MainWindow.xaml` & `MainWindow.xaml.cs` – Interface and logic for Standard mode.
- `ProgrammerMainWindow.xaml` & `ProgrammerMainWindow.xaml.cs` – Interface and logic for Programmer mode.
- `StandardCalculatorViewModel.cs` – Logic for the standard calculator.
- `ProgrammerCalculatorViewModel.cs` – Logic for the programmer calculator.
- `ExpresionInputView` & `ExpresionInputWindow` – Interface and logic with order of operations.
- `MemoryStackWindow` – Interface and logic for the calculator's memory window.
- `Themes` – Calculator styling, using colors and gradients.
- `Properties/Settings.settings` – Persistent storage of user settings.

## 📦 Dependencies
- .NET 8.0
- WPF (Windows Presentation Foundation)
