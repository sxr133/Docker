const express = require('express');
const app = express();
const port = process.env.HEALTH_PORT || 3001;

app.get('/', (req, res) => {
    res.status(200).send('OK');
});

app.listen(port, () => {
    console.log(`Health check server running on port ${port}`);
});

