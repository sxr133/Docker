<template>
  <div class="overflow-x-auto shadow-md sm:rounded-lg">
    <!-- Pitchers table content -->
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
          <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
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
</template>

<script>
export default {
  props: {
    pitchers: {
      type: Array,
      required: true
    },
    sortBy: String,
    sortDirection: String,
    teamName: String
  }
}
</script>