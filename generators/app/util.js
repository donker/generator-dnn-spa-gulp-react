var fs = require('fs');

module.exports = {

  recurseRead: function(basePath, path) {
    var that = this;
    var foundPaths = [];
    var files = fs.readdirSync(basePath + '/' + path);
    files.forEach(function(el, i, arr) {
      var newPath = basePath + '/' + path + '/' + el;
      var st = fs.statSync(newPath);
      if (st.isDirectory()) {
        if (path === '') {
          foundPaths = foundPaths.concat(that.recurseRead(basePath, el));
        } else {
          foundPaths = foundPaths.concat(that.recurseRead(basePath, path + '/' + el));
        }
      } else {
        if (path === '') {
          foundPaths.push(el);
        } else {
          foundPaths.push(path + '/' + el);
        }
      }
    });
    return foundPaths;
  }

}
