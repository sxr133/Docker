<template>
  <div>
    <!-- Conditional rendering of NHL table -->
    <div v-if="showMLBTable" class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <!-- Dropdown for selecting conference -->
      <div class="flex justify-center my-4">
        <select v-model="selectedConference" @change="fetchConferenceStandings" class="block w-1/2 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
          <option value="Select" disabled selected>Select a Conference</option>
          <option value="AL Conference">American League</option>
          <option value="NL Conference">National League</option>
        </select>
      </div>
      <div class="overflow-x-auto max-h-400">
        <table v-if="selectedConference != 'Select'" class="mt-4 border-collapse border border-gray-500">
          <!-- Table content -->
          <colgroup>
            <col style="width: 30%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
            <col style="width: 10%;">
          </colgroup>
          <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100 sticky top-0">

            <tr>
              <th scope="col" class="px-6 py-3">Team</th>
              <th scope="col" class="px-6 py-3">Wins</th>
              <th scope="col" class="px-6 py-3">Losses</th>
              <th scope="col" class="px-6 py-3">PCT</th>
              <th scope="col" class="px-6 py-3">Games Behind</th>
              <th scope="col" class="px-6 py-3">Runs Scored</th>
              <th scope="col" class="px-6 py-3">Runs Allowed</th>
              <th scope="col" class="px-6 py-3">Run Differential</th>
            </tr>
          </thead>

          <tbody>
            
            <!-- Render either Eastern or Western conference teams based on selection -->
            <tr v-for="(team, index) in selectedConference === 'NL Conference' ? nlConference : alConference" :key="index" class="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 px-6 py-4 text-gray-400">
              <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                <img class="block w-12 h-12 mb-2" :src="team.teamLogo" :alt="team.displayName + ' logo'">
                <span class="block text-center">{{ team.displayName }}</span>
              </td>
              <td class="px-6 py-4 text-gray-400 text-center">{{ team.wins }}</td>
              <td class="px-6 py-4 text-gray-400 text-center">{{ team.losses }}</td>
              <td class="px-6 py-4 text-gray-400 text-center">{{ ((team.wins / (team.wins + team.losses)) * 100).toFixed(1) }}</td>
              <td class="px-6 py-4 text-gray-400 text-center">{{ calculatedValue(team) }}</td> 
              <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsScored }}</td> 
              <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsAgainst }}</td> 
              <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsScored - team.runsAgainst }}</td> 
            </tr>
          </tbody>
        </table>
        <!-- Table content -->
        </div>
    </div>
  </div>
</template>

<script>
  
  import axios from 'axios';

  export default {
    props: {
      showMLBTable: Boolean,
    },
    data() {
      return {
        selectedTeamId: '',
        selectedConference: 'Select', // Default selection
        alConference: [],
        nlConference: [],
        isLoading: false, // Added to track when the API call is happening
        nlConferenceWinsDiff: 0,
        nlConferenceLossesDiff: 0,  
        alConferenceWinsDiff: 0,
        alConferenceLossesDiff: 0,
      };
    },
    computed: {
      calculatedValue() {
        return (team) => {
          
            if (this.selectedConference === 'NL Conference') {
              const nlConferenceWinsDiff = Math.abs((team.wins - this.nlConference[0].wins) / 2);
              const nlConferenceLossesDiff = Math.abs((team.losses - this.nlConference[0].losses) / 2);
              return nlConferenceWinsDiff + nlConferenceLossesDiff;
            } else {
              const alConferenceWinsDiff = Math.abs((team.wins - this.alConference[0].wins) / 2);
              const alConferenceLossesDiff = Math.abs((team.losses - this.alConference[0].losses) / 2);
              return alConferenceWinsDiff + alConferenceLossesDiff;
            }
        };
      }
    },
    methods: {
      async fetchConferenceStandings() {
        this.isLoading = true; // Set loading of API to true when the call starts
        const baseURL = process.env.VUE_APP_BACKEND_URL;
        const endpoint = `${baseURL}/api/MLBConference`;
        console.log("i get here inside the fetch");
        console.log('baseURL is ', baseURL);
        console.log('the endpoint is ', endpoint);
        try {
          console.log("i get here inside the try");
          const response = await axios.get(endpoint);
          
          this.alConference = []; // Clear previous data
          this.nlConference = []; // Clear previous data

          // Check if response.data exists
          if (response.data && response.data.body) {
            const result = response.data.body;
            console.log("result data:", result); // Log the response data
            // Function to extract team data
            const extractTeamData = (result, conference) => {
              return result.map(entry => (
                {
                  teamLogo: entry.espnLogo1,

                  displayName: entry.teamCity + " " + entry.teamName,
                  wins: parseInt(entry.wins),
                  losses: parseInt(entry.loss),
                  runsScored: parseInt(entry.RS),
                  runsAgainst: parseInt(entry.RA),
                  conference: conference,
                }
              ));
            };

            // Extract NL conference teams
            this.nlConference = extractTeamData(result.filter(team => team.conference === 'National League'), 'nlConference');

            // Extract AL conference teams
            this.alConference = extractTeamData(result.filter(team => team.conference === 'American League'), 'alConference');

            // Sort NL conference array by wins
            this.nlConference.sort((a, b) => b.wins - a.wins);
            console.table(this.nlConference);
            // Sort AL conference array by wins
            this.alConference.sort((a, b) => b.wins - a.wins);
          } else {
            console.error('Unexpected response format:', response.data);
          }
        } catch (error) {
          console.error('Error fetching MLB conference standings:', error);
        } finally {
          this.isLoading = false; // Reset loading state whether the call succeeds or fails
        }
      },
    },
  };
</script>