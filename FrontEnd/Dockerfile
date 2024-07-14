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

# Copy .env.production to /app
COPY .env.production .env

# Set environment variable for backend URL during the build process
ARG VUE_APP_BACKEND_URL
ENV VUE_APP_BACKEND_URL=$VUE_APP_BACKEND_URL

# Debug: Print environment variables before build
RUN echo "Environment variables before build:"
RUN printenv

# Build the Vue.js project
RUN npm run build

# Debug: Print environment variables after build
RUN echo "Environment variables after build:"
RUN printenv

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