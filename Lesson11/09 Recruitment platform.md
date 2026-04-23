## Recruitment platform

You're working on an API for a recruitment platform. Given the following sample of job offer JSON data, describe the "Create a job offer" and "Update (replace) a job offer" operations with OpenAPI.

```yaml
{
  "reference": "xz789",
  "created": "2024-12-08",
  "title": "Technical book author",
  "description": "Writing technical books"
}
```

### Solution

Schemas
```yaml
components:
  schemas:
    JobOffer:                  # JobOffer complete schema
      required:
        - reference
        - created
        - title
        - description
      properties:
        reference:
          type: string
        created:
          type: string
          format: date
        title:
          type: string
        description:
          type: string
    JobOfferCreateOrReplace:  # JobOffer schema without the read-only properties
      required:
        - title
        - description
      properties:
        title:
          type: string
        description:
          type: string
```

Creating a job offer
```yaml
paths:
  /job-offers:
    post:
      requestBody:
        description: Job offer info.
        content:
          application/json:
            schema:
              $ref: "#/components/schemas/JobOfferCreateOrReplace"
    responses:
      "201":
        description: Job offer created
        content:
          application/json:
            schema:
              $ref: "#/components/schemas/JobOffer"
```

Replacing a job offer
```yaml
paths:
  ...
  /job-offers/{jobOfferReference}:
    parameters:
      - name: jobOfferReference  # ID of the job offer to replace
        in: path
        required: true
        schema:
          type: string
  put:
    requestBody:
      description: Job offer info.
      content:
        application/json:
          schema:
            $ref: "#/components/schemas/JobOfferCreateOrReplace"
      responses:
        "201":
          description: Job offer created
          content:
            application/json:
              schema:
                $ref: "#/components/schemas/JobOffer"
```
