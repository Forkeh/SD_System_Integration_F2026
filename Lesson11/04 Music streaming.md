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
