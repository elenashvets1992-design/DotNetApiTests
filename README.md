## API Tests
Overview

This project contains automated API tests for the public service
https://jsonplaceholder.typicode.com

The tests cover CRUD operations for the /posts endpoint and are implemented using:

- .NET 8
- NUnit
- HttpClient

## Project Structure

ApiTests/

 - Clients/ (API client wrapper (ApiClient, ApiResponse))
 -  Models/( Request/response models)
 - Tests/ (Test cases)
 - Utils/ (Logging and configuration (ApiLogger, AppSettings))

## Configuration

The base URL is defined in:

Utils/AppSettings.cs
public static string BaseUrl => "https://jsonplaceholder.typicode.com";

## Running Tests

Restore dependencies: dotnet restore

Run tests: dotnet test

## Test Scenarios

1) GET /posts
Verify response status is 200 OK
Verify total number of posts is 100
Validate post structure (id, userId, title, body)

2) GET /posts/{id}
Verify fetching a specific post by ID
Verify handling of non-existent post
Note: the API returns 200 OK with an empty body {} instead of 404 NotFound

3) POST /posts
Verify response status is 201 Created
Verify response body contains submitted data

4) PUT /posts/{id}
Verify updating an existing post
Validate updated fields in response

5) DELETE /posts/{id}
Verify response status is 200 OK
Verify response body is empty or {}

## Framework Design

The project follows a simple layered structure:

- ApiClient - handles all HTTP requests, serialization, and logging
- BaseTest – shared setup for tests
- ApiLogger – centralized request/response logging
- Models – strongly typed request and response objects
- AppSettings – configuration for base URL

## Logging

All HTTP requests and responses are logged via ApiLogger for debugging purposes.

## Notes

JSONPlaceholder is a mock API and does not behave like a fully production-ready backend:

- DELETE operations do not persist changes
- Non-existent resources may return 200 OK with {} instead of 404

## Summary

This project demonstrates:

- Clean code organization
- Reusable API client architecture
- Strongly typed models
- Centralized logging
- Maintainable and readable test cases
