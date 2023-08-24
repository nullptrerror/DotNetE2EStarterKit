# Project Setup - E2E Test Project with Selenium, NUnit, and Azure DevOps

## Prerequisites

- **Visual Studio Code**: Download and install from [here](https://code.visualstudio.com/).
- **Git**: Download and install from [here](https://git-scm.com/).
- **.NET SDK**: Download and install from [here](https://dotnet.microsoft.com/download).
- **Azure DevOps Server**: Access the web portal from [your server](http://{your-server}:8080/tfs/).

## Project Structure Setup

### If the Project Solution Already Exists

1. **Clone the Existing Repository**
   - Clone the existing repository to your local machine using Visual Studio Code.

     ```bash
     git clone https://pbcdevops20t.pbcgov.org/tfs/{project-collection}/{project}/_git/{repository}
     ```

2. **Navigate to the Repository Directory**
   - Open the terminal and navigate to the cloned repository directory.

3. **Create a New Test Project**
   - Run the following command to create a new nUnit test project with the specified name (e.g., `AP11_SRS_E2E`):

     ```bash
     dotnet new nunit -n AP11_SRS_E2E
     ```

4. **Add the Test Project to the Solution**
   - Run the following command to add the newly created test project to the existing solution:

     ```bash
     dotnet sln add AP11_SRS_E2E/AP11_SRS_E2E.csproj
     ```

### If the Project Solution Does Not Exist

1. **Create a New Repository**
   - Go to the Azure DevOps web portal and create a new Git repository with the desired name (e.g., `AP11_SRS_TEST`).

2. **Clone the Repository**
   - Clone the new repository to your local machine using Visual Studio Code.

     ```bash
     git clone https://pbcdevops20t.pbcgov.org/tfs/{project-collection}/{project}/_git/{repository}
     ```

3. **Navigate to the Repository Directory**
   - Open the terminal and navigate to the cloned repository directory.

4. **Create a New Solution**
   - Run the following command to create a new solution with the specified name (e.g., `AP11_SRS_TEST`):

     ```bash
     dotnet new sln -n AP11_SRS_TEST
     ```

5. **Create a New Test Project**
   - Run the following command to create a new nUnit test project with the specified name (e.g., `AP11_SRS_E2E`):

     ```bash
     dotnet new nunit -n AP11_SRS_E2E
     ```

6. **Add the Test Project to the Solution**
   - Run the following command to add the newly created test project to the solution:

     ```bash
     dotnet sln add AP11_SRS_E2E/AP11_SRS_E2E.csproj
     ```

### Common Steps

1. **Customize the Test Project**
   - Navigate to the test project directory (`AP11_SRS_E2E`) and open it in Visual Studio Code.
   - Add your Selenium and NUnit tests as needed.
   - Remember to install any necessary NuGet packages using the `dotnet add package` command.
   - Add the dependencies by running the following commands in the terminal:

     ```bash
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package DotNetSeleniumExtras.WaitHelpers --version 3.11.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Microsoft.Extensions.Configuration --version 7.0.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Microsoft.Extensions.Configuration.Binder --version 7.0.4
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Microsoft.Extensions.Configuration.FileExtensions --version 7.0.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Microsoft.Extensions.Configuration.Json --version 7.0.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Microsoft.NET.Test.Sdk --version 17.6.3
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package NUnit --version 3.13.3
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package NUnit3TestAdapter --version 4.5.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package NUnit.Analyzers --version 3.6.1
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package coverlet.collector --version 6.0.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package Selenium.WebDriver --version 4.11.0
     dotnet add AP11_SRS_E2E/AP11_SRS_E2E.csproj package WebDriverManager --version 2.17.0
     ```

2. **Commit and Push Changes**
   - Run the following commands to add, commit, and push the changes to the `development` branch:

     ```bash
     git checkout -b development
     git add .
     git commit -m "Initial commit with E2E test project and dependencies"
     git push origin development
     ```

## Additional Resources

- [Selenium Documentation](https://www.selenium.dev/documentation/en/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Azure DevOps Server Documentation](https://learn.microsoft.com/en-us/azure/devops/server/admin/setup-overview?view=azure-devops)

## Support

If you have any questions or need assistance, please reach out to the team lead or any team member.
