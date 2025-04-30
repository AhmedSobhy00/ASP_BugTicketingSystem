
# 🐞 Bug Ticketing System – API Documentation




## 🔐 User Management

### 🔸 Register User  
**Endpoint:** `POST /api/users/register`  
Registers a new user account.

**Request Body:**
```json
{
  "username": "Ahmed",
  "password": "SecurePassword123",
  "role": "Developer"
}
```

### 🔸 Login User  
**Endpoint:** `POST /api/users/login`  
Authenticates a user and returns a JWT token.

**Request Body:**
```json
{
  "username": "Ahmed",
  "password": "SecurePassword123"
}
```

**Response:**
```json
{
  "token": "jwt_token_here"
}
```

---

## 🗂️ Project Management

### 🔸 Create Project  
**Endpoint:** `POST /api/projects`  
Creates a new project.

**Request Body:**
```json
{
  "projectName": "Bug Tracker App",
  "description": "A system for managing software bugs"
}
```

### 🔸 Get All Projects  
**Endpoint:** `GET /api/projects`  
Returns a list of all projects.

**Response:**
```json
[
  {
    "id": "guid",
    "projectName": "Bug Tracker App",
    "description": "A system for managing software bugs"
  }
]
```

### 🔸 Get Project Details  
**Endpoint:** `GET /api/projects/:id`  
Returns detailed info about a specific project including its bugs.

**Response:**
```json
{
  "id": "guid",
  "projectName": "Bug Tracker App",
  "description": "A system for managing software bugs",
  "bugs": [
    {
      "id": "guid",
      "name": "Login crash",
      "description": "App crashes when user logs in"
    }
  ]
}
```

---

## 🐛 Bug Management

### 🔸 Create Bug  
**Endpoint:** `POST /api/bugs`  
Creates a new bug.

**Request Body:**
```json
{
  "name": "Login crash",
  "description": "App crashes when logging in",
  "projectId": "guid"
}
```

### 🔸 Get All Bugs  
**Endpoint:** `GET /api/bugs`  
Returns all bugs.

**Response:**
```json
[
  {
    "id": "guid",
    "name": "Login crash",
    "description": "App crashes when logging in",
  }
]
```

### 🔸 Get Bug Details  
**Endpoint:** `GET /api/bugs/:id`  
Returns detailed information about a specific bug.

**Response:**
```json
{
  "id": "guid",
  "name": "Login crash",
  "description": "App crashes when logging in",
  "projectId": "guid",
}
```

---

## 👥 User-Bug Assignment

### 🔸 Assign User to Bug  
**Endpoint:** `POST /api/bugs/:id/assignees`  
Assigns a user to a bug.

**Request Body:**
```json
{
  "userId": "guid"
}
```

### 🔸 Remove User from Bug  
**Endpoint:** `DELETE /api/bugs/:id/assignees/:userId`  
Unassigns a user from a bug.

---

## 📎 File Attachments

### 🔸 Upload Attachment  
**Endpoint:** `POST /api/bugs/:id/attachments`  
Uploads a file to a bug (multipart/form-data).

**Form Field:**
- `file`: image or document file

### 🔸 Get Attachments for Bug  
**Endpoint:** `GET /api/bugs/:id/attachments`  
Returns all attachments for a bug.

**Response:**
```json
[
  {
    "id": "guid",
    "filePath": "error-log.png",
  }
]
```

### 🔸 Delete Attachment  
**Endpoint:** `DELETE /api/bugs/:id/attachments/:attachmentId`  
Removes an attachment from a bug.

---

## 🧾 Notes

- All endpoints that modify data require **Authorization Header** with a valid **Bearer token**.
- Only **Managers** may create projects or assign bugs.
- Users can only see or edit bugs they are assigned to unless they are Managers.
