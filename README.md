# Orders API – Clean Architecture + Caching + Logging (.NET 8)

This project is a clean and scalable backend built with **.NET 8 Web API**, following **Clean Architecture** principles and implementing real-world features like caching, logging middleware, and unified API responses.

---

## Features

### **Clean Architecture Structure**
- **Domain** → Entities, Interfaces, Business rules  
- **Application** → Use cases, DTOs, Services  
- **Infrastructure** → EF Core DbContext, Repositories, Redis cache  
- **API** → Controllers, Middleware, Dependency Injection  

### ⚡ **High Performance with Caching**
- Redis distributed cache  
- Cache keys:
  - `order:{id}` → Single order  
  - `orders:list` → Orders list  
- Auto-invalidate cache on:
  - Create → removes `orders:list`
  - Delete → removes both `order:{id}` & `orders:list`

### **Unified API Response**
All endpoints return a consistent format:

```json
{
  "success": true,
  "message": "Order retrieved successfully",
  "data": { ... },
  "timestamp": "2025-01-12T12:00:00Z"
}
