<template>
  <div>
    <div v-if="showNHLDivisionOptionTable" class="relative overflow-x-auto shadow-md sm:rounded-lg">
      <!-- Dropdown for selecting conference -->
      <div class="flex justify-center my-4">
        <select 
          id="nhlDivisionSelect" 
          name="nhlDivision" 
          v-model="selectedDivision" 
         @change="fetchDivisionStandings" 
          class="block w-1/2 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
          <option value="Select" disabled selected>Select a Division</option>
          <option value="Central Division">Central Division</option>
          <option value="Pacific Division">Pacific Division</option>
          <option value="Atlantic Division">Atlantic Division</option>
          <option value="Metropolitan Division">Metropolitan Division</option>
        </select>
      </div>
    </div>

    <!-- Render division tables dynamically -->
    <div v-if="selectedDivision" class="justify-center">
      <table v-if="divisions[selectedDivision]" class="mt-4 border-collapse border border-gray-500">
            <colgroup>
              <col style="width: 20%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
              <col style="width: 5%;">
            </colgroup>
            <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
              <tr>
                <th colspan="18" class="text-center py-4 text-xl font-semibold">{{ selectedDivision }}</th>
              </tr>
              <tr>
                <th scope="col" class="px-6 py-3" title="Team">Team</th>
                <th scope="col" class="px-6 py-3" title="Games Played">Games Played</th>
                <th scope="col" class="px-6 py-3" title="Wins">Wins</th>
                <th scope="col" class="px-6 py-3" title="Losses">Losses</th>
                <th scope="col" class="px-6 py-3" title="OverTime Losses">OT Losses</th>
                <th scope="col" class="px-6 py-3" title="Points">Points</th>
                <th scope="col" class="px-6 py-3" title="Points Percentage">Points %</th>
                <th scope="col" class="px-6 py-3" title="Road Wins">RW</th>
                <th scope="col" class="px-6 py-3" title="Road Overtime Wins">ROW</th>
                <th scope="col" class="px-6 py-3" title="Shootout Wins">S/O Win</th>
                <th scope="col" class="px-6 py-3" title="Goals For">GF</th>
                <th scope="col" class="px-6 py-3" title="Goals Against">GA</th>
                <th scope="col" class="px-6 py-3" title="Goals For per Games Played">GF/GP</th>
                <th scope="col" class="px-6 py-3" title="Goals Against per Games Played">GA/GP</th>
                <th scope="col" class="px-6 py-3" title="Shots per Games Played">Shots/GP</th>
                <th scope="col" class="px-6 py-3" title="Shots Against per Games Played">SA/GP</th>
                <th scope="col" class="px-6 py-3" title="Face-Off win Percentage">FOW%</th>
                <th scope="col" class="px-6 py-3" title="Team Roster">Team Roster</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(team, index) in divisions[selectedDivision]" :key="index" class="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 px-6 py-4 text-gray-400">
                <!-- Team information -->
                <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                  <img class="block w-16 h-16 mb-2" :src="team.teamLogo" :alt="team.displayName + ' logo'">
                  <span class="block text-center">{{ team.displayName }}</span>
                </td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.gamesPlayed}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.wins}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.losses}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.overtimeLosses}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.points}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ (((team.wins / team.gamesPlayed)) * 100).toFixed(1) }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.regulationWin}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.OvertimeWin}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.ShootoutWin}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.GoalsFor}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.GoalsAgainst}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ (((team.GoalsFor / team.gamesPlayed)) * 100).toFixed(1) }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ (((team.GoalsAgainst / team.gamesPlayed)) * 100).toFixed(1) }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.ShotsFor}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.ShotsAgainst}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{team.FaceOffPercentage}}</td>
                <td class="px-6 py-4 text-gray-400 text-center">
                  <router-link :to="'/NHL-team-roster/' + selectedDivision + '/' + team.teamAbv + '/' + team.displayName" @click="$emit('teamSelected', { teamAbv: team.teamAbv, teamName: team.displayName, selectedDivision : selectedDivision })">View Team Roster</router-link>
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
      showNHLDivisionOptionTable: Boolean,
    },
    data(){
      return{
        selectedDivision: 'Select', // Default selection
        divisions: {
            'Central Division': [],
            'Pacific Division': [],
            'Atlantic Division': [],
            'Metropolitan Division': []
          }
      };
    },
    methods: {
      async fetchDivisionStandings() {
        // Set loading of API to true when the call starts
        this.isLoading = true; 
        const baseURL = process.env.VUE_APP_BACKEND_URL;
        const endpoint = `${baseURL}/api/NHLDivision`;

        try {

          const response = await axios.get(endpoint);

          // Check if response.data.response exists
          if (response.data && response.data.body) {
            const teams = response.data.body;

            // Initialize divisions object
            this.divisions = {
                'Central Division': [],
                'Pacific Division': [],
                'Atlantic Division': [],
                'Metropolitan Division': []
            };
            
            // Function to populate divisions for each conference
            const populateDivisions = (team) => {
              this.divisions[team.division + ' Division'].push({
                teamAbv : team.teamAbv,
                teamLogo: team.espnLogo1,
                displayName: team.teamCity + " " + team.teamName,
                gamesPlayed: team.standings.gamesPlayed,
                wins: parseInt(team.standings.W),
                losses: parseInt(team.standings.L),
                overtimeLosses: parseInt(team.standings.OTL),
                points: team.standings.points,
                regulationWin: team.standings.resultDetail.regulationWin,
                OvertimeWin: team.standings.resultDetail.overtimeWin,
                ShootoutWin: team.standings.resultDetail.shootoutWin,
                GoalsFor: team.teamStats.goals,
                GoalsAgainst: team.teamStats.goalsAgainst,
                ShotsFor: team.teamStats.shots,
                ShotsAgainst: team.teamStats.shotsAgainst,
                FaceOffPercentage: ((parseInt(team.teamStats.faceoffsWon) / parseInt(team.teamStats.faceoffs)) * 100).toFixed(1)
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