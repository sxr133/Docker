<template>
  <div>
    <div v-if="showDivisionOptionTable" class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <!-- Dropdown for selecting conference -->
      <div class="flex justify-center my-4">
        <select v-model="selectedDivision" @change="fetchDivisionStandings" class="block w-1/2 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
          <option value="Select" disabled selected>Select a Division</option>
          <option value="American League East">American League East</option>
          <option value="American League Central">American League Central</option>
          <option value="American League West">American League West</option>
          <option value="National League East">National League East</option>
          <option value="National League Central">National League Central</option>
          <option value="National League West">National League West</option>
        </select>
      </div>
    </div>

    <!-- Render division tables dynamically -->
    <div v-if="selectedDivision" class="justify-center">
        <table v-if="divisions[selectedDivision]" class="mt-4 border-collapse border border-gray-500">
            <colgroup>
              <col style="width: 20%;">
              <col style="width: 10%;">
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
                <th colspan="9" class="text-center py-4 text-xl font-semibold">{{ selectedDivision }}</th>
              </tr>
              <tr>
                <th scope="col" class="px-6 py-3">Team</th>
                <th scope="col" class="px-6 py-3">Wins</th>
                <th scope="col" class="px-6 py-3">Losses</th>
                <th scope="col" class="px-6 py-3">PCT</th>
                <th scope="col" class="px-6 py-3">Games Behind</th>
                <th scope="col" class="px-6 py-3">Runs Scored</th>
                <th scope="col" class="px-6 py-3">Runs Allowed</th>
                <th scope="col" class="px-6 py-3">Run Differential</th>
                <th scope="col" class="px-6 py-3">Team Roster</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(team, index) in divisions[selectedDivision]" :key="index" class="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 px-6 py-4 text-gray-400">
                <!-- Team information -->
                <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                  <img class="block w-12 h-12 mb-2" :src="team.teamLogo" :alt="team.displayName + ' logo'">
                  <span class="block text-center">{{ team.displayName }}</span>
                </td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ team.wins }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ team.losses }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ ((team.wins / (team.wins + team.losses)) * 100).toFixed(1) }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ ((team.wins - divisions[selectedDivision][0].wins) / 2) + ((divisions[selectedDivision][0].losses - team.losses) / 2) }}</td> 
                <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsScored }}</td> 
                <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsAgainst }}</td> 
                <td class="px-6 py-4 text-gray-400 text-center">{{ team.runsScored - team.runsAgainst }}</td> 
                <td class="px-6 py-4 text-gray-400 text-center">
                  <router-link :to="'/MLB-team-roster/' + selectedDivision + '/' + team.teamAbv + '/' + team.displayName" @click="$emit('teamSelected', { teamAbv: team.teamAbv, teamName: team.displayName, selectedDivision : selectedDivision })">View Team Roster</router-link>
                </td>
              </tr>            
            </tbody>
          </table>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  export default {
    props: {
      showDivisionOptionTable: Boolean,
    },
    data(){
      return{
        selectedDivision: 'Select', // Default selection
        divisions: {
          'American League East': [],
          'American League Central': [],
          'American League West': [],
          'National League East': [],
          'National League Central': [],
          'National League West': []
        },
        isLoading: false, // Add this line to initialize isLoading
      };
    },
    methods: {
      async fetchDivisionStandings() {
        // Set loading of API to true when the call starts
        this.isLoading = true;

        const endpoint = `https://localhost:7049/api/MLBDivision`;

        try {
          const response = await axios.get(endpoint);
          // Check if response data exists
          if (response.data && response.data.body) {
            const teams = response.data.body;
            // Initialize divisions object
            this.divisions = {
              'American League East': [],
              'American League Central': [],
              'American League West': [],
              'National League East': [],
              'National League Central': [],
              'National League West': []
            };
            // Function to populate divisions for each conference
            const populateDivisions = (team) => {
            const divisionName = team.conference + " " + team.division;
            console.log("divisionName is: " + divisionName);
              this.divisions[divisionName].push({
                teamLogo: team.mlbLogo1,
                teamAbv : team.teamAbv,
                displayName: `${team.teamCity} ${team.teamName}`,
                wins: parseInt(team.wins),
                losses: parseInt(team.loss),
                runsScored: parseInt(team.RS),
                runsAgainst: parseInt(team.RA),
              });
            };

            // Populate divisions for each team
            teams.forEach(team => {
              populateDivisions(team);
            });

             // Sort the teams within each division by wins
            
                for (const division in this.divisions) {
                    this.divisions[division].sort((a, b) => b.wins - a.wins);
                }


            // Now divisions object contains the structured data
            console.log(this.divisions);
          }
        } catch (error) {
          console.error('Error fetching division standings:', error);
        } finally {
          // Set loading state to false after API call completes
          this.isLoading = false;
        }
      },
    }
  };
</script>