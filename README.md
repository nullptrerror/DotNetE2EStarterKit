# Selenium + NUnit + Azure DevOps Starter Guide

Welcome to the Selenium + NUnit + Azure DevOps project. This README will guide you through setting up your development environment and getting started with our testing suite.

## Getting Started

### Prerequisites

- **Visual Studio Code**: Download and install from [here](https://code.visualstudio.com/).
- **Git**: Download and install from [here](https://git-scm.com/).
- **.NET SDK**: Download and install from [here](https://dotnet.microsoft.com/download).
- **Azure DevOps Server**: Access the web portal from [your server](http://{your-server}:8080/tfs/).

### Setup

1. **Clone the Repository**: Open Visual Studio Code, press `F1`, type `Git: Clone`, and paste the repository URL (https://pbcdevops20t.pbcgov.org/tfs/{project-collection}/{project}/_git/{repository}). Use your Azure DevOps Server credentials for authentication.

2. **Open the Repository**: Once cloned, open the repository in VSCode.

3. **Restore NuGet Packages**: Open the terminal in VSCode (`Ctrl+~`) and run `dotnet restore` to restore the required NuGet packages.

4. **Build the Project**: Run `dotnet build` to compile the project.

### Running the Tests

1. **Open Test Explorer**: Go to the `View` menu and select `Test Explorer`.

2. **Run All Tests**: In the Test Explorer, click the `Run All Tests` icon.

3. **View Test Results**: Once the tests run, you can view the results in the Test Explorer.

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

## Support

If you have any questions or need assistance, please reach out to the team lead or any team member.
