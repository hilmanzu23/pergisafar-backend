<template>
    <v-container>
      <v-card>
        <v-tabs v-model="tab" color="#61A3BA" center-active>
          <v-tab value="1">Roles</v-tab>
          <v-tab value="2">Status</v-tab>
          <v-tab value="3">Pengaturan</v-tab>
          <v-tab value="4">Banner</v-tab>
        </v-tabs>
  
        <v-card-text>
          <v-window v-model="tab">
            <v-window-item value="1">
              <RolesView />
            </v-window-item>
            <v-window-item value="2">
              <StatusView />
            </v-window-item>
            <v-window-item value="3">
              <SettingView />
            </v-window-item>
            <v-window-item value="4">
              <BannerView />
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </v-container>
  </template>
  
  <script>
  import RolesView from '@views/master/_RolesView.vue';
  import StatusView from '@views/master/_StatusView.vue';
  import SettingView from '@views/master/_SettingView.vue';
  import BannerView from '@views/master/_BannerView.vue'

  
  
  export default {
    data: () => ({
      tab: 'one', // Set the default tab to 'one'
    }),
    components: {
      RolesView,
      StatusView,
      SettingView,
      BannerView
    },
  };
  </script>
  