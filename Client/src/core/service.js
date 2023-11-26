import axios from 'axios'
import Swal from 'sweetalert2/dist/sweetalert2.js'

export const MasterService = {
    async getData(endpoint) {
        try {
            const response = await axios.get(`/${endpoint}`)
            return response.data
        } catch (error) {
            if(error.response.data.code == 401){
                return Swal.fire({
                title: 'Opss!',
                text: error.response.data.errorMessage.error,
                icon: 'danger',
                showCancelButton: false,
                confirmButtonText: 'Login Aplikasi',
                cancelButtonText: 'Batal',
                confirmButtonColor: 'red',
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('access_token')
                        window.location.href = 'Login'
                    }
                });
            }
            Swal.fire({
                title: 'Error!',
                text: error.response.data.errorMessage.Email || error.response.data.errorMessage.Password,
                icon: 'error',
                confirmButtonText: 'OK'
            })
            return error.response.data
        }
    },
    async postData(endpoint, payload) {
        try {
            const response = await axios.post(`/${endpoint}`, payload)
            return response.data
        } catch (error) {
            if(error.response.data.code == 401){
                return Swal.fire({
                title: 'Opss!',
                text: error.response.data.errorMessage.error,
                icon: 'danger',
                showCancelButton: false,
                confirmButtonText: 'Login Aplikasi',
                cancelButtonText: 'Batal',
                confirmButtonColor: 'red',
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('access_token')
                        window.location.href = 'Login'
                    }
                });
            }
            Swal.fire({
                title: 'Error!',
                text: error.response.data.errorMessage.Email || error.response.data.errorMessage.Password,
                icon: 'error',
                confirmButtonText: 'OK'
            })
            return error.response.data
        }
    },
    async getId(endpoint, payload) {
        try {
            const response = await axios.get(`/${endpoint}/${payload.id}`, payload)
            return response.data
        } catch (error) {
            if(error.response.data.code == 401){
                return Swal.fire({
                title: 'Opss!',
                text: error.response.data.errorMessage.error,
                icon: 'danger',
                showCancelButton: false,
                confirmButtonText: 'Login Aplikasi',
                cancelButtonText: 'Batal',
                confirmButtonColor: 'red',
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('access_token')
                        window.location.href = 'Login'
                    }
                });
            }
            Swal.fire({
                title: 'Error!',
                text: error.response.data.errorMessage.Email || error.response.data.errorMessage.Password,
                icon: 'error',
                confirmButtonText: 'OK'
            })
            return error.response.data
        }
    },
    async putData(endpoint, payload) {
        try {
            const response = await axios.put(`/${endpoint}/${payload.id}`, payload)
            return response.data
        } catch (error) {
            if(error.response.data.code == 401){
                return Swal.fire({
                title: 'Opss!',
                text: error.response.data.errorMessage.error,
                icon: 'danger',
                showCancelButton: false,
                confirmButtonText: 'Login Aplikasi',
                cancelButtonText: 'Batal',
                confirmButtonColor: 'red',
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('access_token')
                        window.location.href = 'Login'
                    }
                });
            }
            Swal.fire({
                title: 'Error!',
                text: error.response.data.errorMessage.Email || error.response.data.errorMessage.Password,
                icon: 'error',
                confirmButtonText: 'OK'
            })
            return error.response.data
        }
    },
    async deleteData(endpoint, id) {
        try {
            const response = await axios.delete(`/${endpoint}/${id}`)
            return response.data
        } catch (error) {
            if(error.response.data.code == 401){
                return Swal.fire({
                title: 'Opss!',
                text: error.response.data.errorMessage.error,
                icon: 'danger',
                showCancelButton: false,
                confirmButtonText: 'Login Aplikasi',
                cancelButtonText: 'Batal',
                confirmButtonColor: 'red',
                }).then((result) => {
                    if (result.isConfirmed) {
                        localStorage.removeItem('access_token')
                        window.location.href = 'Login'
                    }
                });
            }
            Swal.fire({
                title: 'Error!',
                text: error.response.data.errorMessage.Email || error.response.data.errorMessage.Password,
                icon: 'error',
                confirmButtonText: 'OK'
            })
            return error.response.data
        }
    }
}
