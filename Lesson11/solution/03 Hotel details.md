## Hotel details

Fix the OpenAPI description of the "Get hotel details" operation shown below, which returns JSON.

```yaml
paths:
  /hotels/{hotelId}:
    summary: An hotel
    parameters:
      - name: hotelId
        in: path
        required: true
        schema: {}
    get:
      summary: Get hotel details
      responses:
        "200":
          description: Hotel details successfully retrieved
          content:
            application/json:
              schema: {}
```

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>
