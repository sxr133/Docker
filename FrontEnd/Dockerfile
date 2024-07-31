# Use Node.js LTS version as base image
FROM node:20 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (if available) to /app
COPY FrontEnd/package*.json ./

# Install dependencies
RUN npm install
RUN npm install express

# Copy the rest of the application code to /app
COPY FrontEnd/. .

# Copy .env.production to /app
COPY FrontEnd/.env.production .env

# Set environment variable for backend URL during the build process
ARG VUE_APP_BACKEND_URL
ENV VUE_APP_BACKEND_URL=${VUE_APP_BACKEND_URL}

# Debug: Print environment variables before build
RUN echo "Environment variables before build:"
RUN printenv

# Build the Vue.js project
RUN npm run build

# Debug: Print environment variables after build
RUN echo "Environment variables after build:"
RUN printenv

# Use node base image for runtime
FROM node:20

# Install serve and PM2
RUN npm install -g serve pm2

# Set the working directory
WORKDIR /app

# Copy built files from the build stage
COPY --from=build /app/dist /app/dist

# Copy dependencies from build stage
COPY --from=build /app/node_modules /app/node_modules
#COPY --from=build /app/package.json /app/package.json

# Copy pm2
COPY FrontEnd/pm2.json /app/pm2.json

# Copy healthcheck script
COPY FrontEnd/healthcheck.js /app/healthcheck.js

# Copy the start-serve script
COPY FrontEnd/start-serve.js /app/start-serve.js

# Copy environment variables file
COPY FrontEnd/.env.production .env

# Expose ports
EXPOSE 8080 3001

# Set environment variables for HTTPS
ENV HTTPS=true
ENV SSL_CRT_FILE=/https/ssl/certs/selfsigned.crt
ENV SSL_KEY_FILE=/https/ssl/private/selfsigned.key

# Start the Vue.js app and health check server using PM2
CMD ["pm2-runtime", "start", "/app/pm2.json"]