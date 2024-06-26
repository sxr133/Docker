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

# Install global dependencies
RUN npm install -g serve pm2

# Copy healthcheck script
COPY healthcheck.js .

# Copy PM2 configuration file
COPY pm2.json .

# Copy the start-serve script
COPY start-serve.js .

# Expose the ports on which the Vue.js app and health check server run
EXPOSE 8080 3001

# Command to run the Vue.js app and health check server using pm2
CMD ["pm2-runtime", "start", "pm2.json"]