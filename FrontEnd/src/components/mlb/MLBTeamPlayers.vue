<template>
  <div class="flex justify-center bg-zinc-300 h-fit">
    <!-- Dropdown box -->
    <div class="overflow-x-auto shadow-md sm:rounded-lg">
      <div class="flex justify-center my-4">
        <select v-model="selectedPosition" class="block w-150 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
          <option value="Select" disabled selected>Select a statistical table to view</option>
          <option value="Pitchers">Pitchers</option>
          <option value="Fielders">Fielders</option>
        </select>
      </div>

      <!-- Pitchers table -->
      <div class="overflow-x-auto max-h-200">
        <div v-if="selectedPosition === 'Pitchers' && pitchers.length > 0" class="overflow-x-auto shadow-md sm:rounded-lg">
          <input type="text" v-model="searchQuery" placeholder="Search..." class="text-gray-400 uppercase dark:bg-gray-800 my-5 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500 placeholder-opacity-100">
          <button class="back-button" @click="goToDivisionPage">Go to Division Page</button>
  

          <!--Pitchers statistics-->
          <table class="mt-4 border-collapse border border-gray-500 w-full md:max-w-screen-xl">
            <colgroup>
              <col style="width: 5%;">
              <col style="width: 15%;">
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
            <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100 sticky top-0">
              <tr>
                <th colspan="18" class="text-center py-4 text-xl font-semibold border border-gray-300 rounded-md">{{ teamName }}</th>
              </tr> 
              <tr>
                <th scope="col" class="px-6 py-3" title="Position">Pos</th>
                <th scope="col" class="px-6 py-3">
                  <span @click="sortPitchersByColumn('name')" class="cursor-pointer">
                    Name
                    <span v-if="sortBy === 'name' || sortBy === null" class="ml-1">
                      <span v-if="sortDirection === 'asc'">▲</span>
                      <span v-else-if="sortDirection === 'desc'">▼</span>
                    </span>                
                  </span>
                </th>
                <th scope="col" class="px-6 py-3" title="Wins">W</th>
                <th scope="col" class="px-6 py-3" title="Losses">L</th>
                <th scope="col" class="px-6 py-3" title="Earned Run Average">ERA</th>
                <th scope="col" class="px-6 py-3" title="Games">Games</th>
                <th scope="col" class="px-6 py-3" title="Games Started">GS</th>
                <th scope="col" class="px-6 py-3" title="Saves">SV</th>
                <th scope="col" class="px-6 py-3" title="Saves Opportunities">SVO</th>
                <th scope="col" class="px-6 py-3" title="Innings Pitch">IP</th>
                <th scope="col" class="px-6 py-3" title="Hits">H</th>
                <th scope="col" class="px-6 py-3" title="Runs">R</th>
                <th scope="col" class="px-6 py-3" title="Earned Runs">ER</th>
                <th scope="col" class="px-6 py-3" title="Home Runs">HR</th>
                <th scope="col" class="px-6 py-3" title="Walks">BB</th>
                <th scope="col" class="px-6 py-3" title="Strike Outs">SO</th>
                <th scope="col" class="px-6 py-3" title="Walks and Hits Per Inning Pitch">WHIP</th>
                <th scope="col" class="px-6 py-3" title="Batting Average Against">AVG</th>
              </tr>
            </thead>
            <tbody >
              <tr v-for="(player, index) in filteredPitchers" :key="index" 
                :class="{ 'bg-white dark:bg-gray-900': index % 2 === 0, 'bg-gray-50 dark:bg-gray-800': index % 2 !== 0 }"
                class="border-b dark:border-gray-700 px-6 py-4 text-gray-400">
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.position }}</td>
                <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                  <img class="block w-12 h-12 mb-2" :src="player.headshot" :alt="player.fullName + ' headshot'">
                  <span class="block text-center">{{ player.fullName }}</span>
                </td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.wins }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.loss }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.era }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.games }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.gamesStarted }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.saves }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.savesOpportunities }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.inningsPitch }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.hits }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.runs }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.earnedRuns }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.homeRuns }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.walks }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.strikeOuts }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.whip }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.avg }}</td>
              </tr>  
            </tbody>
          </table>
        </div>
        <div v-else-if="selectedPosition === 'Fielders' && nonPitchers.length > 0" class="overflow-x-auto shadow-md sm:rounded-lg">
          <input type="text" v-model="searchQuery" placeholder="Search..." class="text-gray-400 uppercase dark:bg-gray-800 my-5 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-blue-500 placeholder-opacity-100">

          <div> 
            <button class="back-button" @click="goToDivisionPage">Go to Division Page</button>
          </div>  

          <!--Fielders statistics-->
          <table class="mt-4 border-collapse border border-gray-500 w-full md:max-w-screen-xl">
            <colgroup>
              <col style="width: 5%;">
              <col style="width: 15%;">
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
            <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100 sticky top-0">
              <tr>
                <th colspan="18" class="text-center py-4 text-xl font-semibold border border-gray-300 rounded-md">{{ teamName }}</th>
              </tr> 
              <tr>
                <th scope="col" class="px-6 py-3" title="Position">
                  <span @click="sortFieldersByColumn('position')" class="cursor-pointer">
                    POS
                    <span v-if="sortBy === 'position' || sortBy === null" class="ml-1">
                      <span v-if="sortDirection === 'asc'">▲</span>
                      <span v-else-if="sortDirection === 'desc'">▼</span>
                    </span> 
                  </span>   
                </th>
                <th scope="col" class="px-6 py-3" title="Name">
                  <span @click="sortFieldersByColumn('name')" class="cursor-pointer">
                    NAME
                    <span v-if="sortBy === 'name' || sortBy === null" class="ml-1">
                      <span v-if="sortDirection === 'asc'">▲</span>
                      <span v-else-if="sortDirection === 'desc'">▼</span>
                    </span>                
                  </span>
                </th>
                <th scope="col" class="px-6 py-3" title="Games Played">G</th>
                <th scope="col" class="px-6 py-3" title="At Bat">AB</th>
                <th scope="col" class="px-6 py-3" title="Run">R</th>
                <th scope="col" class="px-6 py-3" title="Hits">H</th>
                <th scope="col" class="px-6 py-3" title="Doubles">2B</th>
                <th scope="col" class="px-6 py-3" title="Triples">3B</th>
                <th scope="col" class="px-6 py-3" title="Home Runs">HR</th>
                <th scope="col" class="px-6 py-3" title="Runs Batted In">RBI</th>
                <th scope="col" class="px-6 py-3" title="Walks">BB</th>
                <th scope="col" class="px-6 py-3" title="Strike Outs">SO</th>
                <th scope="col" class="px-6 py-3" title="Stolen Bases">SB</th>
                <th scope="col" class="px-6 py-3" title="Caught Stealing">CS</th>
                <th scope="col" class="px-6 py-3" title="Batting Average">AVG</th>
                <th scope="col" class="px-6 py-3" title="On Base Percentage">OBP</th>
                <th scope="col" class="px-6 py-3" title="Slugging Percentage">SLG</th>
                <th scope="col" class="px-6 py-3" title="On-Base plus Slugging Percentage">OPS</th>
              </tr>
            </thead>
            <tbody >
              <tr v-for="(player, index) in filteredFielders" :key="index" 
                :class="{ 'bg-white dark:bg-gray-900': index % 2 === 0, 'bg-gray-50 dark:bg-gray-800': index % 2 !== 0 }"
                class="border-b dark:border-gray-700 px-6 py-4 text-gray-400">
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.position }}</td>
                <td class="flex flex-col items-center justify-center px-6 py-4 text-gray-400">
                  <img class="block w-12 h-12 mb-2" :src="player.headshot" :alt="player.fullName + ' headshot'">
                  <span class="block text-center">{{ player.fullName }}</span>
                </td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.games }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.atBat }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.run }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.hit }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.double }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.triple }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.homeRun }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.rbi }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.walks }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.strikeOuts }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.stolenBase }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.caughtStealing }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.avg }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.obp }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.slg }}</td>
                <td class="px-6 py-4 text-gray-400 text-center">{{ player.ops }}</td>
              </tr>  
            </tbody>
          </table>
        </div>
      </div>
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
      selectedPosition: 'Select',
      sortBy: null,
      sortDirection: 'asc',
      pitchers: [], // Add pitchers array
      nonPitchers: [], // Add nonPitchers array
      searchQuery: '',
      showDetails: "false",
    };
  },
  mounted() 
  {
    console.log('NHLTeamPlayers component mounted');
    const { teamAbv } = this;
    
    this.fetchTeamPlayers(teamAbv);
  },
  computed: {
    filteredPitchers() {
      if (!this.pitchers) return [];
      return this.pitchers.filter(player => {
        return (
          (player.position && player.position.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
          (player.fullName && player.fullName.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
          false
        );
      });
    },
    filteredFielders() {
      if (!this.nonPitchers) return [];
      return this.nonPitchers.filter(player => {
        return (
          (player.position && player.position.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
          (player.fullName && player.fullName.toLowerCase().includes(this.searchQuery.toLowerCase())) ||
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
    sortFieldersByColumn(column) {
      this.sortBy = column;
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';

      this.nonPitchers.sort((a, b) => {
        if (this.sortDirection === 'asc') {
          return a[column].localeCompare(b[column]);
        } else {
          return b[column].localeCompare(a[column]);
        }
      });
    },
    sortPitchersByColumn(column) {
      this.sortBy = column;
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';

      this.Pitchers.sort((a, b) => {
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
      const baseURL = process.env.VUE_APP_BACKEND_URL;
      const endpoint = `${baseURL}/api/MLBTeamPlayers`;
      try {
        const response = await axios.get(endpoint, {
          headers: {
            teamAbv: teamAbv
          }
        });
        console.log("response is: ", response);

        const pitchers = [];
        const nonPitchers = [];

        response.data.body.roster.forEach(athlete => {
          if (athlete.pos === 'P') {
            console.log("issue with ", athlete.longName);
            pitchers.push({
              headshot: athlete.espnHeadshot ? athlete.espnHeadshot : '/images/no-img.png',
              position: athlete.pos ? athlete.pos : 'Unknown Position',
              fullName: athlete.longName,
              wins: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.Win ? athlete.stats.Pitching.Win : 'No Data',
              loss: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.Loss ? athlete.stats.Pitching.Loss : 'No Data',
              games: athlete.stats && athlete.stats.gamesPlayed ? athlete.stats.gamesPlayed : 'No Data',
              gamesStarted: athlete.stats && athlete.stats.gamesStarted ? athlete.stats.gamesStarted : 'No Data',
              saves: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.Save ? parseInt(athlete.stats.Pitching.Save) : 'No Data',
              savesOpportunities: athlete.stats && athlete.stats.Pitching && (parseInt(athlete.stats.Pitching.BlownSave) + athlete.stats && athlete.stats.Pitching && parseInt(athlete.stats.Pitching.Save)) || 'No Data',
              inningsPitch: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.InningsPitched ? athlete.stats.Pitching.InningsPitched : 'No Data',
              hits: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.H ? athlete.stats.Pitching.H : 'No Data',
              runs: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.R ? athlete.stats.Pitching.R : 'No Data',
              earnedRuns: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.ER ? athlete.stats.Pitching.ER : 'No Data',
              homeRuns: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.HR ? athlete.stats.Pitching.HR : 'No Data',
              walks: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.BB ? athlete.stats.Pitching.BB : 'No Data',
              strikeOuts: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.SO ? athlete.stats.Pitching.SO : 'No Data',
              whip: athlete.stats && athlete.stats.Pitching && athlete.stats.Pitching.WHIP ? athlete.stats.Pitching.WHIP : 'No Data',
              avg: athlete.stats && athlete.stats.Pitching && (parseInt(athlete.stats.Pitching.H) / athlete.stats && athlete.stats.Pitching && parseInt(athlete.stats.Pitching['Batters Faced'])).toFixed(3) || 'No Data',
              era: athlete.stats && athlete.stats.Pitching && ((parseInt(athlete.stats.Pitching.ER) * 9) / athlete.stats && athlete.stats.Pitching && athlete.stats && athlete.stats.Pitching && parseInt(athlete.stats.Pitching.InningsPitched)).toFixed(2) || 'No Data'
           });
          } else {
            const AB = parseInt(athlete.stats.Hitting.AB);
            const H = parseInt(athlete.stats.Hitting.H);
            const BB = parseInt(athlete.stats.Hitting.BB);
            const HBP = parseInt(athlete.stats.Hitting.HBP);
            const SF = parseInt(athlete.stats.Hitting.SF);
            const _1B = H - (parseInt(athlete.stats.Hitting['2B']) + parseInt(athlete.stats.Hitting['3B']) + parseInt(athlete.stats.Hitting.HR));

            const obp = (H + BB + HBP) / (AB + BB + HBP + SF);
            const slg = (_1B + (parseInt(athlete.stats.Hitting['2B']) * 2) + (parseInt(athlete.stats.Hitting['3B']) * 3) + (parseInt(athlete.stats.Hitting.HR) * 4)) / AB;
            const ops = obp + slg;

            nonPitchers.push({
              headshot: athlete.espnHeadshot ? athlete.espnHeadshot : '/images/no-img.png',
              position: athlete.pos ? athlete.pos : 'Unknown Position',
              fullName: athlete.longName,
              games: athlete.stats.gamesPlayed,
              atBat: AB,
              run: parseInt(athlete.stats.Hitting.R),
              hit: H,
              double: parseInt(athlete.stats.Hitting['2B']),
              triple: parseInt(athlete.stats.Hitting['3B']),
              homeRun: parseInt(athlete.stats.Hitting.HR),
              rbi: parseInt(athlete.stats.Hitting.RBI),
              walks: BB,
              strikeOuts: parseInt(athlete.stats.Hitting.SO),
              stolenBase: parseInt(athlete.stats.BaseRunning.SB),
              caughtStealing: parseInt(athlete.stats.BaseRunning.CS),
              avg: athlete.stats.Hitting.avg,
              obp: parseFloat(obp).toFixed(3),
              slg: parseFloat(slg).toFixed(3),
              ops: parseFloat(ops).toFixed(3)
            });
          }
        });

        this.pitchers = pitchers;
        this.nonPitchers = nonPitchers;

        console.log("Pitchers:", this.pitchers);
        console.log("Non-Pitchers:", this.nonPitchers);
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