# Use Node.js LTS version as base image
FROM node:lts-alpine

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (if available) to /app
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code to /app
COPY . .

# Set environment variable for backend URL
# This will be used during the build process
ENV VUE_APP_BACKEND_URL=http://localhost:5000

# Build the Vue.js project
RUN npm run build

# Install a simple HTTP server to serve the built application
RUN npm install -g serve

# Expose the port on which the Vue.js app runs
EXPOSE 8080

# Command to run the Vue.js app
CMD ["serve", "-s", "dist"]