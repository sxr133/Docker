<template>
  <div class="overflow-x-auto shadow-md sm:rounded-lg">
    <!-- Fielders table content -->
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
          <thead class="text-xs text-gray-700 uppercase bg-gray-100 dark:bg-gray-700 dark:text-gray-100">
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
</template>

<script>
export default {
  props: {
    fielders: {
      type: Array,
      required: true
    },
    sortBy: String,
    sortDirection: String,
    teamName: String
  }
}
</script>