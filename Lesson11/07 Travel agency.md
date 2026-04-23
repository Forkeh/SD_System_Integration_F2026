## Travel agency

You are designing an API for a travel agency system. Describe in OpenAPI format the inputs and outputs of an operation that adds a new destination to a specific travel package and returns the created destination.

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

```yaml
paths:
  /packages/{packageId}/destinations:
    summary: A travel package's destinations
    parameters:
      - name: packageId
        in: path
        required: true
        schema: {}
    post:
      summary: Add a new destination to a travel package
      requestBody:
        description: Destination info.
        content:
          application/json:
          schema: {}
      responses:
        "201":
          description: Destination added to the travel package
          headers:
            Location:
              description: Destination URL
              schema: {}
        content:
          application/json:
            schema:
              description: Destination info.
```
