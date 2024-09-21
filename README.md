# Account Transaction Management

# Design Decisions and Implementation Challenges

## Overview
The AccountTransactionManagement project is designed to manage accounts and transactions, providing functionality for creating, reading, updating, and deleting (CRUD) entries while maintaining the integrity of account balances.

## Design Decisions

## Features
- **CRUD Operations**: Create, Read, Update, and Delete transactions and accounts.
- **Balance Management**: Automatically updates account balances based on transactions.
- **Unit Testing**: Comprehensive unit tests for all operations to ensure functionality and maintainability.

### Data Model:

- The Account class includes properties such as Name, Number, CurrentBalance, and OverdraftLimit. The Transaction class includes Description, DebitCredit, Amount, and AccountId.
- Relationships between accounts and transactions are established using a one-to-many relationship, ensuring that transactions are properly linked to their respective accounts.


### In-Memory Database for Testing:
- An in-memory SQLite database is used for unit testing to isolate tests from production data, ensuring that each test runs in a clean environment.
This allows for quick setup and teardown of data between tests.
Separation of Concerns:
- Services such as AccountService and TransactionService handle business logic, keeping the database context focused on data operations.
This separation facilitates easier unit testing and enhances code maintainability.


### Error Handling:
- Exception handling is implemented to manage errors gracefully, such as attempting to add transactions to non-existent accounts or deleting non-existent transactions.


## Challenges Faced

### Managing Transaction Logic:
- Ensuring that account balances are updated correctly with each transaction posed a challenge. It required careful implementation of logic to handle both credit and debit transactions accurately.
- Solution: Implemented a method in the TransactionService that updates the account balance after each transaction is added or modified.


### Testing Setup:
- Setting up an in-memory database for unit tests required additional configuration and understanding of Entity Framework Core's test capabilities.
Solution: Created helper methods to streamline the creation of a new context for each test, ensuring isolation and consistency.


### Handling Concurrency:
- Deleting and updating transactions raised concerns about data consistency and concurrency issues.
- Solution: Implemented optimistic concurrency control with appropriate exception handling to manage cases where transactions are not found.


### Validating Input:
- Ensuring that input values for transactions and accounts are valid (e.g., amounts cannot be negative) required additional validation logic.
- Solution: Incorporated validation checks in the service layer to prevent invalid entries from being processed.


## Conclusion
- The AccountTransactionManagement project effectively manages account and transaction data with a clear structure and robust error handling. The challenges encountered were addressed through careful planning and implementation, ensuring that the application is both functional and maintainable.



## Running the Project

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- An integrated development environment (IDE) like Visual Studio, Visual Studio Code, or JetBrains Rider.

### Clone the Repository
Open a terminal and clone the repository using:
```bash
git clone <repository-url>
cd AccountTransactionManagement
dotnet clean
dotnet ef migrations and InitialCreate
dotnet ef database update
dotnet build

dotnet run

(Check for the migrations folder - if any file present delete the file)

-- to run test 
cd ../Tests
dotnet clean
dotnet build
dotnet test
