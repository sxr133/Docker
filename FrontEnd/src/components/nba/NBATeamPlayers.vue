<template>
  <div class="flex justify-center bg-zinc-300 h-fit">
     <div v-if="teamPlayers.length > 0" class="overflow-x-auto shadow-md sm:rounded-lg">
       <!-- Search input -->
       <input type="text" v-model="searchQuery" placeholder="Search..." class="text-gray-400 uppercase dark:bg-gray-800 my-5 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500 placeholder-opacity-100">
 
       <div>
         <button class="back-button" @click="goToDivisionPage">Go to Division Page</button>
       </div>
 
       <table class="mt-4 border-collapse border border-gray-500 w-full md:max-w-screen-xl">
           <colgroup>
             <col style="width: 5%;">
             <col style="width: 15%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
             <col style="width: 8%;">
           </colgroup>
           <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
             <tr>
               <th colspan="17" class="text-center py-4 text-xl font-semibold border border-gray-300 rounded-md">{{ teamName }}</th>
             </tr> 
             <tr>
               <th scope="col" class="px-6 py-3">
                 <span @click="sortByColumn('position')" class="cursor-pointer">
                   POS
                   <span v-if="sortBy === 'position' || sortBy === null" class="ml-1">
                     <span v-if="sortDirection === 'asc'">▲</span>
                     <span v-else-if="sortDirection === 'desc'">▼</span>
                   </span> 
                 </span>   
               </th>
               <th scope="col" class="px-6 py-3">
                 <span @click="sortByColumn('name')" class="cursor-pointer">
                   NAME
                   <span v-if="sortBy === 'name' || sortBy === null" class="ml-1">
                     <span v-if="sortDirection === 'asc'">▲</span>
                     <span v-else-if="sortDirection === 'desc'">▼</span>
                   </span>                
                 </span>
               </th>
               <th scope="col" class="px-6 py-3" title="Games Play">GP</th>
               <th scope="col" class="px-6 py-3" title="Minutes">Min</th>
               <th scope="col" class="px-6 py-3" title="Points">Pts</th>
               <th scope="col" class="px-6 py-3" title="Field Goals Made">FG</th>
               <th scope="col" class="px-6 py-3" title="Field Goals Percentage">FG%</th>
               <th scope="col" class="px-6 py-3" title="Three Point Made">3pt</th>
               <th scope="col" class="px-6 py-3" title="Three Point Percentage">3pt%</th>
               <th scope="col" class="px-6 py-3" title="Free Throws Made">Ftm</th>
               <th scope="col" class="px-6 py-3" title="Free Throws Attempted">Fta</th>
               <th scope="col" class="px-6 py-3" title="Offensive Rebounds">OffReb</th>
               <th scope="col" class="px-6 py-3" title="Defensive Rebounds">DefReb</th>
               <th scope="col" class="px-6 py-3" title="Assists">Ast</th>
               <th scope="col" class="px-6 py-3" title="Steals">Stl</th>
               <th scope="col" class="px-6 py-3" title="Blocks">Blk</th>
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
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.gamesPlay }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.minutes }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.points }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.fgm }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.fgmPercent }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.tpm }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.tpmPercent }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.ftm }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.ftmAttempt }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.offRebounds }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.defRebounds }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.assists }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.steals }}</td>
               <td class="px-6 py-4 text-gray-400 text-center">{{ player.blocks }}</td>
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

  data() {
    return {
      sortBy: null, // Initialize sortBy to null',
      sortDirection: 'asc',
      teamPlayers: [],
      searchQuery: '', // Add searchQuery property
      showDetails: "false",
    };
  },
  mounted() 
  {
    console.log('NHLTeamPlayers component mounted');
    const { teamAbv } = this;
    
     // Example: Fetch data using teamId and division
     this.fetchTeamPlayers(teamAbv);
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
    goToDivisionPage() {
      console.log("Going to Division Page");
      this.$emit('goToDivisionPage');
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
      console.log("Fetching team players...");
      console.log("Team Abv is: ", teamAbv);
      if (!teamAbv) {
        return;
      }
      const endpoint = `https://localhost:7049/api/NBATeamPlayers`;
        try {
          const response = await axios.get(endpoint, {
            headers: {
              teamAbv: teamAbv // Sending the selected conference as a header
            }
          });

      this.teamPlayers = response.data.body.roster.map(athlete => ({
        headshot: athlete.espnHeadshot ? athlete.espnHeadshot : '/images/no-img.png',
        position: athlete.pos ? athlete.pos : 'Unknown Position',
        fullName: athlete.longName,
        gamesPlay: athlete.stats.gamesPlayed,
        points: athlete.stats.pts,
        minutes: athlete.stats.mins,
        fgm: athlete.stats.fgm,
        fgmPercent: athlete.stats.fgp,
        tpm: athlete.stats.tptfgm,
        tpmPercent: athlete.stats.tptfgp,
        ftm: athlete.stats.ftm,
        ftmAttempt: athlete.stats.fta,
        offRebounds: athlete.stats.OffReb,
        defRebounds: athlete.stats.DefReb,
        assists: athlete.stats.ast,
        steals: athlete.stats.stl,
        blocks: athlete.stats.blk
      }));
      console.log("Team players:", this.teamPlayers); // Log the teamPlayers array
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