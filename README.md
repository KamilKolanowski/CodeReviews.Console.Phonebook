# Phonebook App

## Overview

The **Phonebook App** is a C# application built using **Entity Framework Core (EF Core)** and **SQL Server**. This application allows users to perform CRUD (Create, Read, Update, Delete) operations on contacts. Contacts can include information such as:

- **First Name**
- **Last Name**
- **Phone Number**
- **Email Address**
- **Category** (e.g., Family, Work, etc.)

Users can view all the contacts or filter them based on their category. Additionally, the app includes a feature to send emails to a contact directly.

---

## Features

- Add, update, delete, and view contacts.
- Filter contacts by category (e.g., Family, Work).
- Send an email to a specific contact.

---

## Setup Instructions

### Step 1: Create Database Tables

To run the application, you need to generate the database tables using the following commands in your terminal:

```bash
dotnet ef migrations add initCreate
dotnet ef database update
```

### Step 2: Update `appsettings.json`

Modify the `appsettings.json` file to match the following:

1. Your database server credentials.
2. Your Gmail SMTP **Username** and **Password** for sending emails.
3. Your Twilio **Account SID** and **Authorization Token** for sending SMS.
---

## Prerequisites

Ensure that you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server
- EF Core tools (can be installed by running: `dotnet tool install --global dotnet-ef`)

---

## Packages Used

The app leverages the following NuGet packages:

1. [MailKit](https://www.nuget.org/packages/MailKit)  
   For sending emails.

2. [Microsoft.EntityFrameworkCore.SqlServer.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.Design)  
   Tools for working with EF Core and SQL Server.

3. [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)  
   EF Core provider for SQL Server.

4. [Microsoft.EntityFrameworkCore.SqlServer.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer.Tools)  
   Tools for managing EF Core migrations with SQL Server.

5. [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)  
   EF Core library for building models and querying databases.

6. [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)  
   For binding configuration settings from `appsettings.json`.

7. [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json)  
   For JSON configuration file support.

8. [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)  
   For .NET configuration support.

9. [Spectre.Console](https://www.nuget.org/packages/Spectre.Console)  
   For creating a rich console UI.

---

## Usage

Once the database tables are created and the `appsettings.json` file is configured:

1. Run the application using the .NET CLI or your preferred IDE.
2. Use the console interface to:
    - Manage contacts (add, update, delete, view).
    - Send emails or SMS (*Added in code +48 to match Polish Numbers, you can adjust it to your needs in **SmsOperation() within MessageController.cs**.*) to specific contacts from your Phone Book.

---

## Contributions

Contributions are welcome! Feel free to fork the repository and submit pull requests.

---

## Contact

If you have any questions or issues, feel free to reach out.
