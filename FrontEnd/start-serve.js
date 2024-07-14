const express = require('express');
const path = require('path');
const serveStatic = require('serve-static');
const app = express();
const dotenv = require('dotenv');

// Load environment variables from .env file based on NODE_ENV
const envFilePath = process.env.NODE_ENV === 'production' ? '.env.production' : '.env.development';
dotenv.config({ path: path.resolve(__dirname, envFilePath) });

// Use environment variable for backend URL
const backendURL = process.env.VUE_APP_BACKEND_URL;

console.log("Backend URL is set to this value:", process.env.VUE_APP_BACKEND_URL);

// Log the backend URL for debugging
console.log(`Backend URL is set to: ${backendURL}`);

// Serve static files from the dist directory
app.use('/', serveStatic(path.join(__dirname, '/dist')));

// Healthcheck endpoint
app.get('/healthcheck', (req, res) => {
  res.status(200).send('Healthcheck OK');
});

// Fallback to index.html for any other routes
app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname, '/dist/index.html'));
});

// Define the port to run the server
const port = process.env.PORT || 8080;
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});