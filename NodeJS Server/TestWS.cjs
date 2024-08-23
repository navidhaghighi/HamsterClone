// server.js
import http from 'http';
import WebSocket from 'ws'; // Import the entire 'ws' module

const server = http.createServer((req, res) => {
  res.writeHead(200, { 'Content-Type': 'text/plain' });
  res.end('WebSocket server running');
});

const wss = new WebSocket.Server({ server }); // Use WebSocket.Server directly

wss.on('connection', (ws) => {
  console.log('New WebSocket connection established');
  ws.on('message', (message) => {
    console.log(`Received: ${message}`);
    // Handle the message here and send a response back to the client if needed
  });
});

server.listen(8080, () => {
  console.log('WebSocket server listening on port 8080');
});
