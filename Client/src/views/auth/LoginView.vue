<template>
    <v-container fluid>
        <v-row align="center" justify="center" style="height: 100vh;">
            <v-col cols="12" sm="5">
                <!-- <div style="font-family:'Popins', Tahoma, Geneva, Verdana, sans-serif; text-align: center; padding: 10%;">
                    Travel Berkah Admin</div> -->
                <v-form validate-on="submit lazy" @submit.prevent="submit"
                    style="background-color:aliceblue; padding: 5%; border-radius:50px;">
                    <v-text-field v-model="email" label="Email" :rules="[rulesEmail.required]"></v-text-field>
                    <v-text-field v-model="password" :append-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                        :type="showPassword ? 'text' : 'password'" label="Password"
                        @click:append="showPassword = !showPassword" :rules="[rulesPassword.required]"></v-text-field>
                    <v-btn variant="elevated" style="color: blue;" :loading="loading" type="submit" block class="mt-2"
                        text="Masuk"></v-btn>
                </v-form>
            </v-col>
        </v-row>
    </v-container>
</template>
<script>
import { MasterService } from '@core/service.js'
import Swal from 'sweetalert2/dist/sweetalert2.js'
export default {

    data() {
        return {
            email: "",
            password: "",
            loading: false,
            showPassword: false,
            rulesPassword: {
                required: value => !!value || 'Password is required',
            },
            rulesEmail: {
                required: value => !!value || 'Email is required',
            },
        };
    },
    methods: {
        async submit() {
            Swal.showLoading()
            this.dialog = false
            const payload = {
                email: this.email,
                password: this.password,
            };
            const res = await MasterService.postData('Auth/Login', payload)
            if (res.code !== 200) {
                this.dialog = false
                this.isEdit = false
            } else {
                localStorage.setItem('access_token', res.accessToken)
                localStorage.setItem('id', res.id)
                window.location.href = 'Master'
                this.dialog = false
                this.isEdit = false
                Swal.close()
            }
        },
    }
}
</script>