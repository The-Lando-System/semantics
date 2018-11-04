<template>
<div>
  <h4>Editing Category [{{category.Name}}]</h4>
  <div id="edit-category-name" class="input-group mb-3">
    <input v-model="category.Name" type="text" class="form-control" >
    <div class="input-group-append">
      <button v-on:click="editCategoryName" class="btn btn-outline-secondary">Update</button>
    </div>
  </div>
  <div id="word-filter" class="input-group mb-3">
    <input v-model="filterTerm" v-on:keyup="filterWords" type="text" class="form-control" placeholder="Filter">
    <div class="input-group-append">
      <button v-on:click="clearFilter" class="btn btn-outline-secondary"><i class="fas fa-times" /></button>
    </div>
  </div>
  <div id="remove-words-list">
    <ul class="list-group">
      <li class="list-group-item d-flex justify-content-between align-items-center"
        v-for="(word,index) in filteredWords"
        v-bind:key="index">
        {{word}}
        <span id="remove-word-button" v-on:click="deleteWord(word)" class="badge badge-pill">
          <i class="fas fa-times"></i>
        </span>
      </li>
    </ul>
  </div>
  <button v-on:click="goBack" class="btn btn-outline-secondary">Back</button>
</div>
</template>

<script>
export default {
  props:[
    'category'
  ],
  data: function() {
    return {
      filterTerm: '',
      filteredWords: []
    }
  },
  mounted: function() {
    this.filteredWords = Object.keys(this.category.Words);
  },
  methods: {
    goBack: function() {
      this.$broadcaster.emit('goToCategoryCard', {});
    },
    filterWords: function() {
      if (this.filterTerm) {
        this.filteredWords = Object.keys(this.category.Words).filter(w => w.toLowerCase().includes(this.filterTerm.toLowerCase()));
      } else {
        this.filteredWords = Object.keys(this.category.Words);
      }
    },
    editCategoryName: function() {
      event.preventDefault();
      this.$categorySvc.updateCategoryName(this.$http, this.category)
      .then((response) => {
        this.$broadcaster.emit('categoryNameUpdated', {});
      });
    },
    clearFilter: function() {
      event.preventDefault();
      this.filterTerm = '';
      this.filteredWords = Object.keys(this.category.Words);
    },
    deleteWord: function(word) {
      if (!confirm(`Are you sure you want to remove word [${word}] from category [${this.category.Name}]?`)) {
        return;
      }

      let words = [{'Word' : word}];

      this.$categorySvc.removeWordsFromCategory(this.$http,this.category.Id,words)
      .then((response) => {
        this.clearFilter();
        this.$broadcaster.emit('wordRemoved', {});
      });

    }
  }
}
</script>

<style>
#edit-category-name {
  margin-bottom: 20px;
}

#remove-words-list {
  margin-bottom: 20px;
  max-height: 200px;
  overflow: auto;
}

#remove-word-button {
  cursor: pointer;
}
</style>