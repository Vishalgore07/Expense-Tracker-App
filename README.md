# Expense Tracker App

## Overview
The **Expense Tracker App** is a web application designed to help users track their income, expenses, and overall financial balance. It provides a dashboard that displays financial summaries, such as total income, total expenses, balance, and visual representations of expenses by category and income vs. expenses over time.

### Features:
- **Total Income & Expenses Summary**: View the total income and expenses for the last 7 days.
- **Balance Calculation**: Automatically calculates the remaining balance based on income and expenses.
- **Charts and Graphs**:
  - Doughnut Chart: Displays expenses by category.
  - Spline Chart: Shows a comparison of daily income and expenses.
- **Category Management**: Expenses and income are categorized for easier tracking and reporting.

## Tech Stack

### Backend:
- **ASP.NET Core MVC**: The application is built using the ASP.NET Core MVC framework.
- **Entity Framework Core**: For database interactions.
- **SQL Server**: Used for storing transaction data.

### Frontend:
- **Razor Pages**: For server-side rendering of the UI.
- **Syncfusion Charts**: For rendering interactive charts (Doughnut and Spline).
- **Bootstrap 5**: For styling and responsive design.
- **FontAwesome**: For icons used in the UI.

### Database:
- **SQL Server**: The application uses SQL Server for data persistence.
- **Entity Models**:
  - `Transaction`: Represents a financial transaction (income or expense).
  - `Category`: Represents different categories for transactions.
