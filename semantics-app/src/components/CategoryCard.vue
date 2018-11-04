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
          <div id="add-words-button" v-on:click="addWords" class="input-group-append">
            <span class="input-group-text">Add</span>
          </div>
        </div>
      </div>
      
    </form>
    <ul id="words-list" class="list-group">
      <li class="list-group-item"
        v-for="(word,index) in Object.keys(category.Words)"
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
      editMode: false
    }
  },
  mounted: function() {
    this.$broadcaster.on('categorySelected', category => {
      if (category){
        this.$categorySvc
          .getCategoryById(this.$http,category.Id)
          .then((category) => this.category = category);
      }
    });
    this.$broadcaster.on('goToCategoryCard', () => {
      this.editMode = false;
    });
  },
  methods: {
    addWords: function() {
      let words = [];
      
      this.newWords.split(',').forEach(word => {
        words.push({ 'Word': word });
      })

      this.$categorySvc.addWordsToCategory(this.$http,this.category.Id,words)
      .then((category) => {
        this.category = category;
        this.newWords = '';
      });
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

#add-words-button {
  cursor: pointer;
}

#edit-category-button {
  margin-top: 20px;
}
</style>