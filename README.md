# AI Powered Critical SQL Analysis

## Table of Contents

1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Architecture](#architecture)
4. [Key Components](#key-components)
5. [Installation](#installation)
6. [Usage](#usage)
7. [Configuration](#configuration)
8. [Code Quality Metrics](#code-quality-metrics)
9. [Contributing](#contributing)
10. [License](#license)

## Project Overview

The Critical SQL Analysis Tool is a sophisticated system designed to analyze and evaluate SQL stored procedures based on various code quality, security, maintenance, optimization, and compliance metrics. This tool aims to improve the overall quality and performance of SQL code by providing detailed insights and recommendations.

### Example AI Generated Analysis

```markdown
# Code Quality Metrics

1. Code Review Compliance: 8
   - The stored procedure has undergone basic code review compliance with error handling and validation checks.
   - However, a more detailed review could be beneficial to catch potential logic errors or performance bottlenecks.

2. Use of Parameterized Queries: 9
   - Parameterized queries are used effectively throughout the stored procedure, reducing the risk of SQL injection attacks.

3. Naming Conventions: 7
   - Naming conventions are mostly followed, but some variables and temporary tables could have clearer and more descriptive names for better readability.


{... shortened for readme brevity ...}

```

## Features

- Automated analysis of SQL stored procedures
- Evaluation based on comprehensive metrics
- Detailed reporting on code quality, security, and performance
- Integration with existing development workflows
- Support for large-scale SQL projects

## Architecture

The project follows a modular architecture, leveraging the following key technologies and patterns:

- .NET Core for cross-platform compatibility
- Mediator pattern for decoupling components
- Dependency Injection for managing object lifecycles
- AutoGen for AI-assisted code analysis
- SQL Server for database operations

## Key Components

1. **AnalysisAgent**: Responsible for analyzing file contents using AI-assisted techniques. It utilizes OpenAI's GPT-3.5 model for advanced code analysis.

2. **AgentUtilities**: A utility class that provides methods for creating and configuring agents used in the analysis process. It includes methods for creating user agents and analysis agents with specific configurations.

3. **DocumentationHandler**: Handles the generation of documentation based on analysis results.

4. **ProjectFileHandler**: Manages the processing of project files.

5. **ProjectFileLoader**: Loads SQL files from specified project directories.

6. **Worker**: Background service that orchestrates the analysis process.

## Installation

1. Clone the repository:

   ```shell
   git clone https://github.com/your-username/critical-sql-analysis-tool.git
   ```

2. Navigate to the project directory:

   ```shell
   cd critical-sql-analysis-tool
   ```

3. Restore dependencies:

   ```shell
   dotnet restore
   ```

4. Build the project:

   ```shell
   dotnet build
   ```

## Usage

1. Ensure you have SQL Server installed and accessible.

2. Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. Run the application:

   ```shell
   dotnet run
   ```

4. The tool will automatically scan the specified SQL directories and generate analysis reports.

## Configuration

The tool can be configured through the `appsettings.json` file. Key configuration options include:

- `SqlDirectories`: List of directories containing SQL files to analyze
- `OutputDirectory`: Directory where analysis reports will be generated
- `MetricsFile`: Path to the file containing evaluation metrics
- `OpenAIConfig`: Configuration for the OpenAI integration (API key, model, etc.)

## Code Quality Metrics

By default tool evaluates SQL code based on the following categories of metrics:

***This is completely configuration with natural human language***

1. Code Quality Metrics
   - Code Review Compliance
   - Use of Parameterized Queries
   - Naming Conventions
   - Comment Density
   - Modularization
   - Error Handling
   - Cyclomatic Complexity
   - Code Duplication
   - Consistent Formatting
   - Use of Stored Procedures vs. Inline SQL

2. Security Metrics
   - SQL Injection Vulnerabilities
   - Principle of Least Privilege Implementation
   - Data Encryption at Rest
   - Secure Authentication Mechanisms
   - Access Control Lists (ACLs) Implementation
   - Sensitive Data Masking
   - Security Patch Management

3. Maintenance Metrics
   - Backup Frequency and Reliability
   - Index Fragmentation
   - Unused Indexes
   - Schema Documentation
   - Code Version Control
   - Dependency Tracking
   - Database Consistency Checks
   - Log File Management

4. Optimization Metrics
   - Query Complexity
   - Data Redundancy
   - Consistent Use of Constraints
   - Table Partitioning Strategy
   - Proper Use of Temporary Tables
   - Efficient Use of JOINs

5. Compliance and Best Practices Metrics
   - Adherence to Standards
   - Documentation Completeness
   - Schema Version Control
   - Audit Trail Implementation
   - Data Retention Policy Compliance
   - GDPR Compliance (if applicable)
   - Regular Code Reviews
   - Use of Database Normalization
   - Consistent Naming Conventions across Database Objects
   - Implementation of Database Design Patterns

## Contributing

Contributions to the Critical SQL Analysis Tool are welcome! Please follow these steps to contribute:

1. Fork the repository
2. Create a new branch for your feature or bug fix
3. Make your changes and commit them with descriptive commit messages
4. Push your changes to your fork
5. Submit a pull request to the main repository

Please ensure that your code adheres to the existing coding standards and includes appropriate tests.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
