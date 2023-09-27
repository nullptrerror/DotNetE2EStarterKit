# Playwright + NUnit + DevOps Starter Guide

Welcome to the Playwright + NUnit + Azure DevOps project.
This README will guide you through setting up your development environment and getting started with our testing suite.

[![C# Playwright + NUnit + Allure .NET 6.x.x](https://github.com/nullptrerror/DotNetE2EStarterKit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nullptrerror/DotNetE2EStarterKit/actions/workflows/dotnet.yml)

## Getting Started

### Prerequisites (for Development)

- **Visual Studio Code**: Download and install from [here](https://code.visualstudio.com/).
- **Git**: Download and install from [here](https://git-scm.com/).
- **.NET SDK**: Download and install from [here](https://dotnet.microsoft.com/download).
- **DevOps Server**: Github or Azure DevOps Server (TFS) account.

#### Optional (for Allure Reports)

- **Java**: Download and install from [here](https://www.java.com/en/download/).
- **Node.js**: Download and install from [here](https://nodejs.org/en/).
- **Allure Commandline**: Download and install from [here](https://www.npmjs.com/package/allure-commandline).
  - Allure requires `Java 8` or higher
  - Allure requires `Node.js 10` or higher
  - Install with `npm install -g allure-commandline --save-dev`

### Setup

0. **Pick a Directory**: Choose a directory to clone the repository to. For example, `C:\LocalRepo\DotNetE2EStarterKit\`.

1. **Clone the Repository**: Open a terminal and type `git clone https://github.com/nullptrerror/DotNetE2EStarterKit.git C:\LocalRepo\DotNetE2EStarterKit\`.

2. **Cd to Repository**: Change directory to the repository with `cd C:\LocalRepo\DotNetE2EStarterKit\`.

3. **Restore NuGet Packages**: Run `dotnet restore` to restore the NuGet packages.

4. **Cd to NUnit_E2E**: Change directory to the NUnit_E2E project with `cd C:\LocalRepo\DotNetE2EStarterKit\NUnit_E2E`.

5. **Tool Update Powershell**: Run `dotnet tool update --global PowerShell` to update the PowerShell tool.

6. **Install Required Browsers**: Run `pwsh NUnit_E2E/bin/Debug/net6.0/playwright.ps1 install`

7. **Build the Project**: Run `dotnet build` to compile the project.

#### Playwright Codegen (Optional)

1. **Generate Code**: Run `pwsh NUnit_E2E/bin/Debug/net6.0/playwright.ps1 codegen` to generate the code.

2. **If the pwsh command does not work**: Run `dotnet tool update --global PowerShell` to update the PowerShell tool.

### Running the Tests

1. **Test the Project**: Run `dotnet test` to run the tests.

### Generating Allure Reports

1. **Generate Allure Results**: Run `allure generate .\NUnit_E2E\bin\Debug\net6.0\allure-results\ -o .\NUnit_E2E\bin\Debug\net6.0\allure-report\` to generate the Allure results.

2. **Open Allure Results**: Run `allure open .\NUnit_E2E\bin\Debug\net6.0\allure-report\ -p 61494` to open the Allure results in your browser at <http://localhost:61494/index.html>.

### Contributing

1. **Create a Branch**: Before making any changes, create a new branch. Use the format `feature/{feature-name}` or `bugfix/{bug-name}`.

2. **Make Changes**: Make your changes, ensuring you follow the coding standards and best practices.

3. **Commit and Push**: Commit your changes and push your branch to the Azure DevOps Server repository.

4. **Create a Pull Request**: Go to the Azure DevOps Server web portal and create a Pull Request for your branch.

5. **Review and Merge**: After a successful review, your changes will be merged into the main branch.

## Additional Resources

- [Selenium Documentation](https://www.selenium.dev/documentation/en/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Azure DevOps Server Documentation](https://learn.microsoft.com/en-us/azure/devops/server/admin/setup-overview?view=azure-devops)
- [Dotnet Allure Csharp](https://github.com/allure-framework/allure-csharp/tree/main/Allure.NUnit.Examples)
- [Npm Allure Commandline](https://github.com/allure-framework/allure-npm)
- [Java Allure reference guide](https://docs.qameta.io/allure/#_commandline)

## Support

If you have any questions or need assistance, please reach out to the team lead or any team member.
