<template>
<div id="category-area">
  <form>
    <div class="form-group">
      <label for="find-category">Find Category</label>
      <div class="input-group mb-3">
        <input v-model="filterTerm" v-on:keyup="filterCategories" type="text" id="find-category" class="form-control" placeholder="Filter">
        <div class="input-group-append">
          <span v-on:click="filterTerm = ''; filterCategories();" class="input-group-text"><i class="fas fa-times" /></span>
        </div>
      </div>
    </div>
  </form>

  <ul class="list-group">
    <li class="list-group-item list-group-item-action d-flex justify-content-between align-items-center"
      v-for="(category, index) in filteredCategories"
      v-bind:key="category.Id"
      v-bind:class="activeIndex === index ? 'active' : ''"
      v-on:click="activeIndex = index">
      {{category.Name}}
      <span class="badge badge-pill">
        <i class="far fa-edit"></i>
      </span>
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
  },
  methods: {
    filterCategories: function() {
      if (this.filterTerm) {
        this.filteredCategories = this.categories.filter(c => c.Name.toLowerCase().includes(this.filterTerm.toLowerCase()));
      } else {
        this.filteredCategories = [];
      }
    }
  }
}
</script>

<style>
#category-area {
  border: solid 0.5px #e2e1e0;
  border-radius: 2px;
  min-height: 200px;
  padding: 20px;
}
</style>