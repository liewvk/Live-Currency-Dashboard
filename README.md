# Live Currency Dashboard

## Project Overview
Live Currency Dashboard is a Visual Basic 2026 desktop application that provides real-time currency exchange rates and financial data visualization. This project is part of the Visual Basic 2026 Programming curriculum (Assignment 45).

## Features
- **Real-time Currency Exchange Rates**: Fetches live currency conversion data from external API services
- **Interactive Dashboard UI**: User-friendly Windows Forms interface for viewing and managing currency data
- **Multi-Currency Support**: Track multiple currency pairs simultaneously
- **Service-Oriented Architecture**: Clean separation of concerns with dedicated service classes
- **MVVM Pattern Implementation**: ViewModel pattern for better data binding and testability

## Project Structure

### Core Components

#### `Form1.vb` (Main Application)
- Primary Windows Forms user interface
- Handles user interactions and UI events
- Displays currency data in a dashboard format
- Manages application workflow and navigation

#### `Form1.Designer.vb`
- Auto-generated designer code for Form1
- Manages UI control definitions and layouts

#### `Form1.resx`
- Resource file containing form resources and localization data

#### `CurrencyDashboardService.vb`
- Main service orchestrator for the dashboard
- Coordinates between different currency services
- Manages data retrieval and processing

#### `CurrencyService.vb`
- Handles currency exchange rate API calls
- Converts and calculates currency values
- Manages currency data caching and updates

#### `CurrencyDashboardViewModel.vb`
- Implements MVVM ViewModel pattern
- Exposes data and commands to the UI
- Handles UI-related logic and data transformations

#### `ApplicationEvents.vb`
- Application-level event handlers
- Manages startup and shutdown operations
- Application lifecycle management

### Project Configuration Files

#### `Live Currency Dashboard.vbproj`
- Visual Basic project configuration
- Defines project properties and dependencies
- Build configuration settings

#### `Live Currency Dashboard.slnx`
- Solution file
- Project organization and structure

#### `.gitignore`
- Git ignore patterns for Visual Basic projects
- Excludes build outputs, user files, and temporary data

#### `.gitattributes`
- Git attributes configuration
- Line ending and file handling rules

### My Project Directory
- Contains application manifest and resources
- Assembly version information
- Application resources and settings

## Technologies Used
- **Language**: Visual Basic .NET
- **Platform**: Windows Desktop (.NET Framework)
- **UI Framework**: Windows Forms
- **Architecture Pattern**: MVVM (Model-View-ViewModel)
- **API Integration**: RESTful web services for currency data

## How to Use

### Prerequisites
- Visual Studio 2022 or later (with VB.NET support)
- .NET Framework 4.7.2 or higher
- Internet connection for live currency data

### Installation
1. Clone the repository:
   ```
   git clone https://github.com/liewvk/Live-Currency-Dashboard.git
   ```

2. Open the solution file:
   - Launch Visual Studio
   - Open `Live Currency Dashboard.slnx`

3. Restore dependencies (if any):
   - Right-click the project → "Restore NuGet Packages"

4. Build the project:
   - Press `Ctrl+Shift+B` or use Build → Build Solution

5. Run the application:
   - Press `F5` or use Debug → Start Debugging

## Application Flow

1. **Startup**: ApplicationEvents.vb initializes the application
2. **UI Load**: Form1 displays the main dashboard interface
3. **Data Fetch**: CurrencyDashboardService requests exchange rates from CurrencyService
4. **Display**: ViewModel updates with fetched data, displayed on Form1
5. **User Interaction**: User selects currencies and views conversion rates
6. **Real-time Updates**: Dashboard refreshes currency data at intervals

## Architecture Highlights

### Service Layer
- `CurrencyService` encapsulates API communication
- `CurrencyDashboardService` coordinates services
- Separation of concerns for maintainability

### Presentation Layer
- Windows Forms UI in Form1
- ViewModel pattern for data binding
- Clean separation between UI and business logic

### Event-Driven Architecture
- Event handlers for user interactions
- Application lifecycle management through events

## Development Notes
- This is an educational project for Visual Basic 2026 Programming
- Follow VB.NET coding standards and best practices
- Use meaningful variable and method names
- Implement proper error handling and validation

## Future Enhancements
- Add historical currency rate tracking
- Implement data persistence/database storage
- Add support for cryptocurrency rates
- Create graph/chart visualizations
- Multi-language support
- User preferences and settings management

## Troubleshooting

### Application Won't Start
- Ensure all dependencies are installed
- Check .NET Framework version compatibility
- Verify internet connection for API calls

### Currency Data Not Loading
- Check internet connectivity
- Verify API service is operational
- Review application logs for errors

### UI Issues
- Clear Visual Studio cache and rebuild
- Ensure screen DPI settings are correct
- Try running in compatibility mode if needed

## License
This project is created for educational purposes as part of Visual Basic 2026 Programming coursework.

## Author
**liewvk** - Visual Basic Developer

## Contributing
As an educational project, contributions should follow course guidelines and be approved by the course instructor.

---

**Last Updated**: September 2, 2026  
**Project Status**: In Development  
**Version**: 1.0.0