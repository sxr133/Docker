<template>
  <section class="py-10 md:py-1">
    <div class="container max-w-screen-xl mx-auto px-4">
      <h1 class="font-medium text-gray-400 text-3xl md:text-10xl mb-5">Login to Sports Stats</h1>
      
        <div class="bg-gray-50 px-8 py-10 rounded-md">
          <form @submit.prevent="login">
            <div class="mb-4">
              <label for="email" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Email</label>
              <input v-model="email" type="text" id="username" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Username" autocomplete="username" />
            </div>
            <div class="flex flex-col relative mb-4">
                <label for="password" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Password</label>
                <input v-model="password" id="password" :type="showPassword ? 'text' : 'password'" placeholder="Password" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
                <button type="button" class="absolute inset-y-14 right-0 flex items-center px-3 py-1 text-slate-100" @click="togglePasswordVisibility">
                  <font-awesome-icon :icon="showPassword ? 'fa-eye-slash' : 'eye'" />
                </button>
            </div>
            <div  class="flex justify-between">
              <button type="submit" class="px-7 py-3 md:px-9 md:py-4 font-medium md:font-semibold bg-gray-700 text-gray-50 text-sm rounded-md hover:bg-gray-50 hover:text-gray-700 transition ease-linear duration-500">Login</button>
              <router-link to="/signup" class="px-7 py-3 md:px-9 md:py-4 font-medium md:font-semibold bg-gray-700 text-gray-50 text-sm rounded-md hover:bg-gray-50 hover:text-gray-700 transition ease-linear duration-500">Register</router-link>
            </div>
          </form>
        </div>

    </div>
  </section>
</template>

<script>
import { library } from '@fortawesome/fontawesome-svg-core';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import axios from 'axios'; // Import Axios
import { ref } from 'vue';
import { useRouter } from 'vue-router';

library.add(faEyeSlash);
library.add(faEye);

export default {
  components: {
    FontAwesomeIcon
  },
  methods: {
    togglePasswordVisibility() {
      console.log('togglePasswordVisibility called');
      console.log("showPassword set to: ", this.showPassword);
      this.showPassword = !this.showPassword;
      console.log("showPassword set to: ", this.showPassword);
    },
  },  
  // Define the loginSuccess event

  setup(_, { emit }) { // Destructure emit from context
    
    const email = ref('');
    const password = ref('');
    const showPassword = ref(false); // Define showPassword as reactive ref
    const router = useRouter(); // Access router instance

    const login = async () => { // Make login function async to use await
      try {
        if (email.value === 'guest' && password.value === 'password') {
          // If the entered email and password match the bypass values
          console.log('Bypassing validation...');
          emit('loginSuccess');
          router.push({ name: 'homePage' }); // Redirect to the 'entry' route upon successful login
          return;
        }

        const endpoint = `http://localhost:5000/Login`;
        const response = await axios.post(endpoint, { 
          email: email.value,
          password: password.value
        });

        console.log(response.data); // Handle response data as needed
        // Emit the loginSuccess event upon successful login
        emit('loginSuccess');
        router.push({ name: 'homePage' }); // Redirect to the 'entry' route upon successful login
      } catch (error) {

        if (error.response) {
          // The request was made and the server responded with a status code
          // that falls out of the range of 2xx
          console.error('Login failed with status:', error.response.status);
          console.error('Response data:', error.response.data);
          if (error.response.data.message == "Invalid email address"){
            console.log("invalid response asdfasdf");
          }
        } else if (error.request) {
          // The request was made but no response was received
          console.error('No response received:', error.request);
        } else {
          // Something happened in setting up the request that triggered an Error
          console.error('Error setting up request:', error.message);
        }
        // Handle login failure
      }
    };
    return { email, password, login, showPassword };
  },
};
</script>