<template>
  <v-layout class="rounded rounded-md">
    <v-app-bar elevation="1" app color="#427D9D">
      <v-app-bar-nav-icon @click="toggleHide"></v-app-bar-nav-icon>
      <v-app-bar-title>Travel Berkah Tracking System</v-app-bar-title>
    </v-app-bar>
    <v-navigation-drawer app v-model="drawer">
      <v-list>
        <v-list-item :style="url === 'Dashboard' ? activeDrawer : ''" link title="Dashboard"
          v-on:click="toggleDrawer('Dashboard')"></v-list-item>
        <v-list-item :style="url === 'Invoice' ? activeDrawer : ''" link title="Purchase Order"
          v-on:click="toggleDrawer('Invoice')"></v-list-item>
        <v-list-item :style="url === 'ProcessingPlan' ? activeDrawer : ''" link title="Penempatan"
          v-on:click="toggleDrawer('ProcessingPlan')"></v-list-item>
        <v-divider></v-divider>
        <v-list-item :style="url === 'Master' ? activeDrawer : ''" link title="Master Data"
          v-on:click="toggleDrawer('Master')"></v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-main class="d-flex justify-center" style="min-height: 300px;">
      <router-view></router-view>
    </v-main>
  </v-layout>
</template>

<style>
.custom-active {
  /* Define your custom styles for active v-list-item here */
  background-color: #ff5733;
  /* Example background color */
  color: white;
  /* Example text color */
}
</style>

<script>
import { MasterService } from '@core/service.js'
import Swal from 'sweetalert2/dist/sweetalert2.js'

export default {
  data() {
    return {
      url: "Dashboard",
      drawer: true,
      activeDrawer: 'border-radius: 5px;margin: 10px;background-color: #427D9D; color: white;'
    }
  },
  methods: {
    toggleHide() {
      this.drawer = !this.drawer;
    },
    async toggleDrawer(data) {
      window.location.href = data
      this.geturl()
    },
    async addInvoice(data) {
      Swal.showLoading();
      const response = await MasterService.postData("Invoice")
      if (response.code !== 200) {
        Swal.fire({
          title: 'Error!',
          text: response.errorMessage.Error,
          icon: 'error',
          confirmButtonText: 'OK'
        })
        Swal.close()
      } else {
        window.location.href = data + "?id=" + response.id
        this.geturl()
        Swal.close()
      }
    },
    async geturl() {
      const currentURL = window.location.href;
      const parsedURL = new URL(currentURL);
      const pathname = parsedURL.pathname;
      const pathSegments = pathname.split('/');
      const filteredSegments = pathSegments.filter(segment => segment !== '');
      this.url = filteredSegments.pop();
    }
  },
  created() {
    this.geturl()
  }
}
</script>