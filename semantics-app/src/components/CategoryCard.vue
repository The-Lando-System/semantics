<template>
<div id="category-card">
  <div v-if="category.Id && !editMode">
    <div>
      <h4 id="category-name">{{category.Name}}</h4>
      <span id="word-count">{{Object.keys(category.Words).length}} words</span>
    </div>
    <form>
      <div class="form-group">
        <div class="input-group mb-3">
          <input v-model="newWords" type="text" id="add-words" class="form-control" placeholder="add words">
          <div class="input-group-append">
            <button v-on:click="addWords" class="btn btn-outline-secondary">Add</button>
          </div>
        </div>
      </div>
    </form>

    <div id="word-filter" class="input-group mb-3">
      <input v-model="filterTerm" v-on:keyup="filterWords" type="text" class="form-control" placeholder="Filter">
      <div class="input-group-append">
        <button v-on:click="clearFilter" class="btn btn-outline-secondary"><i class="fas fa-times" /></button>
      </div>
    </div>

    <ul id="words-list" class="list-group">
      <li class="list-group-item"
        v-for="(word,index) in filteredWords"
        v-bind:key="index">
        {{word}}
      </li>
    </ul>
    <button id="edit-category-button" v-on:click="editMode = true;" class="btn btn-info">Edit</button>
  </div>
  <div v-else-if="category.Id && editMode">
    <category-editor v-bind:category="category" />
  </div>
  <div v-else>
    <h4>Select a category</h4>
  </div>
</div>
</template>

<script>
import CategoryEditor from './CategoryEditor.vue';

export default {
  components: {
    CategoryEditor
  },
  data: function() {
    return {
      category: {
        'Name': 'N/A',
        'Id': '',
        'Words': {}
      },
      newWords: '',
      editMode: false,
      filterTerm: '',
      filteredWords: []
    }
  },
  mounted: function() {
    this.$broadcaster.on('categorySelected', category => {
      if (category){
        this.$categorySvc.getCategoryById(this.$http,category.Id)
        .then((category) => {
          this.category = category;
          this.filteredWords = Object.keys(this.category.Words);
          this.clearFilter();
        });
      }
    });
    this.$broadcaster.on('goToCategoryCard', () => {
      this.editMode = false;
      this.clearFilter();
    });
    this.$broadcaster.on('wordRemoved', () => {
      this.$categorySvc.getCategoryById(this.$http,this.category.Id)
      .then((category) => {
        this.category = category;
        this.filteredWords = Object.keys(this.category.Words);
        this.editMode = false;
        this.clearFilter();
      });
    });
  },
  methods: {
    addWords: function() {
      event.preventDefault();
      let words = [];
      
      this.newWords.split(',').forEach(word => {
        words.push({ 'Word': word.trim() });
      })

      this.$categorySvc.addWordsToCategory(this.$http,this.category.Id,words)
      .then((category) => {
        this.category = category;
        this.newWords = '';
        this.filteredWords = Object.keys(this.category.Words);
        this.clearFilter();
      });
    },
    filterWords: function() {
      if (this.filterTerm) {
        this.filteredWords = Object.keys(this.category.Words).filter(w => w.toLowerCase().includes(this.filterTerm.toLowerCase()));
      } else {
        this.filteredWords = Object.keys(this.category.Words);
      }
    },
    clearFilter: function() {
      event.preventDefault();
      this.filterTerm = '';
      this.filteredWords = Object.keys(this.category.Words);
    }
  }
}
</script>

<style>
#category-card {
  border: solid 0.5px #e2e1e0;
  border-radius: 2px;
  padding: 20px;
}

#category-name {
  display: inline-block;
}

#word-count {
  float: right;
}

#words-list {
  max-height: 300px;
  overflow: auto;
}

#edit-category-button {
  margin-top: 20px;
}

#word-filter {
  margin-top: 40px;
}
</style>