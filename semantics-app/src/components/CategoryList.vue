<template>
<div>
  <form>
    <div class="form-group">
      <label for="find-category">Find Category</label>
      <div class="input-group mb-3">
        <input v-model="filterTerm" v-on:keyup="filterCategories" type="text" id="find-category" class="form-control" placeholder="Filter">
        <div class="input-group-append">
          <button v-on:click="clearFiler" class="btn btn-outline-secondary"><i class="fas fa-times" /></button>
        </div>
      </div>
    </div>
  </form>

  <ul id="category-list" class="list-group">
    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center"
      v-for="(category, index) in filteredCategories"
      v-bind:key="category.Id"
      v-bind:class="activeIndex === index ? 'active' : ''"
      v-on:click="activeIndex = index; selectCategory(category);">
      {{category.Name}}
    </li>
  </ul>
</div>
</template>

<script>
export default {
  data: function() {
    return {
      filterTerm: '',
      categories: [],
      filteredCategories: [],
      activeIndex: 0
    }
  },
  mounted: function() {
    this.$categorySvc.getAllCategories(this.$http)
    .then((categories) => {
      this.categories = categories;
    });

    this.$broadcaster.on('categoryNameUpdated', () => {
      this.$categorySvc.getAllCategories(this.$http)
      .then((categories) => {
        this.categories = categories;
        this.filteredCategories = [];
        this.filterTerm = '';
      });
    });
  },
  methods: {
    selectCategory: function(category) {
      this.$broadcaster.emit('categorySelected', category);
    },
    filterCategories: function() {
      if (this.filterTerm) {
        this.filteredCategories = this.categories.filter(c => c.Name.toLowerCase().includes(this.filterTerm.toLowerCase()));
      } else {
        this.filteredCategories = [];
      }
    },
    clearFiler: function() {
      event.preventDefault();
      this.filterTerm = '';
      this.filteredCategories = [];
    }
  }
}
</script>

<style>
#category-list {
  max-height: 200px;
  overflow: auto;
}
</style>