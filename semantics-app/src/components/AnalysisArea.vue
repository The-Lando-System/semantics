<template>
<div id="analysis-area">
  <div v-if="category.Id">
    <h4>Analyzing with category {{category.Name}}</h4>
    <textarea v-model="textInput" class="form-control" id="text-input" rows="5" />
    <button v-on:click="analyze" class="btn btn-primary">Analyze</button>
    <div v-if="results">
      <hr/>
      {{results}}
    </div>
    <div v-if="parsedWords.length > 0">
      <hr/>
      Non-matched words:
      <ul id="parsed-words-list" class="list-group">
        <li class="list-group-item d-flex justify-content-between align-items-center"
          v-for="(word,index) in parsedWords"
          v-bind:key="index">
          {{word}}
          <span id="add-parsed-word-button" v-on:click="addWord(word)" class="badge badge-pill">
            <i class="fas fa-plus-square"></i> Add to Category
          </span>
        </li>
      </ul>
    </div>
  </div>
  <div v-else>
    <h4>Select a category</h4>
  </div>
</div>
</template>

<script>

export default {
  components: {
  },
  data: function() {
    return {
      textInput: '',
      category: {},
      results: '',
      parsedWords: []
    }
  },
  mounted: function() {
    this.$broadcaster.on('categorySelected', (categoryId) => {
      if (categoryId){
        this.$categorySvc.getCategoryById(this.$http,categoryId)
        .then((category) => {
          this.results = '';
          this.category = category;
        });
      }
    });
  },
  methods: {
    analyze: function() {
      event.preventDefault();

      let words = [];

      this.textInput.split(' ').forEach(word => {
        words.push({'Word':word});
      });

      this.$categorySvc.containsWords(this.$http,this.category.Id,words)
      .then((matchedWords) => {
        let wordPercent = Math.round((matchedWords.length / words.length) * 100);
        this.results = `Input matches ${wordPercent}% in ${this.category.Name}`;
        this.parsedWords = words.map(w => this.removePunctuation(w.Word.trim())).filter(w => !matchedWords.includes(w));
      });
    },
    addWord: function(word) {
      event.preventDefault();
      let words = [{'Word':word}];
      
      this.$categorySvc.addWordsToCategory(this.$http,this.category.Id,words)
      .then(() => {
        for (let i=0; i<this.parsedWords.length; i++) {
          if (this.parsedWords[i] === word) {
            this.parsedWords.splice(i,1);
          }
        }
        this.$broadcaster.emit('addedWord', {});
      });
    },
    removePunctuation: function(word) {
      return word
        .replace('.','')
        .replace(',','')
        .replace('!','');
    }
  }
}
</script>

<style>
#analysis-area {
  border: solid 0.5px #e2e1e0;
  border-radius: 2px;
  padding: 20px;
}

#text-input {
  margin-bottom: 20px;
  margin-top: 20px;
}

#add-parsed-word-button {
  cursor: pointer;
}
</style>