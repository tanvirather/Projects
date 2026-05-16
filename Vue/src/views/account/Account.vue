<!-------------------------------------------------- script -------------------------------------------------->
<script setup>
import QRCode from 'qrcode';
import { onMounted, ref } from 'vue';
import { accountStore } from '../../store';

/************************* props *************************/
const props = defineProps({
  id: String
});

const model = ref({
  firstName: "",
  lastName: "",
  phoneNumber: "",
  phoneNumberConfirmed: false
});

const token = ref("");
const showTokenInput = ref(false);

const twoFactorInfo = ref(null);
const showTwoFactorSetup = ref(false);
const twoFactorCode = ref("");
const qrCodeUrl = ref("");

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

async function verifyPhone() {
  await accountStore.verifyPhoneNumber(props.id);
  showTokenInput.value = true;
}

async function confirmPhone() {
  const success = await accountStore.confirmPhoneNumber(props.id, token.value);
  if (success) {
    model.value.phoneNumberConfirmed = true;
    showTokenInput.value = false;
  }
}

async function setupTwoFactor() {
  const data = await accountStore.getEnableTwoFactor(props.id);
  if (data) {
    twoFactorInfo.value = data;
    showTwoFactorSetup.value = true;
    if (data.authenticatorUri) {
      qrCodeUrl.value = await QRCode.toDataURL(data.authenticatorUri);
    }
  }
}

async function enableTwoFactor() {
  const success = await accountStore.enableTwoFactor(props.id, twoFactorCode.value);
  if (success) {
    showTwoFactorSetup.value = false;
    model.value.twoFactorEnabled = true;
  }
}
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <Card id="account" title="Account Settings" saveText="Update" cancelText="" @onSave="update">
    <Text label="First Name" v-model="model.firstName" />
    <Text label="Last Name" v-model="model.lastName" />
    <div class="phone-container">
      <Text label="Phone" v-model="model.phoneNumber" />
      <Button v-if="!model.phoneNumberConfirmed && !showTokenInput" label="Verify" @click="verifyPhone" />
    </div>
    <div v-if="showTokenInput" class="phone-container">
      <Text label="Token" v-model="token" />
      <Button label="Confirm" @click="confirmPhone" />
    </div>
    <hr />
    <div v-if="!model.twoFactorEnabled && !showTwoFactorSetup">
      <Button label="Enable 2FA" @click="setupTwoFactor" />
    </div>
    <div v-if="showTwoFactorSetup">
      <p>Scan this QR code or enter the key in your Microsoft Authenticator app:</p>
      <div v-if="qrCodeUrl" class="qr-code">
        <img :src="qrCodeUrl" alt="QR Code" />
      </div>
      <p><b>Key:</b> {{ twoFactorInfo.sharedKey }}</p>
      <Text label="Verification Code" v-model="twoFactorCode" />
      <Button label="Verify & Enable" @click="enableTwoFactor" />
    </div>
    <div v-if="model.twoFactorEnabled">
      <p>Two-factor authentication is enabled.</p>
    </div>
  </Card>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
#account {
  width: 400px;
}

.phone-container {
  display: flex;
  align-items: flex-end;
  gap: 10px;
}

.phone-container> :first-child {
  flex: 1;
}

.phone-container> :last-child {
  margin-bottom: 10px;
}

.qr-code {
  display: flex;
  justify-content: center;
  margin: 1rem 0;
}

.qr-code img {
  max-width: 200px;
}
</style>
