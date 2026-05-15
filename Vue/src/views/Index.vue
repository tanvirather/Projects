<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { storage } from '../store';
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <nav>
    <router-link to="/dashboard">Dashboard</router-link>
    <router-link :to="`/account/account/${storage.userId}`">Account</router-link>
    <router-link to="/numericType">Numeric Type</router-link>
  </nav>
  <router-view />
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped></style>
