<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { accountStore, storage } from '../../store';

const router = useRouter();

/************************* props *************************/
const model = ref({
  email: "test@example.com",
  password: "P@ssw0rd",
  twoFactorCode: "",
  userId: ""
});
const requiresTwoFactor = ref(false);

/************************* computed *************************/
/************************* functions *************************/
async function login() {
  if (requiresTwoFactor.value) {
    const success = await accountStore.loginTwoFactor(model.value.userId, model.value.twoFactorCode);
    if (success) {
      router.push(`/account/account/${storage.userId}`);
    }
    return;
  }

  const response = await accountStore.login(model.value);
  if (response.success) {
    router.push(`/account/account/${storage.userId}`);
  } else if (response.requiresTwoFactor) {
    requiresTwoFactor.value = true;
    model.value.userId = response.userId;
  }
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Card id="login" title="Login" saveText="Login" cancelText="" @onSave="login">
    <div v-if="!requiresTwoFactor">
      <Text label="Email" v-model="model.email" />
      <Password label="Password" v-model="model.password" />
    </div>
    <div v-else>
      <Text label="Authenticator Code" v-model="model.twoFactorCode" />
    </div>
    <div class="register-link">
      Don't have an account? <router-link to="/account/register">Register</router-link>
    </div>
  </Card>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
#login {
  width: 400px;
}

.register-link {
  margin-top: 1rem;
  text-align: center;
  font-size: 0.9rem;
}

.register-link a {
  color: var(--primary-color);
  text-decoration: none;
  font-weight: bold;
}

.register-link a:hover {
  text-decoration: underline;
}
</style>
