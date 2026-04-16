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

```yaml
paths:
  /artists/{artistRef}/albums:
    summary: All albums for specific artist
    parameters:
      - name: artistRef
        in: path
        required: true
        schema:
          description: The artist's name in URL encoded format
          type: string
    get:
      summary: It retrieves all albums by an artist
      parameters: 
        - name: releaseYear
          in: query
          required: false
          schema:
            description: The release year for an artist's albums
            type: integer
      responses:
        "200":
          description: Albums by specific artists successfully retrieved
          content:
            application/json:
              schema: 
                description: The list of albums found
                type: array
                items:
                  $ref: "#/components/schemas/AlbumSummary"

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
      required:
        - id
        - name
      type: object
      properties:
        id:
          type: string
        name:
          type: string


```

<sub>From Lauret, Arnaud (2025). *The Design of Web APIs*, 2nd ed.</sub>
