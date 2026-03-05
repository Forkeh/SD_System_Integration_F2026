### REST Endpoint Naming

Imagine that you have a database with CVs whose design is based on the [CV XSD exercise](https://github.com/arturomorarioja-ek/SD_System_Integration_F2026/blob/main/Lesson03/Ex%2002%20CV%20XSD.md). You have to design a REST API based on this data.

Write down the names of the following REST endpoints:

- List CVs. `GET /cvs`
  Either the full list or filtered by: - Last name `GET /cvs?last-name=hansen` - Language `GET /cvs?language=dk`

- Create a new CV `POST /cvs`

- Get a full CV `GET /cvs/{id}`

- Replace an entire CV `PUT /cvs/{id}`

- Partially update a CV `PATCH /cvs/{id}`

- Delete a CV `DELETE /cvs/{id}`

- Get personal information for a CV `GET /cvs/{id}/personal-information`

- Replace personal information for a CV `PUT /cvs/{id}/personal-information`

- Partially update personal information for a CV `PATCH /cvs/{id}/personal-information`

- Retrieve the picture file name for a CV `GET /cvs/{id}/personal-information/picture-file-name`
- Update the picture file name for a CV `PUT /cvs/{id}/personal-information/picture-file-name`

- Get all degrees for a CV `GET /cvs/{id}/degrees`
- Add a new degree to a CV `POST /cvs/{id}/degrees`
- Get one degree for a CV `GET /cvs/{cvId}/degrees/{degreeId}`
- Replace a degree in a CV `PUT /cvs/{cvId}/degrees/{degreeId}`
- Delete a degree in a CV `DELETE /cvs/{cvId}/degrees/{degreeId}`

- Logging in `POST /session`
- Logging out `DELETE /session/{id}`
