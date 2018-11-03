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
  }
}