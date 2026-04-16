## Music streaming

You're designing an API for a music streaming service. Use OpenAPI to describe the following sample API call:

```yaml
GET /artists/pink-floyd/albums?releaseYear=1969
200 OK
Content-Type: application/json
[
  {
    "id": "A890",
    "name": "More",
    "mainArtist": {
      "id": "pink-floyd",
      "name": "Pink Floyd"
    },
    "releaseYear": 1969,
    "comment": "Movie soundtrack"
  },
  {
    "id": "A789",
    "name": "Ummagumma",
    "mainArtist": {
      "id": "pink-floyd",
      "name": "Pink Floyd"
    },
    "releaseYear": 1969
  },
]
```

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>

### Solution

Operation description
```yaml
paths:
  /artists/{artistId}/albums:
    parameters:
      - name: artistId
        in: path
        required: true
        schema:
          type: string
    get:
      summary: Search an artist's albums
      parameters:
        - name: releaseYear
          in: query
          required: false
          schema:
            type: integer
      responses:
        "200":
          description: Albums found
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: "#/components/schemas/AlbumSummary"
```

Schemas
```yaml
components:
  schemas:
    AlbumSummary:
      type: object
      required:
        - id
        - name
        - mainArtist
        - releaseYear
      properties:
        id:
          type: string
        name:
          type: string
        mainArtist:
          $ref: "#/components/schemas/ArtistSummary"
        releaseYear:
          type: integer
        comment:
          type: string
    ArtistSummary:
      type: object
      required:
        - id
        - name
      properties:
        id:
          type: string
        name:
          type: string
```
