/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./index.html","./src/**/*.{vue,js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        'custom-dark-purple': 'rgba(31, 17, 57, 1)', // Using full opacity (alpha = 1)
        'custom-dark-purple-hover': 'rgba(50, 30, 80, 1)', // Define a new hover color (example)
      }
    },
  },
  plugins: [
   
  ],
}

