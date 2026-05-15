<script setup>
import { toasterStore } from '../lib/toasterStore';
</script>

<template>
  <div class="toaster">
    <TransitionGroup name="toast">
      <div v-for="toast in toasterStore.toasts" :key="toast.id" :class="['toast', toast.type]">
        <span class="message">{{ toast.message }}</span>
        <button class="close-btn" @click="toasterStore.dismiss(toast.id)">&times;</button>
      </div>
    </TransitionGroup>
  </div>
</template>

<style scoped>
.toaster {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 1000;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.toast {
  padding: 12px 20px;
  border-radius: var(--radius, 4px);
  color: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
  min-width: 200px;
  max-width: 400px;
  word-break: break-word;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 10px;
}

.message {
  flex: 1;
  white-space: pre-line;
}

.close-btn {
  background: none;
  border: none;
  color: white;
  font-size: 20px;
  line-height: 1;
  cursor: pointer;
  padding: 0;
  opacity: 0.7;
}

.close-btn:hover {
  opacity: 1;
}

.error {
  background-color: #ff4d4f;
}

.success {
  background-color: #52c41a;
}

.info {
  background-color: #1890ff;
}

.warning {
  background-color: #d48806;
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  opacity: 0;
  transform: translateX(30px);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(30px);
}
</style>
