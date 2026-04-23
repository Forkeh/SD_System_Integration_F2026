# Voting Server

WebSocket server in TypeScript for the voting client.

Listens on `ws://localhost:8080`.

## Protocol

**Incoming** (from client):

```json
{ "type": "vote", "option": "Smørrebrød" | "Flæskesteg" | "Frikadeller" }
```

**Outgoing** (to clients):

```json
{ "type": "state", "votes": { "Smørrebrød": 0, "Flæskesteg": 0, "Frikadeller": 0 } }
```

On invalid input the server replies with `{ "type": "error", "message": "..." }`.

## Run

```bash
npm install
npm run dev
```
