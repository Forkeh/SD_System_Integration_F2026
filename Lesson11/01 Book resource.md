## Book resource

Fix the following OpenAPI book resource path definition:

```yaml
paths:
  /books/{bookReference}:
  summary: A book
  parameters:
    - name: bookId
      in: path
      schema: {}
```
<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

1. `summary` and `parameters` lack indentation
2. The name of a path parameter must match the path template
3. A path parameter definition must be required

```yaml
paths:
  /books/{bookReference}:
    summary: A book
    parameters:
      - name: bookReference
        in: path
        required: true
        schema: {}
```
