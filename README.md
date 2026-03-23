# WorkTracker

A Windows Forms application (.NET 9) to track employee attendance using SQLite and Dapper.

## Features
- Track employee attendance (Present, Absent, Remote)
- Recurring absences (e.g. every Monday)
- Monthly calendar view
- Weekly report with clipboard export

## Getting Started
1. Open `WorkTracker.sln` in Visual Studio 2022
2. Build the solution (NuGet packages restore automatically)
3. Run with F5

The SQLite database (`worktracker.db`) is created automatically on first run.