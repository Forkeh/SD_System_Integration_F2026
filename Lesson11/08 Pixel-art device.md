## Pixel-art device

You're designing an API for a pixel-art device comprising a "screen" composed of 36-by-36 luminous squares. A screen has an id (SC001, for example) and a matrix of pixels (array of array). Each pixel has an rgb array (`[10, 23, 45]`, for example), a
brightness percentage between 0 and 1 (0.3, for example), and an `on` flag. All properties are required except the brightness. Describe the corresponding Screen, Pixels,
and Pixel reusable schemas with OpenAPI. 

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

```yaml
components:
  schemas:
    Screen:
      type: object
      required:
        - id
        - pixels
      properties:
        id:
          type: string
        pixels:
          $ref: "#/components/schemas/Pixels"
    Pixels:
      description: A matrix of pixels (array of array)
      type: array
      items:
        type: array
        items:
          $ref: "#/components/schemas/Pixel"
    Pixel:
      type: object
      required:
        - rgb
        - on
      properties:
        rgb:
          description: "[r, g, b]"
          type: array
          items:
            type: integer
        brightness:
          type: number
        on:
          type: boolean
```
