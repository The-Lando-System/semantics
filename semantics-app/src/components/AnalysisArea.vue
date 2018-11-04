<template>
<div id="analysis-area">
  <div v-if="category.Id">
    <h4>Analyzing with category [{{category.Name}}]</h4>
    <textarea v-model="textInput" class="form-control" id="text-input" rows="5" placeholder="insert text"/>
    <button v-on:click="analyze" class="btn btn-primary">Analyze</button>
    <hr/>
    <div v-if="results">
      {{results}}
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
      results: ''
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
      });
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
}
</style>