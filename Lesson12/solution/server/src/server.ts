import { WebSocketServer, WebSocket } from "ws";

type Option = "Smørrebrød" | "Flæskesteg" | "Frikadeller";

type LogEntry = {
    timestamp: string;
    votes: Record<Option, number>;
};

const votes: Record<Option, number> = {
    Smørrebrød: 0,
    Flæskesteg: 0,
    Frikadeller: 0,
};

const log: LogEntry[] = [];

const PORT = 8080;
const wss = new WebSocketServer({ port: PORT });

console.log(`Voting WebSocket server listening on ws://localhost:${PORT}`);

function broadcast(message: object): void {
    const json = JSON.stringify(message);
    for (const client of wss.clients) {
        if (client.readyState === WebSocket.OPEN) {
            client.send(json);
        }
    }
}

function recordAndBroadcastState(): void {
    const entry: LogEntry = {
        timestamp: new Date().toISOString(),
        votes: { ...votes },
    };
    log.push(entry);

    broadcast({ type: "state", votes });
    broadcast({ type: "log", entry });
}

function isValidOption(value: unknown): value is Option {
    return value === "Smørrebrød" || value === "Flæskesteg" || value === "Frikadeller";
}

wss.on("connection", (ws) => {
    console.log("Client connected");

    // Send current state and full log history to the newly connected client
    ws.send(JSON.stringify({ type: "state", votes }));
    ws.send(JSON.stringify({ type: "log", entries: log }));

    ws.on("message", (raw) => {
        let data: unknown;
        try {
            data = JSON.parse(raw.toString());
        } catch {
            ws.send(JSON.stringify({ type: "error", message: "Invalid JSON" }));
            return;
        }

        const msg = data as { type?: string; option?: unknown };

        if (msg.type === "vote" && isValidOption(msg.option)) {
            votes[msg.option]++;
            recordAndBroadcastState();
        } else {
            ws.send(JSON.stringify({ type: "error", message: "Invalid vote message" }));
        }
    });

    ws.on("close", () => {
        console.log("Client disconnected");
    });
});
