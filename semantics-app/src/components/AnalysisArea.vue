<template>
<div id="analysis-area">
  <div v-if="category.Id">
    <h4>Analyzing with category {{category.Name}}</h4>
    <textarea v-model="textInput" class="form-control" id="text-input" rows="5" />
    <button v-on:click="analyze" class="btn btn-primary">Analyze</button>
    <div class="btn-group btn-group-toggle" data-toggle="buttons" style="float:right">
      <label class="btn btn-outline-info" v-bind:class="parseWordsMode ? 'active' : ''">
        <input v-on:click="parseWordsMode = true" type="radio" name="options" id="word-mode-toggle" autocomplete="off">Parse Words
      </label>
      <label class="btn btn-outline-info" v-bind:class="parseWordsMode ? '' : 'active'">
        <input v-on:click="parseWordsMode = false" type="radio" name="options" id="phrase-mode-toggle" autocomplete="off">Parse Phrases
      </label>
    </div>
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
    <div v-if="parsedPhrases.length > 0">
      <hr/>
      Select a phrase<br/>
      <div id="phrase-buttons" class="btn-group btn-group-toggle" data-toggle="buttons">
        <label v-for="(phraseWord,index) in parsedPhrases" v-bind:key="index"
          class="btn btn-outline-secondary phrase-word" v-bind:class="phraseWord.isSelected ? 'active' : ''">
          <input v-on:click="phraseWord.isSelected = !phraseWord.isSelected" type="radio" name="options" autocomplete="off">{{phraseWord.word}}
        </label>
      </div>
      <button v-on:click="addPhrase" class="btn btn-primary">Add Phrases</button>
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
      parsedWords: [],
      parseWordsMode: true,
      parsedPhrases: []
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

          if (this.parseWordsMode) {

            this.parsedPhrases = [];

            this.parsedWords = words.map(w => this.removePunctuation(w.Word.trim())).filter(w => !matchedWords.includes(w));
          } else {

            this.parsedWords = [];
            this.parsedPhrases = [];

            words.forEach((word) => {
              this.parsedPhrases.push({
                'word': word.Word,
                'isSelected': false
              });
            });

          }
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
    addPhrase: function() {
      let phrases = [];
      let currentPhrase = '';
      let endOfPhrase = false;
      for (let i=0; i<this.parsedPhrases.length; i++) {
        
        if (this.parsedPhrases[i].isSelected) {
          currentPhrase += this.parsedPhrases[i].word + ' ';
        }

        if (i !== this.parsedPhrases.length - 1) {
          endOfPhrase = !this.parsedPhrases[i+1].isSelected;
        } else {
          endOfPhrase = true;
        }

        if (endOfPhrase && currentPhrase !== '') {
          phrases.push(currentPhrase.trim());
          currentPhrase = '';
          endOfPhrase = false;
        }

      }

      console.log(phrases);
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

.phrase-word {
  border-radius: 10px;
  margin-top: 10px;
  padding-top: 0px;
  padding-bottom: 2px;
}

#phrase-buttons {
  margin-top: 10px;
  margin-bottom: 20px;
  display: block;
}
</style>