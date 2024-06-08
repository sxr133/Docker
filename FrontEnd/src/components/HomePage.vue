<template>
 <section class="py-10 md:py-10 bg-zinc-300 h-fit">
    <div class="container max-w-screen mx-auto px-4">
      
        <div class="flex justify-center items-center border-8 rounded-md border-gray-500 bg-slate-200">
          <div class="flex space-x-8 cursor-pointer mx-5 my-5">
            <img src="/images/NHL-Logo.png" alt="NHL Logo" class="border-4 border-gray-500 rounded-xl" @click="showTable('nhl')">
            <img src="/images/MLB-Logo.png" alt="MLB Logo" class="border-4 border-gray-500 rounded-xl" @click="showTable('mlb')">
            <img src="/images/NBA-Logo.png" alt="NBA Logo" class="border-4 border-gray-500 rounded-xl" @click="showTable('nba')">
          </div>
        </div>

        <div v-if="(showDropDown && !showNHLTeamPlayers) || (showDropDown && !showNBATeamPlayers) || (showDropDown && !showMLBTeamPlayers)" class="flex justify-center my-4">
          <select v-model="selectedStats" @change="showStats(selectedStats)" class="block w-1/2 p-2 text-gray-700 bg-white border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none focus:ring">
            <option value="Select" disabled selected>Select an option</option>
            <option value="Conferences">Conferences</option>
            <option value="Divisions">Divisions</option>
          </select>
        </div>

        <!-- NHL Conference Component -->
        <div v-if="currentLeague === 'nhl'">
          <NHLConference 
            :showNHLTable="showNHLTable"
            :showNHLDivisionTable="false"
          />
        
          <div v-if="showNHLDivisionTable">
            <!-- NHL Division Component -->
            <NHLDivision 
              :showNHLDivisionOptionTable = "true"
              @teamSelected = "showTeamPlayers" 
             
            />
          </div>

          <!-- NHL Team Players Component -->
          <div v-if="showNHLTeamPlayers">
            <NHLTeamPlayers 
              :teamName = "selectedTeamName"
              :teamAbv = "selectedTeamAbv"
              @goBack="handleGoToDivisionPage"
            />
          </div>
        </div>
        
        <div v-if="currentLeague === 'nba'">
          <NBAConference 
            :showNBATable="showNBATable"
          />

          <div v-if="!showNBATeamPlayers">
            <!-- NBA Division Component -->
            <NBADivision 
              :showDivisionOptionTable = "showDivisionOptionTable"
            />
          </div>
        </div>

        <div v-if="currentLeague === 'mlb'">
          <MLBConference 
            :showMLBTable="showMLBTable"
          />

          <div v-if="!showMLBTeamPlayers">
            <!-- MLB Division Component -->
            <MLBDivision 
              :showDivisionOptionTable = "showDivisionOptionTable"
            />
          </div>
        </div>

    </div>
  </section>
</template>

