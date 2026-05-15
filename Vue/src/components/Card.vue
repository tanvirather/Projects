<script setup>
/************************************************** Props **************************************************/
const props = defineProps({
  title: { type: String, default: "" },
  saveText: { type: String, default: 'Save' },
  cancelText: { type: String, default: 'Cancel' }
});

/************************************************** Emits **************************************************/
defineEmits(["onSave", "onCancel"]);

/************************************************** Computed **************************************************/

/************************************************** functions **************************************************/
</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="card">
    <header>{{ title }}</header>
    <main class="card-content">
      <slot />
    </main>
    <footer class="card-actions">
      <slot name="footer">
        <Button :label="saveText" v-if="saveText.length > 0" @click="$emit('onSave')" />
        <Button :label="cancelText" v-if="cancelText.length > 0" @click="$emit('onCancel')" />
      </slot>
    </footer>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
.card {
  background-color: var(--background-color);
  box-shadow: 0 0px 4px 2px rgba(0, 0, 0, 0.2);
  margin: var(--margin);
  border-radius: var(--radius);
  display: flex;
  flex-direction: column;

  header, main, footer {
    padding: calc(var(--margin) * 2) calc(var(--margin) * 2);
  }

  header {
    border-radius: var(--radius) var(--radius) 0 0;
    background-color: var(--primary-color);
    color: var(--primary-color-text);
    font-weight: bold;
    font-size: larger;
  }

  main {
    flex: 1;
    overflow-y: auto;
    background-color: var(--background-color);
  }

  footer {
    border-radius: 0 0 var(--radius) var(--radius);
    text-align: right;
    > * {
      margin: 0 0 0 var(--margin); 
    }
  }
}
</style>
