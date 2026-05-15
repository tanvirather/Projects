<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { accountStore, storage } from '../../store';

const router = useRouter();

/************************* props *************************/
const model = ref({
  email: "test@example.com",
  password: "P@ssw0rd"
});
/************************* computed *************************/
/************************* functions *************************/
async function login() {
  const response = await accountStore.login(model.value);
  if (response) {
    router.push(`/account/account/${storage.userId}`);
  }
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Card id="login" title="Login" saveText="Login" cancelText="" @onSave="login">
    <Text label="Email" v-model="model.email" />
    <Password label="Password" v-model="model.password" />
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
