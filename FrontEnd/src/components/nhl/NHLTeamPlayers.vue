<template>
  <div class="flex justify-center bg-zinc-300 h-fit">
     <div v-if="teamPlayers.length > 0" class="overflow-x-auto shadow-md sm:rounded-lg">
       <!-- Search input -->
       <input type="text" v-model="searchQuery" placeholder="Search..." class="text-gray-400 uppercase dark:bg-gray-800 my-5 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500 placeholder-opacity-100">
 
       <div>
        <button @click="goToDivisionPage(selectedDivision)">Go to Division Page</button>
        <router-link :to="'/Home/'" @click="$emit('goBack', { selectedDivision: selectedDivision })">Go back</router-link>
  
       </div>
 
       <table class="mt-4 border-collapse border border-gray-500 w-full md:max-w-screen-xl">
           <colgroup>
             <col style="width: 15%;">
             <col style="width: 30%;">
             <col style="width: 15%;">
             <col style="width: 5%;">
             <col style="width: 5%;">
             <col style="width: 15%;">
           </colgroup>
           <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
             <tr>
               <th colspan="17" class="text-center py-4 text-xl font-semibold border border-gray-300 rounded-md">{{ teamName }}</th>
             </tr> 
             <tr>
               <th scope="col" class="px-6 py-3">
                 <span @click="sortByColumn('position')" class="cursor-pointer">
                   Position
                   <span v-if="sortBy === 'position' || sortBy === null" class="ml-1">
                     <span v-if="sortDirection === 'asc'">▲</span>
                     <span v-else-if="sortDirection === 'desc'">▼</span>
                   </span> 
                 </span>   
               </th>
               <th scope="col" class="px-6 py-3">
                 <span @click="sortByColumn('name')" class="cursor-pointer">
                   Name
                   <span v-if="sortBy === 'name' || sortBy === null" class="ml-1">
                     <span v-if="sortDirection === 'asc'">▲</span>
                     <span v-else-if="sortDirection === 'desc'">▼</span>
                   </span>                
                 </span>
               </th>
               <th scope="col" class="px-6 py-3" title="Games Played">GP</th>
               <th scope="col" class="px-6 py-3" title="Goals">G</th>
               <th scope="col" class="px-6 py-3" title="Assists">A</th>
               <th scope="col" class="px-6 py-3" title="Points">P</th>
               <th scope="col" class="px-6 py-3" title="Plus Minus">+/-</th>
               <th scope="col" class="px-6 py-3" title="Penalty In Minutes">PIM</th>
               <th scope="col" class="px-6 py-3" title="Power Play Goals">PPG</th>
               <th scope="col" class="px-6 py-3" title="Power Play Assists">PPA</th>
               <th scope="col" class="px-6 py-3" title="Time On Ice Per Games Played">TOI/GP</th>
               <th scope="col" class="px-6 py-3" title="Face-Off Won Percentage">FOW %</th>
               <th scope="col" class="px-6 py-3" title="Goals Against">GA</th>
               <th scope="col" class="px-6 py-3" title="Saves">S</th>
               <th scope="col" class="px-6 py-3" title="Short Handed Saves">SHS</th>
               <th scope="col" class="px-6 py-3" title="Shots Against">SA</th>
               <th scope="col" class="px-6 py-3" title="Shots">S</th>
             </tr>

           </thead>
           <tbody >
             <tr v-for="(player, index) in filteredPlayers" :key="index" 
               :class="{ 'bg-white dark:bg-gray-900': index % 2 === 0, 'bg-gray-50 dark:bg-gray-800': index % 2 !== 0 }"
               class="border-b dark:border-gray-700 px-6 py-4 text-gray-400">
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.position }}</td>
               <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                 <img class="block w-16 h-16 mb-2" :src="player.headshot" :alt="player.fullName + ' headshot'">
                 <span class="block text-center">{{ player.fullName }}</span>
               </td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.gamesPlayed }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ displayGoals }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.assists }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.points }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.plusMinus }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.penaltyInMinutes }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.powerPlayGoals }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.powerPlayAssist }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.timeOnIcePerGamesPlayed }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.faceOffWonPercentage}}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.goalsAgainst }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.saves }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.shortHandedSaves }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.shotsAgainst }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.shots }}</td>
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
    teamAbv: {
      type: String,
      required: true,
    },
    teamName: {
      type: String,
      required: true
    },
    selectedDivision: {
      type: String,
      required: true
    }

  },
  mounted() 
  {
    // Example: Fetch data using teamId and division
    console.log("testing",this.selectedDivision);
    this.fetchTeamPlayers(this.teamAbv);
  },
  data() {
    return {
      sortBy: null, // Initialize sortBy to null',
      sortDirection: 'asc',
      teamPlayers: [],
      searchQuery: '', // Add searchQuery property
      showDetails: "false"
    };
  },
  computed: {
    filteredPlayers() {
      if (!this.teamPlayers) return [];
        return this.teamPlayers.filter(player => {
        // Check if player's properties are defined and contain the search query
          return (
            (player.position && player.position.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
            (player.fullName && player.fullName.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
            // Add more conditions for other properties as needed
            false
          );
        });
    },
  },
  methods: {
    goToDivisionPage(division) {
      console.log("i am in hte go to division");
      console.log("selected division is:",division);
      this.$emit('divisionSelected', division);
    },
    formatDate(dateString) {
      // Parse the date string
      const date = new Date(dateString);
  
      // Extract year, month, and day
      const year = date.getFullYear();
      let month = (date.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-indexed
      let day = date.getDate().toString().padStart(2, '0');
  
      // Format the date as yyyy-mm-dd
      return `${year}-${month}-${day}`;
    },
    sortByColumn(column) {
      this.sortBy = column;
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc'; // Toggle sorting direction

      // Sort the teamPlayers array based on the selected column and sorting direction
      this.teamPlayers.sort((a, b) => {
        if (this.sortDirection === 'asc') {
          return a[column].localeCompare(b[column]);
        } else {
          return b[column].localeCompare(a[column]);
        }
      });
    },
    async fetchTeamPlayers(teamAbv) {
      if (!teamAbv) {
        return;
      }
      const baseURL = process.env.VUE_APP_BACKEND_URL;
      const endpoint = `${baseURL}/api/NHLTeamPlayers`;
      try {
        const response = await axios.get(endpoint, {
          headers: {
            teamAbv: teamAbv // Sending the selected conference as a header
          }
        });
        
        this.teamPlayers = response.data.body.roster.map(athlete => {
      // Check if both gamesPlayed and timeOnIce are valid
      const gamesPlayed = parseInt(athlete.stats.gamesPlayed);
      const timeOnIce = athlete.stats.timeOnIce;
      const validGamesPlayed = !isNaN(gamesPlayed) && gamesPlayed > 0;
      const validTimeOnIce = timeOnIce && /^\d+:\d+$/.test(timeOnIce);

      // If either gamesPlayed or timeOnIce is not valid, set timeOnIcePerGamesPlayed to 'No Data'
      if (!validGamesPlayed || !validTimeOnIce) {
        return {
          headshot: athlete.espnHeadshot ? athlete.espnHeadshot : '/images/no-img.png',
          position: athlete.pos ? athlete.pos : 'Unknown Position',
          fullName: athlete.longName,
          gamesPlayed: validGamesPlayed ? gamesPlayed : 'No Data',
          goals: isNaN(parseInt(athlete.stats.goals)) || athlete.stats.goals === '-' ? 'No Data' : parseInt(athlete.stats.goals),
          assists: isNaN(parseInt(athlete.stats.assists)) ? 'No Data' : parseInt(athlete.stats.assists),
          points: isNaN(parseInt(athlete.stats.goals)) || athlete.stats.goals === '-' && (isNaN(parseInt(athlete.stats.assists)) || athlete.stats.assists === '-') ? 'No Data' : (parseInt(athlete.stats.goals) || 0) + (parseInt(athlete.stats.assists) || 0),
            plusMinus: isNaN(parseInt(athlete.stats.plusMinus)) ? 'No Data' : parseInt(athlete.stats.plusMinus),
            penaltyInMinutes: isNaN(athlete.stats.penaltiesInMinutes) ? 'No Data' : athlete.stats.penaltiesInMinutes,
            powerPlayGoals: isNaN(athlete.stats.powerPlayGoals) ? 'No Data' : athlete.stats.powerPlayGoals,
            powerPlayAssist: isNaN(athlete.stats.powerPlayAssists) ? 'No Data' : athlete.stats.powerPlayAssists,
            timeOnIcePerGamesPlayed: 'No Data', // Set timeOnIcePerGamesPlayed to 'No Data',            
            faceOffWonPercentage: isNaN(((parseInt(athlete.stats.faceoffsWon) / parseInt(athlete.stats.faceoffs))* 100).toFixed(1)) ? 'No Data' : ((parseInt(athlete.stats.faceoffsWon) / parseInt(athlete.stats.faceoffs))* 100).toFixed(1),
            goalsAgainst: isNaN(athlete.stats.goalsAgainst) ? 'No Data' : athlete.stats.goalsAgainst,
            saves:isNaN(athlete.stats.saves) ? 'No Data' : athlete.stats.saves,
            shortHandedSaves:isNaN(athlete.stats.shortHandedSaves) ? 'No Data' : athlete.stats.shortHandedSaves,
            shotsAgainst: isNaN(athlete.stats.shotsAgainst) ? 'No Data' : athlete.stats.shotsAgainst,
            shots: isNaN(athlete.stats.shots) ? 'No Data' : athlete.stats.shots,// Include other properties as needed
         
        };
      }

      // Convert time on ice to minutes
      const [minutes, seconds] = timeOnIce.split(':').map(Number);
      const totalTimeInMinutes = minutes + seconds / 60; // Convert seconds to fraction of minutes

      // Calculate average time on ice per game
      const averageTimePerGame = totalTimeInMinutes / gamesPlayed;

      // Format average time per game as MM:SS
      const averageMinutes = Math.floor(averageTimePerGame);
      const averageSeconds = Math.round((averageTimePerGame - averageMinutes) * 60);
      const averageTimeFormatted = `${averageMinutes}:${averageSeconds.toString().padStart(2, '0')}`;

      return {
        headshot: athlete.espnHeadshot ? athlete.espnHeadshot : '/images/no-img.png',
        position: athlete.pos ? athlete.pos : 'Unknown Position',
        fullName: athlete.longName,
        gamesPlayed: gamesPlayed,
        goals: isNaN(parseInt(athlete.stats.goals)) || athlete.stats.goals === '-' ? 'No Data' : parseInt(athlete.stats.goals),
        assists: isNaN(parseInt(athlete.stats.assists)) ? 'No Data' : parseInt(athlete.stats.assists),
        points: isNaN(parseInt(athlete.stats.goals)) || athlete.stats.goals === '-' && (isNaN(parseInt(athlete.stats.assists)) || athlete.stats.assists === '-') ? 'No Data' : (parseInt(athlete.stats.goals) || 0) + (parseInt(athlete.stats.assists) || 0),
            plusMinus: isNaN(parseInt(athlete.stats.plusMinus)) ? 'No Data' : parseInt(athlete.stats.plusMinus),
            penaltyInMinutes: isNaN(athlete.stats.penaltiesInMinutes) ? 'No Data' : athlete.stats.penaltiesInMinutes,
            powerPlayGoals: isNaN(athlete.stats.powerPlayGoals) ? 'No Data' : athlete.stats.powerPlayGoals,
            powerPlayAssist: isNaN(athlete.stats.powerPlayAssists) ? 'No Data' : athlete.stats.powerPlayAssists,
            timeOnIcePerGamesPlayed: averageTimeFormatted, // Set timeOnIcePerGamesPlayed to the calculated value        
            faceOffWonPercentage: isNaN(((parseInt(athlete.stats.faceoffsWon) / parseInt(athlete.stats.faceoffs))* 100).toFixed(1)) ? 'No Data' : ((parseInt(athlete.stats.faceoffsWon) / parseInt(athlete.stats.faceoffs))* 100).toFixed(1),
            goalsAgainst: isNaN(athlete.stats.goalsAgainst) ? 'No Data' : athlete.stats.goalsAgainst,
            saves:isNaN(athlete.stats.saves) ? 'No Data' : athlete.stats.saves,
            shortHandedSaves:isNaN(athlete.stats.shortHandedSaves) ? 'No Data' : athlete.stats.shortHandedSaves,
            shotsAgainst: isNaN(athlete.stats.shotsAgainst) ? 'No Data' : athlete.stats.shotsAgainst,
            shots: isNaN(athlete.stats.shots) ? 'No Data' : athlete.stats.shots,// Include other properties as needed

      };
    });
    
      } catch (error) {
        console.error('Failed to fetch team players:', error);
      }
    },
 
  },

 
}
</script>

<style>
  .back-button {
    display: inline-block;
    margin-top: 20px;
    padding: 10px 20px;
    background-color: #ccc;
    color: #000;
    text-decoration: none;
    border-radius: 5px;
    cursor: pointer;
  }
</style>