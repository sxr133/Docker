<template>
  <div>
    <!-- Conditional rendering of NHL table -->
   
    <div v-if="showNHLTable" class="relative overflow-x-auto shadow-md sm:rounded-lg">
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
            </colgroup>
            <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
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
              </tr>
            </thead>
        <tbody>
          <tr v-for="(team, index) in selectedConference === 'Eastern Conference' ? easternConference : westernConference" :key="index" class="odd:bg-white odd:dark:bg-gray-900 even:bg-gray-50 even:dark:bg-gray-800 border-b dark:border-gray-700 px-6 py-4 text-gray-400">
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
      showNHLTable: Boolean,
    },
    data(){
      return{
        selectedTeamId: '',
        selectedConference: 'Select', // Default selection
        westernConference: [],
        easternConference: [],
        isLoading: false, // Added to track when the API call is happening
      };
    },
    methods: {
      async fetchConferenceStandings() {
        this.isLoading = true; // Set loading of API to true when the call starts
        const endpoint = `https://localhost:7049/api/NHLConference`;
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
                gamesPlayed: entry.standings.gamesPlayed,
                wins: parseInt(entry.standings.W),
                losses: parseInt(entry.standings.L),
                overtimeLosses: parseInt(entry.standings.OTL),
                points: entry.standings.points,
                regulationWin: entry.standings.resultDetail.regulationWin,
                OvertimeWin: entry.standings.resultDetail.overtimeWin,
                ShootoutWin: entry.standings.resultDetail.shootoutWin,
                GoalsFor: entry.teamStats.goals,
                GoalsAgainst: entry.teamStats.goalsAgainst,
                ShotsFor: entry.teamStats.shots,
                ShotsAgainst: entry.teamStats.shotsAgainst,
                FaceOffPercentage: ((parseInt(entry.teamStats.faceoffsWon) / parseInt(entry.teamStats.faceoffs)) * 100).toFixed(1),
                conference: conference
            }));
            };

            // Extract NL conference teams
            this.easternConference = extractTeamData(result.filter(team => team.conference === 'Eastern'), 'easternConference');

            // Extract AL conference teams
            this.westernConference = extractTeamData(result.filter(team => team.conference === 'Western'), 'westernConference');

            // Sort NL conference array by wins
            this.easternConference.sort((a, b) => b.wins - a.wins);

            // Sort AL conference array by wins
            this.westernConference.sort((a, b) => b.wins - a.wins);

          } else {
            console.error('Unexpected response format:', response.data);
          }
        } catch (error) {
          console.error('Error fetching NHL conference standings:', error);
        } finally {
          this.isLoading = false; // Reset loading state whether the call succeeds or fails
        }
      },
    },
  };
</script>