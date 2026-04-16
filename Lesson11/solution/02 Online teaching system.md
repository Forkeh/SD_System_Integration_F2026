## Online teaching system

You are designing a RESTful API for an online teaching system. Describe the resource path representing a specific course offered by a specific instructor in OpenAPI format.

```yaml
paths:
  /instructors/{instructorReference}/courses/{courseReference}:
    summary: A specific course by a specific instructor
    parameters:
      - name: instructorReference
        in: path
        required: true
        schema: {}
      - name: courseReference
        in: path
        required: true
        schema: {}

```

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>
