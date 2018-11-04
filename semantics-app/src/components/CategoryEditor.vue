<template>
<div>
  <h4>Editing Category [{{category.Name}}]</h4>
  <button v-on:click="goBack" class="btn btn-outline-secondary">Back</button>
</div>
</template>

<script>
export default {
  props:[
    'category'
  ],
  methods: {
    goBack: function() {
      this.$broadcaster.emit('goToCategoryCard', {});
    }
  }
}
</script>

<style>
</style>