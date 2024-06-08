import { createRouter, createWebHistory } from 'vue-router';
import NHLTeamPlayers from './components/nhl/NHLTeamPlayers.vue';
import MLBTeamPlayers from './components/mlb/MLBTeamPlayers.vue';
import NBATeamPlayers from './components/nba/NBATeamPlayers.vue';
import SignupForm from './components/SignupForm.vue';
import LoginForm from './components/LoginForm.vue';
import HomePage from './components/HomePage.vue';


// Define routes
const routes = [
  {
    path: '/',
    name: 'LoginForm',
    component: LoginForm,
    props: true
  },
  {
    path: '/home',
    name: 'homePage',
    component: HomePage,
    props: true
  },
  {
    path: '/MLB-team-roster/:selectedDivision/:teamAbv/:teamName',
    name: 'mlb-team-roster',
    component: MLBTeamPlayers,
    props: route => ({ selectedDivision: route.params.selectedDivision, teamAbv: route.params.teamAbv, teamName: route.params.teamName }),
    beforeEnter: (to, from, next) => {
      console.log('Navigating to MLB team-roster route');
      next(); 
    }
  },
  {
    path: '/NHL-team-roster/:selectedDivision/:teamAbv/:teamName',
    name: 'nhl-team-roster',
    component: NHLTeamPlayers,
    props: route => ({ selectedDivision: route.params.selectedDivision, teamAbv: route.params.teamAbv, teamName: route.params.teamName }),
    beforeEnter: (to, from, next) => {
      console.log('Route params:', to.params);
      console.log('Navigating to NHL team-roster route');
      next(); 
    }
  },
  {
    path: '/NBA-team-roster/:selectedDivision/:teamAbv/:teamName',
    name: 'nba-team-roster',
    component: NBATeamPlayers,
    props: route => ({ selectedDivision: route.params.selectedDivision, teamAbv: route.params.teamAbv, teamName: route.params.teamName }),
    beforeEnter: (to, from, next) => {
      console.log('Navigating to NBA team-roster route');
      next(); 
    }
  },
  {
    path: '/signup',
    name: 'SignupForm',
    component: SignupForm,
    props: false
  }
];

// Create router instance
const router = createRouter({
  history: createWebHistory(),  // Using HTML5 history mode
  routes,
});

export default router;