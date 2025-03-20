# BlogApi
A simple Blog API built with ASP.NET Core Web API. It allows users to create, read, update, and delete blog posts. Users can also comment on posts and authenticate using JWT.

## Features
- ✅ User authentication & registration with JWT
- ✅ Create, read, update, delete (CRUD) blog posts
- ✅ Comment on posts (with automatic user tracking)
- ✅ Secure endpoints using authentication & authorization
- ✅ Uses Entity Framework Core with a SQL database

## Installation & Setup

### Prerequisites
- .NET SDK (latest version)
- SQL Server (or any configured database)
- Postman (for testing API endpoints)

### Steps to Run Locally
1. **Clone the repository**  
   ``sh
   git clone https://github.com/UmukoroHub/BlogApi.git
   cd BlogApi
   
---

### ** API Endpoints**
Provide an overview of how to use the API with sample endpoints.  

``markdown
## API Endpoints

### Authentication
- POST /api/auth/register` → Register a new user
- POST /api/auth/login` → Login and get a JWT token

### Blog Posts
- GET /api/blogposts` → Fetch all posts
- POST /api/blogposts` → Create a new post (auth required)
- GET /api/blogposts/{id}` → Get a single post

### Comments
- POST /api/comments` → Add a comment to a post

## Contribution
Want to improve this project? Feel free to fork, create an issue, or submit a pull request. Contributions are always welcome!

## License
This project is licensed under the MIT License.


