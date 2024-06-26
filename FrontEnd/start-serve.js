const { exec } = require('child_process');

exec('npx serve -s dist -l 8080', (err, stdout, stderr) => {
  if (err) {
    console.error(`Error: ${err}`);
    return;
  }
  console.log(stdout);
  console.error(stderr);
});
