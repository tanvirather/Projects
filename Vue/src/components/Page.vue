<script setup>
/************************************************** Props **************************************************/
const props = defineProps({
  title: { type: String, default: "" },
  saveText: { type: String, default: 'Save' },
});

/************************************************** Emits **************************************************/
defineEmits(["onSubmit"]);

/************************************************** Computed **************************************************/

/************************************************** functions **************************************************/
const displayHeader = () => props.title.length > 0;
const displayFooter = () => props.saveText.length > 0;

</script>

<!-------------------------------------------------- template -------------------------------------------------->
<template>
  <div class="page">
      <header v-if="displayHeader()">{{ title }}</header>
      <main>
        <slot />
      </main>
      <footer v-if="displayFooter()">
        <slot name="footer">
          <button id="save-btn" v-if="saveText.length > 0" @click="$emit('onSubmit')">{{ saveText }}</button>
        </slot>
      </footer>
  </div>
</template>

<!-------------------------------------------------- style -------------------------------------------------->
<style scoped>
.page {
  display: flex;
  flex-direction: column;
  height: calc(100% - var(--margin) * 2);
  background-color: var(--surface-color);
  padding: var(--margin);

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
    box-shadow: 0 4px 8px 2px rgba(0, 0, 0, 0.2);
  }

  footer {
    border-top: 1px solid var(--surface-color);
    background-color: var(--primary-color);
    border-radius: 0 0 var(--radius) var(--radius);
  }
}
</style>
