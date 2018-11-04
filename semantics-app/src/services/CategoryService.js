export default {
  requestUrl : 'http://localhost:50588/categories',
  
  getAllCategories: function(http) {
    return new Promise((resolve,reject) => {
      http.get(this.requestUrl)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    });
  },
  getCategoryById: function(http, id) {
    return new Promise((resolve,reject) => {
      http.get(`${this.requestUrl}/${id}`)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    });
  },
  addWordsToCategory: function(http, id, words) {
    return new Promise((resolve,reject) => {
      http.post(`${this.requestUrl}/${id}/add-words`, words)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    })
  }
}