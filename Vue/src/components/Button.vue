<script setup>
import { computed } from "vue";

/************************************************** Props **************************************************/
const props = defineProps({
  id: { type: String, default: "" },
  label: { type: String, default: "" },
  disabled: { type: Boolean, default: false },
});

/************************************************** Emits **************************************************/
defineEmits(["click"]);

/************************************************** Computed **************************************************/
const computedId = computed(() => props.id || props.label.toLowerCase().replace(/\s+/g, "_"));

/************************************************** functions **************************************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <button :id="computedId" :disabled="disabled" @click="$emit('click', $event)">{{ label }}</button>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
button {
  background: var(--primary-color);
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: var(--radius);
  cursor: pointer;
}

button:hover {
  cursor: pointer;
  opacity: 0.8;
}
</style>
