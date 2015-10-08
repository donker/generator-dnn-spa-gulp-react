'use strict';
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var util = require('./util.js');

module.exports = yeoman.generators.Base.extend({
  prompting: function() {
    var done = this.async();

    // Have Yeoman greet the user.
    this.log(yosay(
      'Welcome to the ' + chalk.red('DNN SPA Module using Gulp and React') + ' generator!'
    ));

    var prompts = [{
      type: 'input',
      name: 'projectName',
      message: 'Project Name',
      validate: function(value) {
        if (value !== '') {
          return true;
        } else {
          return "You must enter a name for the project";
        }
      }
    }, {
      type: 'input',
      name: 'organization',
      message: 'Organization name (also used as subfolder and in namespaces)',
      default: 'Dnn',
      validate: function(value) {
        if (value !== '') {
          return true;
        } else {
          return "You must enter a name for the organization";
        }
      }
    }];

    this.prompt(prompts, function(props) {
      this.props = props;
      // To access props later use this.props.someOption;
      this.sourceRoot(this.sourceRoot() + '/../../../template');

      done();
    }.bind(this));
  },

  writing: {
    app: function() {},

    projectfiles: function() {

      var files = util.getFilesRecursive(this.templatePath(), '');
      for (var i = files.length - 1; i >= 0; i--) {
        var dest = files[i].replace('Project', this.props.projectName)
                           .replace('_package', 'package')
                           .replace('Company', this.props.organization);
        this.fs.copyTpl(
          this.templatePath(files[i]),
          this.destinationPath(dest), {
            props: this.props
          }
        );
      };

    }

  },

  copyFiles: function(fileList) {
    if (fileList === undefined) {
      return;
    }
    console.log(typeof fileList);
    for (var i = fileList.length - 1; i >= 0; i--) {
      this.fs.copyTpl(
        this.templatePath(fileList[i][0]),
        this.destinationPath(fileList[i][1]), {
          props: this.props
        }
      );
    };
  },

  install: function() {
    // this.installDependencies();
  }
});
