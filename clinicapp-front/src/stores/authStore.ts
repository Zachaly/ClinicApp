import type { LoginResponse } from "@/model/Response";
import type UserModel from "@/model/user/UserModel";
import axios, { AxiosError } from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";

export const claimNames = {
    admin: 'admin',
    doctor: 'doctor',
    receptionist: 'receptionist'
}

export const useAuthStore = defineStore("auth", () => {
  const userData = ref<UserModel | null>(null);

  const isAuthorized = computed(() => userData.value);

  const authorize = (login: string, password: string) =>
    axios
      .post<LoginResponse>("/authorization/login", { login, password })
      .then(async (response) => {
        axios.defaults.headers['Authorization'] = "Bearer " + response.data.authToken
        const dataResponse = await axios.get<UserModel>(
          "user/" + response.data.userId,
        );

        userData.value = dataResponse.data;
        return true
      }).catch((error: AxiosError<LoginResponse>) => {
        alert(error.response?.data.error)
        return false
      });

  return { userData, isAuthorized, authorize };
});
