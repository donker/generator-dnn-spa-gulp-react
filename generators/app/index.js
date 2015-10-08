'use strict';
var yeoman = require('yeoman-generator'),
  chalk = require('chalk'),
  yosay = require('yosay'),
  path = require('path'),
  extend = require('extend'),
  util = require('./util.js');

module.exports = yeoman.generators.Base.extend({

  _configPath: '',
  _config: {},
  _defaults: {
    dnn: {
      organization: 'Connect',
      email: 'webmaster@dnn-connect.org',
      name: ''
    }
  },

  initializing: function() {

    if (path.basename(this.destinationPath()).toLowerCase() !== 'desktopmodules') {
      this.log('You must run this in the ' + chalk.red('DesktopModules') + ' folder!!');
      process.exit();
    }

    this._configPath = this.sourceRoot() + '/../../../.yo-rc.json';
    this._config = extend(true, this._defaults, this.fs.readJSON(path.normalize(this._configPath)));
    console.log(this._config);

  },

  prompting: function() {
    var done = this.async();

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
      default: this._config.dnn.organization,
      validate: function(value) {
        if (value !== '') {
          return true;
        } else {
          return "You must enter a name for the organization";
        }
      }
    }, {
      type: 'input',
      name: 'name',
      message: 'Your name',
      default: this._config.dnn.name,
      validate: function(value) {
        if (value !== '') {
          return true;
        } else {
          return "You must enter your name";
        }
      }
    }, {
      type: 'input',
      name: 'email',
      message: 'Email address to use for project',
      default: this._config.dnn.email
    }, {
      type: 'confirm',
      name: 'overwriteSettings',
      message: 'Overwrite previous settings for organization, name and email?',
      default: false
    }];

    this.prompt(prompts, function(props) {
      this.props = props;
      if (this.props.overwriteSettings) {
        this._config.dnn.organization = this.props.organization;
        this._config.dnn.name = this.props.name;
        this._config.dnn.email = this.props.email;
        this.fs.writeJSON(this._configPath, this._config);
      }
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
