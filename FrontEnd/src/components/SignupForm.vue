<template>
  <section class="py-10 md:py-1 ">
    <div class="container max-w-screen-xl mx-auto px-4">
      <h1 class="font-medium text-gray-400 text-3xl md:text-10xl mb-5">Register to Sports Stats</h1>
      <div  class="bg-gray-50 px-8 py-10 rounded-md">
        <form @submit.prevent="signup">
          <h6 class="font-small text-gray-400 text-3xl md:text-10xl mb-5">Full Name</h6>
          <div class="flex mb-4 border border-slate-500 p-6 rounded-lg mx-auto">
            <div class="flex flex-col w-1/2 mr-4">
              <input v-model="userSignup.FirstName" id="firstname" type="text" placeholder="Firstname" required class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
              <label for="firstname" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">First Name</label>
            </div>
            <div class="flex flex-col w-1/2 mr-4">
              <input v-model="userSignup.LastName" id="lastname" type="text" placeholder="Lastname" required class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
              <label for="lastname" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Last Name</label>
            </div>
          </div>

          <h6 class="font-small text-gray-400 text-3xl md:text-10xl mb-5">Address</h6>
          <div class="border border-slate-500 p-6 rounded-lg mx-auto">
            <div class="flex flex-col mb-4">
              <input v-model="userSignup.StreetAddress1" id="address1" type="text" placeholder="Address 1" required class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
              <label for="address1" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Street Address<span class="text-red-500">*</span></label>
            </div>
            <div class="flex flex-col mb-4">
              <input v-model="userSignup.StreetAddress2" id="address2" type="text" placeholder="Address 2" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
              <label for="address2" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Street Address Line 2</label>
            </div>
            
            <div class="flex mb-4">
              <div class="flex flex-col w-1/2 mr-4">
                  <input v-model="userSignup.City" id="city" type="text" placeholder="City" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required >
                  <label for="city" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">City <span class="text-red-500">*</span></label>
              </div>
              <div class="flex flex-col w-1/2">
                  <input v-model="userSignup.State_Prov" id="state_prov" type="text" placeholder="State / Province" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
                  <label for="state_prov" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">StateProvince</label>
              </div>
            </div>

            <div class="flex mb-4">
              <div class="flex flex-col w-1/2 mr-4">
                <input v-model="userSignup.Zip_Post_Cd" id="postal_zip" type="text" placeholder="PostalCd_Zip"  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
                <label for="postal_zip" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Zip / Postal Code<span class="text-red-500">*</span></label>
              </div>
              <div class="flex flex-col w-1/2 mr-4">
                <input v-model="userSignup.Country" id="country" type="text" placeholder="Country"  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
                <label for="country" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Country<span class="text-red-500">*</span></label>
              </div>
            </div>
          </div>

          <div class="border border-slate-500 p-6 rounded-lg mx-auto my-5">
            <div class="flex flex-col mb-4 my-5">
              <input v-model="userSignup.Email" id="email" type="text" placeholder="example@provider.com" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
              <label for="email" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Email<span class="text-red-500">*</span></label>
            </div>

            <div class="flex flex-col relative mb-2">
              <input v-model="userSignup.Password" id="password" :type="showPassword ? 'text' : 'password'" placeholder="Password" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" required>
              <button type="button" class="absolute inset-y-10 right-0 flex items-center px-3 py-1 text-slate-100" @click="togglePasswordVisibility">
                <font-awesome-icon :icon="showPassword ? 'fa-eye-slash' : 'eye'" />
              </button>
              <label for="password" class="block mb-2 text-2xl font-medium text-black-900 dark:text-black">Password<span class="text-red-500">*</span></label>
            </div>
          </div>
          <div>
            <!-- Hidden input fields for verification token and email verification status -->
            <input type="hidden" v-model="userSignup.VerificationToken">
            <input type="hidden" v-model="userSignup.IsEmailVerified">
          </div>
          <div class="flex justify-between">
            <button  type="submit" class="px-7 py-3 md:px-9 md:py-4 font-medium md:font-semibold bg-gray-700 text-gray-50 text-sm rounded-md hover:bg-gray-50 hover:text-gray-700 transition ease-linear duration-500">Register</button>
            <router-link :to="'/'" class="px-7 py-3 md:px-9 md:py-4 font-medium md:font-semibold bg-gray-700 text-gray-50 text-sm rounded-md hover:bg-gray-50 hover:text-gray-700 transition ease-linear duration-500">Cancel</router-link>
          </div>
        </form>
      </div>
    </div>
  </section>
</template>

<script>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { library } from '@fortawesome/fontawesome-svg-core';
import { faEyeSlash, faEye } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios'; // Import Axios

library.add(faEyeSlash);
library.add(faEye);

export default {
  components: {
    FontAwesomeIcon
  },
  data() {
    return {
      showPassword: false,
    
      userSignup: {
        FirstName: '',
        LastName: '',
        StreetAddress1: '',
        StreetAddress2: '',
        City: '',
        State_Prov: '',
        Zip_Post_Cd: '',
        Country: '',
        Email: '',
        Password: '',
        VerificationToken: ' ',
        IsEmailVerified: false
      }
    }
  },
  methods: {
    togglePasswordVisibility() {
      console.log('togglePasswordVisibility called');
      this.showPassword = !this.showPassword;
    },
    async signup() {
      try {
        const userData = {
          FirstName: this.userSignup.FirstName,
          LastName: this.userSignup.LastName,
          StreetAddress1: this.userSignup.StreetAddress1,
          StreetAddress2: this.userSignup.StreetAddress2,
          City: this.userSignup.City,
          State_Prov: this.userSignup.State_Prov,
          Zip_Post_Cd: this.userSignup.Zip_Post_Cd,
          Country: this.userSignup.Country,
          Email: this.userSignup.Email,
          Password: this.userSignup.Password,
          VerificationToken: this.userSignup.VerificationToken,
          IsEmailVerified: this.userSignup.IsEmailVerified
        };

        console.log("userData data:", userData); // Log userSignup data
        console.log("Sending signup request...");
        const endpoint = 'https://localhost:7049/Signup'; // Adjust the URL as per your backend setup
        const response = await axios.post(endpoint, userData);

        console.log("Signup response:", response.data); // Log signup response
        this.$router.push({ name: 'LoginForm' }); // Redirect to the login page upon successful signup
      } catch (error) {
        console.error('Error:', error);
        // Handle signup failure
      }
    }
  }
};
</script>