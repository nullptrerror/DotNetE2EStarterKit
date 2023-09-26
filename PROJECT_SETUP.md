# Project Setup - E2E Test Project with Selenium, NUnit, and DevOps

## Project Structure Setup

### If the Project Solution Already Exists

1. **Clone the Existing Repository**
   - Clone the existing repository to your local machine using Visual Studio Code.

     ```bash
     git clone https://github.com/nullptrerror/DotNetE2EStarterKit.git C:\LocalRepo\DotNetE2EStarterKit\
     ```

2. **Navigate to the Repository Directory**
   - Open the terminal and navigate to the cloned repository directory.

3. **Create a New Test Project**
   - Run the following command to create a new nUnit test project with the specified name (e.g., `NUnit_E2E`):

     ```bash
     dotnet new nunit -n NUnit_E2E
     ```

4. **Add the Test Project to the Solution**
   - Run the following command to add the newly created test project to the existing solution:

     ```bash
     dotnet sln add NUnit_E2E/NUnit_E2E.csproj
     ```

### If the Project Solution Does Not Exist

1. **Create a New Repository**
   - Go to the Azure DevOps web portal and create a new Git repository with the desired name (e.g., `E2E_TEST`).

2. **Clone the Repository**
   - Clone the new repository to your local machine using Visual Studio Code.

     ```bash
     git clone https://github.com/nullptrerror/DotNetE2EStarterKit.git C:\LocalRepo\DotNetE2EStarterKit\
     ```

3. **Navigate to the Repository Directory**
   - Open the terminal and navigate to the cloned repository directory.

4. **Create a New Solution**
   - Run the following command to create a new solution with the specified name (e.g., `E2E_TEST`):

     ```bash
     dotnet new sln -n E2E_TEST
     ```

5. **Create a New Test Project**
   - Run the following command to create a new nUnit test project with the specified name (e.g., `NUnit_E2E`):

     ```bash
     dotnet new nunit -n NUnit_E2E
     ```

6. **Add the Test Project to the Solution**
   - Run the following command to add the newly created test project to the solution:

     ```bash
     dotnet sln add NUnit_E2E/NUnit_E2E.csproj
     ```

### Common Steps

1. **Customize the Test Project**
   - Navigate to the test project directory (`NUnit_E2E`) and open it in Visual Studio Code.
   - Add your Selenium and NUnit tests as needed.
   - Remember to install any necessary NuGet packages using the `dotnet add package` command.
   - Add the dependencies by running the following commands in the terminal:

     ```bash
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Microsoft.Extensions.Configuration
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Microsoft.Extensions.Configuration.Binder
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Microsoft.Extensions.Configuration.FileExtensions
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Microsoft.Extensions.Configuration.Json
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Allure.NUnit --version 2.10.0-preview.1
     dotnet add NUnit_E2E/NUnit_E2E.csproj package Microsoft.NET.Test.Sdk
     dotnet add NUnit_E2E/NUnit_E2E.csproj package NUnit --version 3.13.3
     dotnet add NUnit_E2E/NUnit_E2E.csproj package NUnit3TestAdapter --version 4.5.0
     dotnet add NUnit_E2E/NUnit_E2E.csproj package NUnit.Analyzers --version 3.6.1
     dotnet add NUnit_E2E/NUnit_E2E.csproj package coverlet.collector --version 6.0.0
     ```

## Support

If you have any questions or need assistance, please reach out to the team lead or any team member.
