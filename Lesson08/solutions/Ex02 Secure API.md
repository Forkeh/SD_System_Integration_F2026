### Secure API

Work in groups of 4 or 5 students.

A team has developed a small API for a course project submission system. The API works functionally, but its security design is incomplete.

The system has four user roles:

- **Guest**: not logged in
- **Student**: can manage their own project submissions
- **Teacher**: can view and grade student submissions
- **Admin**: can manage users

The API currently exposes these endpoints:

| Method | Endpoint                  | Purpose                                  |
| ------ | ------------------------- | ---------------------------------------- |
| POST   | `/login`                  | Log in                                   |
| GET    | `/projects`               | List all project titles and descriptions |
| GET    | `/projects/{id}`          | View one project                         |
| POST   | `/projects`               | Create a new project                     |
| PUT    | `/projects/{id}`          | Update a project                         |
| DELETE | `/projects/{id}`          | Delete a project                         |
| POST   | `/projects/{id}/submit`   | Submit work for a project                |
| GET    | `/submissions`            | List submissions                         |
| GET    | `/submissions/{id}`       | View one submission                      |
| PATCH  | `/submissions/{id}/grade` | Grade a submission                       |
| GET    | `/users`                  | List users                               |
| POST   | `/users`                  | Create a user                            |

Additional information:

- The API can currently be reached over both HTTP and HTTPS
- Failed logins return either Unknown username or Wrong password
- All logged-in users currently have access to all authenticated endpoints
- No rate limiting has been configured
- No request size limits have been configured
- Input validation exists only partially
- Logging has not yet been designed
- Security headers have not yet been chosen
- Password hashing is required, but not yet implemented

---

**1. Endpoint authentication**

For each endpoint, decide whether access should be:

- Public
- Authenticated. If affirmative, specify to which role(s) should access be restricted

| Method | Endpoint                  | Purpose                                  | Auth Role        |
| ------ | ------------------------- | ---------------------------------------- | ---------------- |
| POST   | `/login`                  | Log in                                   | Public           |
| GET    | `/projects`               | List all project titles and descriptions | Student, Teacher |
| GET    | `/projects/{id}`          | View one project                         | Student, Teacher |
| POST   | `/projects`               | Create a new project                     | Teacher          |
| PUT    | `/projects/{id}`          | Update a project                         | Teacher          |
| DELETE | `/projects/{id}`          | Delete a project                         | Teacher          |
| POST   | `/projects/{id}/submit`   | Submit work for a project                | Student          |
| GET    | `/submissions`            | List submissions                         | Teacher          |
| GET    | `/submissions/{id}`       | View one submission                      | Student, Teacher |
| PATCH  | `/submissions/{id}/grade` | Grade a submission                       | Teacher          |
| GET    | `/users`                  | List users                               | Teacher, Admin   |
| POST   | `/users`                  | Create a user                            | Admin            |

---

**2. Access control**

Is RBAC sufficient for this API, or would ABAC be necessary?

No, For example the "view one submission" endpoint is set to Student and Teacher- role, but you should only be allowed to view your own submission and not everyone eleses in detail. A fine-grained access control is necessary.

---

**3. Rate limiting**

The team decides to limit each endpoint to a maximum number of requests per minute per client with the following rate limiting criteria:

- Strict: 5 requests/minute
- Moderate: 30 requests/minute
- Relaxed: 120 requests/minute

1. Indicate which endpoint belongs in each of the three approaches
2. Which is the heaviest endpoint?

| Method | Endpoint                  | Purpose                                  | Rate-limit |
| ------ | ------------------------- | ---------------------------------------- | ---------- |
| POST   | `/login`                  | Log in                                   | Strict     |
| GET    | `/projects`               | List all project titles and descriptions | Relaxed    |
| GET    | `/projects/{id}`          | View one project                         | Relaxed    |
| POST   | `/projects`               | Create a new project                     | Moderate   |
| PUT    | `/projects/{id}`          | Update a project                         | Moderate   |
| DELETE | `/projects/{id}`          | Delete a project                         | Relaxed    |
| POST   | `/projects/{id}/submit`   | Submit work for a project                | Strict     |
| GET    | `/submissions`            | List submissions                         | Relaxed    |
| GET    | `/submissions/{id}`       | View one submission                      | Relaxed    |
| PATCH  | `/submissions/{id}/grade` | Grade a submission                       | Moderate   |
| GET    | `/users`                  | List users                               | Relaxed    |
| POST   | `/users`                  | Create a user                            | Moderate   |

- Submit work for a project is heaviest.
