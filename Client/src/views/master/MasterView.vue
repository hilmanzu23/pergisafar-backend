<template>
    <v-container>
      <v-card>
        <v-tabs v-model="tab" color="#61A3BA" center-active>
          <v-tab value="1">Roles</v-tab>
          <v-tab value="2">Jenis Ikan</v-tab>
          <v-tab value="3">Bahan</v-tab>
          <v-tab value="4">Lokasi</v-tab>
          <v-tab value="5">Status Stock</v-tab>
          <v-tab value="8">Role</v-tab>
          <v-tab value="9">Expedisi</v-tab>
        </v-tabs>
  
        <v-card-text>
          <v-window v-model="tab">
            <v-window-item value="1">
              <RolesView />
            </v-window-item>
            <v-window-item value="2">
              <RolesView />
            </v-window-item>
            <v-window-item value="3">
              <RolesView />
            </v-window-item>
            <v-window-item value="4">
              <RolesView />
            </v-window-item>
            <v-window-item value="5">
              <RolesView />
            </v-window-item>
            <v-window-item value="8">
              <RolesView />
            </v-window-item>
            <v-window-item value="9">
              <RolesView />
            </v-window-item>
          </v-window>
        </v-card-text>
      </v-card>
    </v-container>
  </template>
  
  <script>
  import RolesView from '@views/master/_RolesView.vue';
  
  
  export default {
    data: () => ({
      tab: 'one', // Set the default tab to 'one'
    }),
    components: {
      RolesView,
    },
  };
  </script>
  