<script>
  import MLBConference from './mlb/MLBConference.vue';
  import MLBDivision from './mlb/MLBDivision.vue';
  import NBAConference from './nba/NBAConference.vue';
  import NBADivision from './nba/NBADivision.vue';
  import NHLConference from './nhl/NHLConference.vue';
  import NHLDivision from './nhl/NHLDivision.vue';
  import NHLTeamPlayers from './nhl/NHLTeamPlayers.vue';

  export default {
  
    components:{
      MLBConference,
      MLBDivision,
      NBAConference,
      NBADivision,
      NHLConference,
      NHLDivision,
      NHLTeamPlayers
      
    },
    data() {
      return {
        showDropDown: false,
        selectedStats: "Select", 
        showNHLTable: false,
        showNHLDivisionTable: false,
        showNHLDivisionOptionTable:false,
        showDivisionOptionTable: false,
        showNHLTeamPlayers: false,
        selectedTeamAbv: '',
        selectedTeamName: '',
        currentLeague: null,

        showMLBTable: false,
        
        showMLBTeamPlayers: false,

        showNBATable: false,
        
        showNBATeamPlayers: false,
        
      };
    },
    methods: {

     showStats(option) {

        this.showNHLTable = false;
        this.showNBATable = false;
        this.showMLBTable = false;
        this.showNHLDivisionOptionTable = false;
        this.showDivisionOptionTable = false;

        if (this.currentLeague === 'nhl') {
          console.log("changing selection");
          if (option === 'Conferences') {
            console.log("changing selection in conferences");
            // Logic for showing conference stats
            this.showNHLTable = true;
            this.showNHLDivisionTable = false;
            this.showNHLDivisionOptionTable = false;
          } else if (option === 'Divisions') {
            console.log("changing selection in divisions");
            // Logic for showing division stats
            this.showNHLDivisionTable = true;
            this.showNHLDivisionOptionTable = true;
            this.showNHLTable = false;
          }
        }else if (this.currentLeague === 'mlb') {
          if (option === 'Conferences') {
           // Logic for showing conference stats
            this.showMLBTable = !this.showMLBTable;
            this.showDivisionOptionTable = false;
          } else if (option === 'Divisions') {
            // Logic for showing division stats
            this.showDivisionOptionTable = !this.showDivisionOptionTable;
            this.showMLBTable = false;
          }
        }else if (this.currentLeague === 'nba') {
          if (option === 'Conferences') {
            // Logic for showing conference stats
            this.showNBATable = !this.showNBATable;
            this.showDivisionOptionTable = false;
          } else if (option === 'Divisions') {
            // Logic for showing division stats
            this.showDivisionOptionTable = !this.showDivisionOptionTable;
            this.showNBATable = false;
          }
        }
      },
      showTable(league) {
        
        if (league === 'nhl') {
          console.log("NHL Selected");
          this.currentLeague = 'nhl';
          this.showDropDown = true;
          this.showNHLDivisionTable = false;
          this.showMLBTable = false;
          this.showNBATable = false;
          this.showNHLTeamPlayers = false;
          this.showNBATeamPlayers = false;
          this.selectedStats = "Select";
          this.showNHLDivisionOptionTable = false;
        } 
        else if (league === 'mlb') {
          console.log("MLB Selected")
          this.currentLeague = 'mlb';
          this.showDropDown = true;
          this.showMLBTeamPlayers = false;
          this.showMLBWesternTable = false;
          this.showMLBEasternTable = false;
          this.showNHLTable = false;
          this.showNBATable = false;
          this.showNHLTeamPlayers = false;
          this.selectedStats = "Select";
          this.showDivisionOptionTable = false;
        }
        else if (league === 'nba') {
          console.log("NBA Selected")
          this.currentLeague = 'nba';
          this.showDropDown = true;
          this.showNBATeamPlayers = false;
          this.showNBAWesternTable = false;
          this.showNBAEasternTable = false;
          this.showNHLTable = false;
          this.showMLBTable = false;
          this.showNHLTeamPlayers = false;
          this.selectedStats = "Select";
          this.showDivisionOptionTable = false;
        }
      },
      showNHLDivision(conference) {
        if (conference === 'western') {
          this.showNHLWesternDivisionTitle = true;
          this.showNHLEasternDivisionTitle = false;
          this.showNHLWesternTable = !this.showNHLWesternTable;
          this.showNHLEasternTable = false;
        } else if (conference === 'eastern') {
          this.showNHLEasternTable = !this.showNHLEasternTable;
          this.showNHLWesternDivisionTitle = false;
          this.showNHLEasternDivisionTitle = true;
          this.showNHLWesternTable = false;
        }
      },
      showMLBDivision(conference) {
        if (conference === 'al') {
          this.showMLBWesternDivisionTitle = true;
          this.showMLBEasternDivisionTitle = false;
          this.showMLBALTable = !this.showMLBALTable;
          this.showMLBNLTable = false;
        } else if (conference === 'nl') {
          this.showMLBEasternDivisionTitle = true;
          this.showMLBWesternDivisionTitle = false;
          this.showMLBNLTable = !this.showMLBNLTable;
          this.showMLBALTable = false;
        }
      },
      showNBADivision(conference) {
        if (conference === 'western') {
          this.showNBAWesternDivisionTitle = true;
          this.showNBAEasternDivisionTitle = false;
          this.showNBAWesternTable = !this.showNBAWesternTable;
          this.showNBAEasternTable = false;
        } else if (conference === 'eastern') {
          this.showNBAEasternTable = !this.showNBAEasternTable;
          this.showNBAWesternDivisionTitle = false;
          this.showNBAEasternDivisionTitle = true;
          this.showNBAWesternTable = false;
        }
      },
       // Update this method to set showNHLTeamPlayers to true and selectedTeamId to the selected team's ID
      showTeamPlayers(teamInfo) {
        console.log("home page team info is: ", teamInfo);
        this.showNHLTeamPlayers = true;
        this.selectedTeamAbv = teamInfo.teamAbv;
        this.selectedTeamName = teamInfo.teamName;
      }
    }
  };
</script>

<style>
  .logo {
    width: 500px; 
    cursor: pointer;
    border: solid;
  }
</style>