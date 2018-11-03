<template>
  <div id="app">
    <div class="container-fluid">

      <div class="jumbotron jumbotron-fluid">
        <div class="container">
          <h1 class="display-4">Semantics</h1>
          <p class="lead">Categorize and analyze words</p>
        </div>
      </div>

      <div class="row">
        <div class="col-md-6">
          <category-area />
        </div>
        <div class="col-md-6">
        </div>
      </div>

    </div>
  </div>
</template>

<script>
import CategoryArea from './components/CategoryArea.vue';

export default {
  name: 'app',
  components: {
    CategoryArea
  }
}
</script>

<style>
  #app {
    margin: 20px;
  }
</style>
