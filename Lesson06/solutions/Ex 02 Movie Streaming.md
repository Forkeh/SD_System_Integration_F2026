## Movie Streaming

**Part I**

Write a GraphQL SDL for a movie streaming platform with the following requirements:

```graphql
type Schema {
	query: Query
	mutation: Mutation
}
```

Models:

- Movie (id, title, genre, duration in minutes, rating, is available, details, cast)

```graphql
type Movie {
    id: ID!
    title: String!
    genre: Genre!
    duration: Int!
    rating: Float!
    isAvailable: Boolean!
    details: MovieDetails!
    cast: [Person!]!
}
```

- Movie details (release year, language, age rating -e.g., "PG-13")

```graphql
type MovieDetails {
    releaseYear: Int!
    language: String!
    ageRating: String!
}
```

- Person (id, name, role)

```graphql
type Person {
    id: ID!
    name: String!
    role: String!
}
```

- User (id, username, email, has subscription, favourite movies)

```graphql
type User {
    id: ID!
    username: String!
    email: String!
    hasSubscription: Boolean!
    favouriteMovies: [Movies]!
}
```

Enums

- Genre (ACTION, COMEDY, DRAMA, HORROR, SCIFI, OTHER)

```graphql
enum Genre {
    ACTION
    COMEDY
    DRAMA
    HORROR
    SCIFI
    OTHER
}
```

Queries

- Movie by ID
- Movies by genre and average rating
- Users

```graphql
type Query {
    MovieById(id: ID!): Movie!
    Movies(genre: Genre, rating: Int): [Movie]!
    Users: [User]!
}
```

Mutations

- Add movie (parameters: title, genre, duration in minutes)

```graphql
type Mutation {
    AddMovie(title: String!, genre: Genre!, duration: Int!): Movie!
}
```

**Part II**

Write the following queries:

1. Get the titles of all movies

```graphql
query GetTitlesofAllMovies {
    Movies {
        title
    }
}
```

2. Get a movie by ID

```graphql
query GetMovieById($id: ID!) {
    MovieById(id: $id) {
        id
        title
        genre
        duration
        rating
        isAvailable
        details
        cast
    }
}
```

3. Get all action movies with an average rating of 4.0

```graphql
query GetMoviesByGenreAndRating($genre: Genre!, $rating: Float!) {
    Movies(genre: $genre, rating: $rating) {
        id
        title
        genre
        duration
        rating
        isAvailable
        details
        cast
    }
}
```

4. Get all users with their favourite movies

```graphql
query GetUsersWithFavoriteMovies {
    Users {
        id
        username
        email
        hasSubscription
        favouriteMovies
    }
}
```

5. Write a mutation to add the film _Inception_ (science-fiction, 148 minutes).

```graphql
mutation AddMovie($title: String!, $genre: Genre!, $duration: Int!) {
    AddMovie(title: $title, genre: $genre, duration: $duration) {
        id
        title
    }
}
```
