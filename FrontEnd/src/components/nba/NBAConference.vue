<template>
  <div>
    <!-- Conditional rendering of NHL table -->
    <div v-if="showNBATable" class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <!-- Dropdown for selecting conference -->
      <div class="flex justify-center my-4">
        <select v-model="selectedConference" @change="fetchConferenceStandings" class="block w-1/2 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
          <option value="Select" disabled selected>Select a Conference</option>
          <option value="Western Conference">Western</option>
          <option value="Eastern Conference">Eastern</option>
        </select>
      </div>

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
        <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
          <tr>
            <th scope="col" class="px-6 py-3">Team</th>
            <th scope="col" class="px-6 py-3">Wins</th>
            <th scope="col" class="px-6 py-3">Losses</th>
            <th scope="col" class="px-6 py-3">PCT</th>
            <th scope="col" class="px-6 py-3">Games Behind</th>
            <th scope="col" class="px-6 py-3">Points For</th>
            <th scope="col" class="px-6 py-3">Points Against</th>
            <th scope="col" class="px-6 py-3">Points Differential</th>
          </tr>
        </thead>

        <tbody>
          
          <!-- Render either Eastern or Western conference teams based on selection -->
          <tr v-for="(team, index) in selectedConference === 'Eastern Conference' ? easternConference : westernConference" :key="index" class="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 px-6 py-4 text-gray-400">
            <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
              <img class="block w-12 h-12 mb-2" :src="team.teamLogo" :alt="team.displayName + ' logo'">
              <span class="block text-center">{{ team.displayName }}</span>
            </td>
            <td class="px-6 py-4 text-gray-400 text-center">{{ team.wins }}</td>
            <td class="px-6 py-4 text-gray-400 text-center">{{ team.losses }}</td>
            <td class="px-6 py-4 text-gray-400 text-center">{{ ((team.wins / (team.wins + team.losses)) * 100).toFixed(1) }}</td>
            <td class="px-6 py-4 text-gray-400 text-center">{{ calculatedValue(team) }}</td> 
            <td class="px-6 py-4 text-gray-400 text-center">{{ team.pointsFor }}</td> 
            <td class="px-6 py-4 text-gray-400 text-center">{{ team.pointsAgainst }}</td> 
            <td class="px-6 py-4 text-gray-400 text-center">{{ team.pointsFor - team.pointsAgainst }}</td> 
          </tr>
        </tbody>
      </table>        <!-- Table content -->
    </div>
  </div>
</template>

<script>
  
  import axios from 'axios';

  export default {
    props: {
      showNBATable: Boolean,
    },
    data() {
      return {
        selectedTeamId: '',
        selectedConference: 'Select', // Default selection
        westernConference: [],
        easternConference: [],
        isLoading: false, // Added to track when the API call is happening
        easternConferenceWinsDiff: 0,
        easternConferenceLossesDiff: 0,  
        westernConferenceWinsDiff: 0,
        westernConferenceLossesDiff: 0,
      };
    },
    computed: {
      calculatedValue() {
        return (team) => {
          
            if (this.selectedConference === 'Eastern Conference') {
              const easternConferenceWinsDiff = Math.abs((team.wins - this.easternConference[0].wins) / 2);
              const easternConferenceLossesDiff = Math.abs((team.losses - this.easternConference[0].losses) / 2);
              return easternConferenceWinsDiff + easternConferenceLossesDiff;
            } else {
              const westernConferenceWinsDiff = Math.abs((team.wins - this.westernConference[0].wins) / 2);
              const westernConferenceLossesDiff = Math.abs((team.losses - this.westernConference[0].losses) / 2);
              return westernConferenceWinsDiff + westernConferenceLossesDiff;
            }
        };
      }
    },
    methods: {
      async fetchConferenceStandings() {
        this.isLoading = true; // Set loading of API to true when the call starts
        const endpoint = `http://localhost:5000/api/NBAConference`;
        console.log("i get here inside the fetch");
        try {
          console.log("i get here inside the try");
          const response = await axios.get(endpoint);
          
          this.westernConference = []; // Clear previous data
          this.eaternConference = []; // Clear previous data

          // Check if response.data exists
          if (response.data && response.data.body) {
            const result = response.data.body;
            // Function to extract team data
            const extractTeamData = (result, conference) => {
              return result.map(entry => ({
                teamLogo: entry.espnLogo1,

                displayName: entry.teamCity + " " + entry.teamName,
                wins: parseInt(entry.wins),
                losses: parseInt(entry.loss),
                pointsFor: parseInt(entry.ppg),
                pointsAgainst: parseInt(entry.oppg),
                conference: conference,
            }));
            };

            // Extract NL conference teams
            this.easternConference = extractTeamData(result.filter(team => team.conference === 'Eastern Conference'), 'easternConference');

            // Extract AL conference teams
            this.westernConference = extractTeamData(result.filter(team => team.conference === 'Western Conference'), 'westernConference');

            // Sort NL conference array by wins
            this.easternConference.sort((a, b) => b.wins - a.wins);
            console.table("eastern",this.easternConference);
            // Sort AL conference array by wins
            this.westernConference.sort((a, b) => b.wins - a.wins);
            console.table("western",this.westernConference);
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