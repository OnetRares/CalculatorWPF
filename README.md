# ✍️ Descriere

Aceasta este o aplicație de calculator dezvoltată în C# folosind WPF. Aplicația oferă două moduri de operare: Standard și Programmer, fiecare având propria fereastră (MainWindow pentru modul Standard și ProgrammerMainWindow pentru modul Programer). Este folosită arhitectura MVVM pentru a separa logica aplicației de interfața grafică.

# ⚙️ Caracteristici

📐 Mod Standard: Operații aritmetice simple (adunare, scădere, înmulțire, împărțire).

🔢 Mod Programer: Operații pe baze numerice diferite (Bin, Oct, Dec, Hex).

💾 Persistență setări: Aplicația reține ultimul mod utilizat, baza de numeratie și opțiunea Digit Grouping activată sau nu, folosind Properties.Settings.

🎨 Interfață modernă: Interfața grafică este dezvoltată folosind XAML și respectă principiile MVVM.

# 🚀 Cum se folosește

📥 Clonează acest repository:

 git clone https://github.com/utilizator/calculator-wpf.git

📝 Deschide soluția în Visual Studio.

▶️ Compilează și rulează aplicația.

📁 Structura Proiectului

MainWindow.xaml & MainWindow.xaml.cs – Interfața și logica pentru modul Standard.

ProgrammerMainWindow.xaml & ProgrammerMainWindow.xaml.cs – Interfața și logica pentru modul Programer.

StandardCalculatorViewModel.cs – Logica pentru calculatorul standard.

ProgrammerCalculatorViewModel.cs – Logica pentru calculatorul programer.

Properties/Settings.settings – Stocarea persistentă a setărilor utilizatorului.

# 📦 Dependențe

.NET 6.0 (sau mai recent)

WPF (Windows Presentation Foundation)
