<template>
  <div class="bg-zinc-300 h-screen">
    <router-view />
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router'; // Import useRouter from vue-router

export default {
  setup() {
    // Reactive variable to track login status
    const loggedIn = ref(false);
    const router = useRouter(); // Access router instance

    // Function to handle login success event
    const handleLoginSuccess = () => {
      
      loggedIn.value = true; // Update loggedIn status upon successful login
      // Redirect to home page upon successful login
      router.push({ name: 'homePage' });
    };

    return { loggedIn, handleLoginSuccess };
  }
};
</script>