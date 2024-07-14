# Boutique E-Commerce

Welcome to the Boutique E-Commerce backend project developed using ASP.NET Core. This application follows the onion architecture design pattern and leverages MongoDB, PostgreSQL, Redis, and ElasticSearch for data storage and processing. Additionally, it integrates RabbitMQ for messaging.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Setup and Installation](#setup-and-installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Features

- User authentication with JWT and role-based access control
- SMS verification for login
- Product and category management
- Commenting system
- Integration with MongoDB, PostgreSQL, Redis, and ElasticSearch
- RabbitMQ for asynchronous messaging

## Technologies Used

- ASP.NET Core
- MongoDB
- PostgreSQL
- Redis
- ElasticSearch
- RabbitMQ

## Architecture

This project uses the onion architecture to ensure maintainability and testability.

- **Core**: Contains business logic and domain entities.
- **Infrastructure**: Contains data access implementations, external service integrations, and configurations.
- **Application**: Contains application services and use cases.
- **API**: Contains controllers and API endpoints.

## Setup and Installation

### Prerequisites

- .NET 8 SDK
- MongoDB
- PostgreSQL
- Redis
- RabbitMQ
- ElasticSearch

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/uuroren/boutique-e-commerce.git
   cd boutique-e-commerce
   ```

2. Set up environment variables in `appsettings.json` or create a `secrets.json` file for sensitive data:
   ```json
   {
     "ConnectionStrings": {
       "MongoDB": "your_mongodb_connection_string",
       "PostgreSQL": "your_postgresql_connection_string"
     },
     "JwtSettings": {
       "Key": "your_jwt_secret_key",
       "Issuer": "your_issuer",
       "Audience": "your_audience"
     },
     "SmsSettings": {
       "Provider": "your_sms_provider",
       "ApiKey": "your_sms_api_key"
     }
   }
   ```

3. Restore the dependencies and run the application:
   ```sh
   dotnet restore
   dotnet run
   ```

## Usage

Once the application is running, you can access the API documentation and test the endpoints using Swagger by navigating to `http://localhost:5000/swagger`.

## API Endpoints

### Authentication

- **POST** `/api/auth/register` - Register a new user
- **POST** `/api/auth/login` - Login and obtain JWT token
- **POST** `/api/auth/verify` - Verify phone number with SMS

### Products

- **GET** `/api/products` - Get all products
- **GET** `/api/products/{id}` - Get a specific product by ID
- **POST** `/api/products` - Create a new product
- **PUT** `/api/products/{id}` - Update an existing product
- **DELETE** `/api/products/{id}` - Delete a product

### Categories

- **GET** `/api/categories` - Get all categories
- **GET** `/api/categories/{id}` - Get a specific category by ID
- **POST** `/api/categories` - Create a new category
- **PUT** `/api/categories/{id}` - Update an existing category
- **DELETE** `/api/categories/{id}` - Delete a category

### Comments

- **GET** `/api/comments` - Get all comments
- **GET** `/api/comments/{id}` - Get a specific comment by ID
- **POST** `/api/comments` - Create a new comment
- **PUT** `/api/comments/{id}` - Update an existing comment
- **DELETE** `/api/comments/{id}` - Delete a comment

---
