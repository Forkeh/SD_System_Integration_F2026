# CORS

## **Scenario 1: Simple `GET`**

Request

```
GET http://127.0.0.1:3000/projects
Origin: http://127.0.0.1:5500
```

Response

```
HTTP/1.1 200 OK
Content-Type: application/json
```

#### 1. Is this a cross-origin request?

Yes, different origin (specifically port)

#### 2. Is it a simple request?

Yes

#### 3. Will the browser be allowed to read the response?

Only if CORS policy is setup in web server

#### 4. If negative, what should be done and where to allow the browser to read the response?

CORS - Allow the origin 127.0.0.1:5500 with the HTTP method GET in the web server.

---

## **Scenario 2: `POST` with JSON**

Cross-Origin Non-Simple Request (preflight needed)

```
POST http://127.0.0.1:3000/projects
Origin: http://127.0.0.1:5500
Content-Type: application/json
```

Body

```json
{
	"title": "New project",
	"description": "Created from frontend"
}
```

#### 1. What `OPTIONS` request will the browser send?

METHOD: POST & Content-type: appplication/json

#### 2. Let us assume that the server returns the following response:

    ```
    HTTP/1.1 204 No Content
    Access-Control-Allow-Origin: http://127.0.0.1:5500
    ```

Will the browser send the actual `POST` request?

No, needs method and content-type options.

#### 3. If negative, what should be done and where to allow the browser to send the `POST` request?

Needs to also set method and content-type options.

---

## **Scenario 3: Custom header**

Cross-origin request

```
POST http://127.0.0.1:3000/projects
Origin: http://127.0.0.1:5500
Content-Type: text/plain
X-Requested-With: fetch
```

#### 1. Would the browser send a preflight request? Why?

Yes, because of the custom header

#### 2. If affirmative, what should the preflight response look like?

```
    HTTP/1.1 204 No Content
    Access-Control-Allow-Origin: http://127.0.0.1:5500
```

---

## **Scenario 4: Request with credentials**

A request with origin `http://127.0.0.1:5500` calls an API with credentials included. The API server responds:

```
HTTP/1.1 200 OK
Access-Control-Allow-Origin: *
Access-Control-Allow-Credentials: true
Content-Type: application/json
Set-Cookie: sessionId=abc123; HttpOnly
```

#### 1. The browser will block the response. Why?

You cannot combine allow-origin: \* with credentials: true. They are mutually exclusive.

#### 2. What should be done and where for the browser to process the response?

Specify origin(s) instead of wildcat.

---

## **Scenario 5: Simple request**

Request

```
GET http://127.0.0.1:3000/projects
Origin: http://127.0.0.1:3000
```

#### 1. Is preflight necessary?

No

#### 2. Can the browser read the response?

Yes
