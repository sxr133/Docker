const express = require('express');
const app = express();

app.get('/', (req, res) => {
    res.status(200).send('OK');
});

const port = 3001;
app.listen(port, () => {
  console.log(`Health check server running on port ${port}`);
});