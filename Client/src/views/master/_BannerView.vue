<template>
  <v-container>
    <v-btn variant="flat" color="#164863" style="width: max-content;" @click="openDialog">
      Tambah
    </v-btn>
    <div class="py-2" />
    <EasyDataTable :headers="headers" :items="items" :rows-per-page="10" show-index buttons-pagination
      :search-field="searchField" :search-value="searchValue">
      <template #item-id="item">
        <div class="operation-wrapper text-right"> <!-- Added 'text-right' class -->
          <v-btn class="ma-2" color="blue" @click="editItem(item)" size="x-small" icon="$edit" variant="outlined">
            <v-icon icon="mdi-pencil" size="20"></v-icon>
          </v-btn>
          <v-btn color="red" @click="deleteItem(item)" size="x-small" icon="$edit" variant="outlined">
            <v-icon icon="mdi-delete" size="20"></v-icon>
          </v-btn>
        </div>
      </template>
      <template #loading>
        <img src="https://i.pinimg.com/originals/94/fd/2b/94fd2bf50097ade743220761f41693d5.gif"
          style="width: 100px; height: 80px;" />
      </template>
      <template #empty-message>
        <v-col>
          <div>Belum ada Data, Tambah Data Disini</div>
        </v-col>
        <v-btn variant="flat" color="#164863" style="width: max-content;" @click="openDialog">
          Tambah
        </v-btn>
      </template>
      <template #customize-headers>
        <thead class="customize-headers">
          <div class="py-2" />
          <tr>
            <th></th>
            <th></th>
          </tr>
          <tr>
            <th class="operation-wrapper text-left px-2">
              <h5>No</h5>
            </th>
            <th class="operation-wrapper text-left px-2">
              <h5>Name</h5>
            </th>
            <th class="operation-wrapper text-right px-2">
              <h5>Actions</h5>
            </th>
          </tr>
        </thead>
      </template>
    </EasyDataTable>
    <div class="py-2" />
    <div class="py-2" />
    <!-- dialog add items -->
    <v-row justify="center">
      <v-dialog v-model="dialog" persistent width="500">
        <v-card class="py-3 px-1">
          <v-card-title>
            <span class="text-h5" v-if="!isEdit">Tambah Data</span>
            <span class="text-h5" v-if="isEdit">Edit Data</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-text-field v-model="newItemName" label="Nama Data*" required></v-text-field>
                </v-col>
              </v-row>
            </v-container>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue-darken-1" variant="text" @click="closeDialog">
              Batal
            </v-btn>
            <v-btn color="blue-darken-1" variant="outlined" @click="saveItem">
              Simpan
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-row>
  </v-container>
</template>
  
<script>
import { MasterService } from '@core/service.js'
import Swal from 'sweetalert2/dist/sweetalert2.js'
export default {
  data() {
    return {
      searchField: "",
      searchValue: "",
      headers: [
        { text: "Nama", value: "name", sortable: true, width: "50%" },
        // { text: "Image", value: "image", sortable: true },
        { text: "Actions", value: "id" }
      ],
      items: [],
      dialog: false,
      isLoading: false,
      isEdit: false,
      newItemName: "",
      newId: ""
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
      this.isEdit = false;
      this.newItemName = ""
    },
    editItem(item) {
      this.dialog = true;
      this.isEdit = true;
      this.newItemName = item.name
      this.newId = item.id
    },
    deleteItem(item) {
      Swal.fire({
        title: 'Error!',
        text: `Yakin ingin hapus data ini?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Batal',
        confirmButtonColor: 'red',
      }).then((result) => {
        if (result.isConfirmed) {
          this.deleteData(item.id)
          Swal.fire('Deleted!', 'Your data has been deleted.', 'success');
        } else if (result.dismiss === Swal.DismissReason.cancel) {
          Swal.fire('Cancelled', 'Your data is safe.', 'info');
        }
      });
    },
    closeDialog() {
      this.dialog = false;
    },
    saveItem() {
      if (this.isEdit) {
        this.updateData()
      }
      if (!this.isEdit) {
        this.postData()
      }
    },
    async getData() {
      const response = await MasterService.getData('Banner')
      this.items = response.data
    },
    async postData() {
      Swal.showLoading()
      this.dialog = false
      const payload = {
        name: this.newItemName,
      };
      const res = await MasterService.postData('Banner', payload)
      if (res.code !== 200) {
        this.dialog = false
        this.isEdit = false
      } else {
        this.getData()
        this.dialog = false
        this.isEdit = false
        Swal.close()
      }
    },
    async updateData() {
      Swal.showLoading()
      this.dialog = false
      const payload = {
        id: this.newId,
        name: this.newItemName,
      };
      const res = await MasterService.putData('Banner', payload)
      if (res.code !== 200) {
        Swal.fire({
          title: 'Error!',
          text: res.errorMessage.Error,
          icon: 'error',
          confirmButtonText: 'OK'
        })
        this.dialog = false
        this.isEdit = false
      } else {
        this.getData()
        this.dialog = false
        this.isEdit = false
        Swal.close()
      }
    },
    async deleteData(item) {
      Swal.showLoading()
      this.dialog = false
      const res = await MasterService.deleteData('Banner', item)
      if (res.code !== 200) {
        Swal.fire({
          title: 'Error!',
          text: res.errorMessage.Error,
          icon: 'error',
          confirmButtonText: 'OK'
        })
        this.dialog = false
        this.isEdit = false
      } else {
        this.getData()
        this.dialog = false
        this.isEdit = false
      }
    }
  },
  created() {
    this.getData()
  }
};
</script>
  