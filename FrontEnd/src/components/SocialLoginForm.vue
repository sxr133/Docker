<template>
  <div class="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4">
    <!-- Logo Section -->
    <img
  src="/images/sporting-stats-high-resolution-logo.png"
  alt="Sporting Stats Logo"
  class="mb-6"
  style="width: 600px; height: 300px;"
/>
    
    <!-- Form Section -->
    <div class="login-container bg-white p-6 rounded shadow-md w-full max-w-md">
      <h2 class="text-center text-2xl font-semibold mb-4">Login</h2>
      <form @submit.prevent="handleLogin" class="flex flex-col space-y-4">
        <div class="form-group">
          <input
            type="text"
            v-model="emailOrUsername"
            placeholder="Email or Username"
            class="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <div class="form-group">
          <input
            type="password"
            v-model="password"
            placeholder="Password"
            class="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
            required
          />
        </div>
        <button type="submit" class="bg-custom-dark-purple text-white px-4 py-2 rounded shadow hover:custom-dark-purple-hover">Login</button>
      </form>
      
      <!-- Social Login Buttons -->
      <div class="social-login mt-4">
        <h2 class="text-center text-2xl font-semibold mb-4">Login with Social Media</h2>
        
        <div class="flex space-x-4">
          <button @click="loginWithGoogle" class="flex items-center justify-center bg-custom-dark-purple text-white px-4 py-2 rounded shadow hover:custom-dark-purple-hover w-full mb-2 mr-4">
            <div class="flex items-center">
              <img src="/images/google.svg" alt="Google Icon" class="w-5 h-5 mr-2" />
              - Google
            </div>
          </button>

          <button @click="loginWithFacebook" class="flex items-center justify-center bg-custom-dark-purple text-white px-4 py-2 rounded shadow hover:custom-dark-purple-hover w-full mb-2">
            <div class="flex items-center">
              <img src="/images/facebook.svg" alt="Google Icon" class="w-5 h-5 mr-2" />
              - Facebook
            </div>
          </button>
        </div>
      </div>
    </div>
    
    <!-- Disclaimer Link -->
    <a href="/privacy-policy.html" target="_blank" class="mt-6 text-gray-600 underline">
      Privacy Policy
    </a>
  </div>
</template>


<script>
import { useRouter } from 'vue-router';

export default {
  mounted(){
    this.setFavicon();
  },
  data() {
    return {
      emailOrUsername: '',
      password: '',
      username: '',
      baseURL: process.env.VUE_APP_BACKEND_URL || 'https://www.sportingstats.ca'
    };
  },
  setup() {
    const router = useRouter(); // Access the router instance
    return { router };
  },
  methods: {
    async handleLogin() {
      // Send login request to the backend
      try {

        // Determine if the input is an email or username
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Simple email regex
        if (emailRegex.test(this.emailOrUsername)) {
          // It's an email
          this.email = this.emailOrUsername;
          this.username = '';
        } else {
          // It's a username
          this.username = this.emailOrUsername;
          this.email = '';
        }

        const baseURL = process.env.VUE_APP_BACKEND_URL;
        const endpoint = `${baseURL}/api/auth/login`;

        console.log("endpoint is: ", endpoint);
        console.log("email is", this.email);
        console.log("username is", this.username);
        console.log("password is", this.password);

        const response = await fetch(endpoint, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            email: this.emailOrUsername,
            username: this.username,
            password: this.password,
          }),
        });
        
        console.log('json data is', response);
        // Check if the response is JSON
        const contentType = response.headers.get('Content-Type');
        let data = {};
        if (contentType && contentType.includes('application/json')) {
          data = await response.json(); // Parse JSON only if content is JSON
        } else {
          throw new Error('Response is not in JSON format');
        }

        if (response.ok) {
          // Handle successful login, e.g., store tokens or redirect
          alert('Login successful!');

          // Redirect to the home page after successful login
          this.router.push({ name: 'homePage' });

        } else {
          if (response.status === 401) {
            alert('Invalid email/username or password. Please check your credentials or sign up.');
          } else {
            alert(data.message || 'Login failed.');
          }
        }
      } catch (error) {
        console.error('Login error:', error);
        alert('An error occurred.');
      }
    },
    loginWithGoogle() {
      console.error('login with google')
      const baseURL = process.env.VUE_APP_BACKEND_URL;
      window.location.href = `${baseURL}/api/auth/google`;
    },
    loginWithFacebook() {
      console.error('login with facebook')
      const baseURL = process.env.VUE_APP_BACKEND_URL;
      window.location.href = `${baseURL}/api/auth/facebook`;
    },
    setFavicon() {
      // Find the existing favicon link element
      const favicon = document.querySelector('link[rel="icon"]');

      // Create a new link element for the favicon with your logo
      const newFavicon = document.createElement('link');
      newFavicon.rel = 'icon';
      newFavicon.type = 'image/png'; // Change the type if your logo is not in PNG format
      newFavicon.href = './images/sporting-stats-high-resolution-logo.png'; // Adjust the path to your logo file

      // Replace the existing favicon with the new one
      if (favicon) {
        document.head.removeChild(favicon);
      }
      document.head.appendChild(newFavicon);
    }
  },
};
</script>

<style scoped>
/* Add some basic styling */
.login-container {
  width: 100%;
  margin: auto;
}
.form-group {
  margin-bottom: 15px;
}
button {
  width: 100%;
  margin-top: 10px;
}
.social-login button {
  width: 48%;
  display: inline-block;
  margin-top: 10px;
}
</style>
