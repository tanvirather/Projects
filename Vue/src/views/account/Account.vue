<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import { ref, onMounted } from 'vue';
import { accountStore } from '../../store';

/************************* props *************************/
const props = defineProps({
  id: String
});

const model = ref({
  firstName: "",
  lastName: "",
  phoneNumber: ""
});

/************************* computed *************************/
/************************* functions *************************/
onMounted(async () => {
  const data = await accountStore.get(props.id);
  if (data) {
    model.value = data;
  }
});

async function update() {
  await accountStore.update(model.value);
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Card id="account" title="Account Settings" saveText="Update" cancelText="" @onSave="update">
    <Text label="First Name" v-model="model.firstName" />
    <Text label="Last Name" v-model="model.lastName" />
    <Text label="Phone" v-model="model.phoneNumber" />
  </Card>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
#account {
  width: 400px;
}
</style>
