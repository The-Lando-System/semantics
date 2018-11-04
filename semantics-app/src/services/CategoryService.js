export default {
  categoryUrl : 'http://localhost:50588/categories',
  
  getAllCategories: function(http) {
    return new Promise((resolve,reject) => {
      http.get(this.categoryUrl)
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
      http.get(`${this.categoryUrl}/${id}`)
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
      http.post(`${this.categoryUrl}/${id}/add-words`, words)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    });
  },
  removeWordsFromCategory: function(http, id, words) {
    return new Promise((resolve,reject) => {
      http.post(`${this.categoryUrl}/${id}/remove-words`, words)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    });
  },
  updateCategoryName: function(http, updatedCategory) {
    return new Promise((resolve,reject) => {
      this.getCategoryById(http, updatedCategory.Id)
      .then((category) => {

        category.Name = updatedCategory.Name;

        http.put(`${this.categoryUrl}/${category.Id}`, category)
        .then((response) => {
          resolve(response);
        })
        .catch((error) => {
          reject(error);
        });

      })
      .catch((error) => {
        reject(error);
      });

    });
  },
  containsWords: function(http, id, words) {
    return new Promise((resolve,reject) => {
      http.post(`${this.categoryUrl}/${id}/contains-words`, words)
      .then((response) => {
        resolve(response.data);
      })
      .catch((error) => {
        reject(error);
      });
    });
  }
}