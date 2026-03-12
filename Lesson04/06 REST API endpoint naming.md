### REST Endpoint Naming
Imagine that you have a database with CVs whose design is based on the [CV XSD exercise](https://github.com/arturomorarioja-ek/SD_System_Integration_F2026/blob/main/Lesson03/Ex%2002%20CV%20XSD.md). You have to design a REST API based on this data.

Write down the names of the following REST endpoints:
- List CVs. Either the full list or filtered by:
  - Last name
  - Language
- Create a new CV
  
- Get a full CV
- Replace an entire CV
- Partially update a CV
- Delete a CV

- Get personal information for a CV
- Replace personal information for a CV
- Partially update personal information for a CV

- Retrieve the picture file name for a CV
- Update the picture file name for a CV

- Get all degrees for a CV
- Add a new degree to a CV
- Get one degree for a CV
- Replace a degree in a CV
- Delete a degree in a CV

- Logging in
- Logging out

### Solution
- List CVs
  - Full list: `GET /cvs`
  - Filtered by:
    - Last name: `GET /cvs?lastName={last_name}`
    - Language: `GET /cvs?language={language}`
    - Both: `GET /cvs?lastName={last_name}&language={language}`
- Create a new CV: `POST /cvs`
  
- Get a full CV: `GET /cvs/{cvId}`
- Replace an entire CV: `PUT /cvs/{cvId}`
- Partially update a CV: `PATCH /cvs/{cvId}`
- Delete a CV: `DELETE /cvs/{cvId}`

- Get personal information for a CV: `GET /cvs/{cvId}/personal-information`
- Replace personal information for a CV: `PUT /cvs/{cvId}/personal-information`
- Partially update personal information for a CV: `PATCH /cvs/{cvId}/personal-information`

- Retrieve the picture file name for a CV: `GET /cvs/{cvId}/picture`
- Update the picture file name for a CV: `PUT /cvs/{cvId}/picture`

- Get all degrees for a CV: `GET /cvs/{cvId}/degrees`
- Add a new degree to a CV: `POST /cvs/{cvId}/degrees`
- Get one degree for a CV: `GET /cvs/{cvId}/degrees/{degreeId}`
- Replace a degree in a CV: `PUT /cvs/{cvId}/degrees/{degreeId}`
- Delete a degree in a CV: `DELETE /cvs/{cvId}/degrees/{degreeId}`

If authentication is based on sessions:
- Logging in: `POST /auth/sessions`. This endpoint will return a session token
- Logging out: `DELETE /auth/sessions/{sessionId}`. The server will delete a specific session
  
If authentication is based on tokens:
- Logging in: `POST /auth/tokens`. This endpoint will return a token
- Logging out `DELETE /auth/tokens/{tokenId}`. The server will delete a specific session

`/auth/login` and `/auth/logout` are widely used, but they contravene REST naming conventions by expressing operations/processes rather than resources.
