## Hiking application

You are designing an API for a hiking application. Describe in OpenAPI format the inputs of the operation that retrieves the list of segments in a specific trail and allows users to filter segments
by difficulty.

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

```yaml
paths:
  /trails/{trailId}/segments:
    summary: A trail's segments
    parameters:
      - name: trailId
        in: path
        required: true
        schema: {}
    get:
      summary: List segments of a trail
      parameters:
        - name: difficulty
          in: query
          schema: {}
```
