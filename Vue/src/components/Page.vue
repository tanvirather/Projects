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
  height: 100%;

  /* * {
    border: 1px solid red;
  } */

  header {
    height: 25px;
  }

  main {
    flex: 1;
    overflow-y: auto;
  }

  footer {
    height: 25px;
  }
}
</style>
