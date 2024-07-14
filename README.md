
# Boutique E-Commerce

Welcome to the Boutique E-Commerce backend project. This application is developed using ASP.NET Core and follows the onion architecture design pattern. It utilizes MongoDB, PostgreSQL, Redis, and ElasticSearch for data management and RabbitMQ for messaging.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Setup and Installation](#setup-and-installation)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Configuration](#database-configuration)
  - [Environment Variables](#environment-variables)
- [Usage](#usage)
  - [Running the Application](#running-the-application)
  - [Accessing Swagger](#accessing-swagger)
- [API Endpoints](#api-endpoints)
- [Background Services](#background-services)
- [Contributing](#contributing)
- [License](#license)

## Features

- User authentication with JWT and role-based access control
- SMS verification for login
- Product and category management
- Commenting system
- Integration with MongoDB, PostgreSQL, Redis, and ElasticSearch
- RabbitMQ for asynchronous messaging
- Iyzico integration for payment processing including 3D secure payments
- Background services for order processing and stock management

## Technologies Used

- ASP.NET Core
- MongoDB
- PostgreSQL
- Redis
- ElasticSearch
- RabbitMQ
- Iyzico

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
   \`\`\`sh
   git clone https://github.com/uuroren/boutique-ecommerce.git
   cd boutique-ecommerce
   \`\`\`

2. Restore the dependencies:
   \`\`\`sh
   dotnet restore
   \`\`\`

### Database Configuration

#### MongoDB

Make sure MongoDB is installed and running. You can follow the [MongoDB installation guide](https://docs.mongodb.com/manual/installation/) for your operating system.

#### PostgreSQL

Make sure PostgreSQL is installed and running. You can follow the [PostgreSQL installation guide](https://www.postgresql.org/download/) for your operating system.

#### Redis

Make sure Redis is installed and running. You can follow the [Redis installation guide](https://redis.io/download) for your operating system.

#### RabbitMQ

Make sure RabbitMQ is installed and running. You can follow the [RabbitMQ installation guide](https://www.rabbitmq.com/download.html) for your operating system.

#### ElasticSearch

Make sure ElasticSearch is installed and running. You can follow the [ElasticSearch installation guide](https://www.elastic.co/guide/en/elasticsearch/reference/current/install-elasticsearch.html) for your operating system.

### Environment Variables

Set up environment variables in \`appsettings.json\` or create a \`secrets.json\` file for sensitive data:
\`\`\`json
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
  },
  "IyzicoSettings": {
    "ApiKey": "your_iyzico_api_key",
    "SecretKey": "your_iyzico_secret_key",
    "BaseUrl": "your_iyzico_base_url"
  }
}
\`\`\`

## Usage

### Running the Application

Run the application using the following command:
\`\`\`sh
dotnet run
\`\`\`

### Accessing Swagger

Once the application is running, you can access the API documentation and test the endpoints using Swagger by navigating to \`http://localhost:5000/swagger\`.

## API Endpoints

### Authentication

- **POST** \`/api/auth/register\` - Register a new user
- **POST** \`/api/auth/login\` - Login and obtain JWT token
- **POST** \`/api/auth/verify\` - Verify phone number with SMS

### Products

- **GET** \`/api/products\` - Get all products
- **GET** \`/api/products/{id}\` - Get a specific product by ID
- **POST** \`/api/products\` - Create a new product
- **PUT** \`/api/products/{id}\` - Update an existing product
- **DELETE** \`/api/products/{id}\` - Delete a product

### Categories

- **GET** \`/api/categories\` - Get all categories
- **GET** \`/api/categories/{id}\` - Get a specific category by ID
- **POST** \`/api/categories\` - Create a new category
- **PUT** \`/api/categories/{id}\` - Update an existing category
- **DELETE** \`/api/categories/{id}\` - Delete a category

### Comments

- **GET** \`/api/comments\` - Get all comments
- **GET** \`/api/comments/{id}\` - Get a specific comment by ID
- **POST** \`/api/comments\` - Create a new comment
- **PUT** \`/api/comments/{id}\` - Update an existing comment
- **DELETE** \`/api/comments/{id}\` - Delete a comment

### Orders

- **GET** \`/api/orders\` - Get all orders
- **GET** \`/api/orders/{id}\` - Get a specific order by ID
- **POST** \`/api/orders\` - Create a new order
- **PUT** \`/api/orders/{id}\` - Update an existing order
- **DELETE** \`/api/orders/{id}\` - Delete an order

## Background Services

- **Order Processing Service**: Processes orders and updates stock levels.
- **Payment Processing Service**: Handles payment processing with Iyzico.
- **Notification Service**: Sends notifications to users about order status changes.

## Contributing

Contributions are welcome! Please read the [contributing guidelines](CONTRIBUTING.md) before getting started.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Bu şablon, projenizin ihtiyaçlarına göre uyarlanabilir ve genişletilebilir.
