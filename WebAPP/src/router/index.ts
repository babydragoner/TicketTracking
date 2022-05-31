import { createRouter, createWebHistory } from 'vue-router'
import TicketView from '../views/ticket/TicketList.vue'
import TicketDetailView from '../views/ticket/info/TicketDetail.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/ticket',
      name: 'ticket',
      component: TicketView, 
    },
    {
      path: '/ticket/info/:id',
      name: 'ticketDetail',
      component: TicketDetailView,
      props: true
    }
  ]
})

export default router
