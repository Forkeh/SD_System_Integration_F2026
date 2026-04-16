## Book resource

Fix the following OpenAPI book resource path definition:

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
<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>
