## Hotel details

Fix the OpenAPI description of the "Get hotel details" operation shown below, which returns JSON.

```yaml
paths:
  /hotels/{hotelId}:
    summary: An hotel
    get:
      summary: Get hotel details
      responses:
        "200":
          description: Hotel details successfully retrieved
          content:
            schema: {}
```

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

1. The `hotelId` path parameter is not defined
2. Response content type is missing

```yaml
paths:
  /hotels/{hotelId}:
    summary: A hotel
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
