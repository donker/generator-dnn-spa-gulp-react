# DNN SPA Module generator with Gulp and React

> A [Yeoman](http://yeoman.io) generator

This generator scaffolds out a module leveraging DNN's SPA module pattern. Included are references to Gulp and React so if you're using those then you'll hit the road running. This generator *must be run in the DesktopModules folder*. It will create subfolders for the organization and module name based on your input. It will allow you to store common settings such as name, email etc to the generator's settings so you don't need to type them in every time you create a module.

## Getting Started

### What is Yeoman? (skip if you know Yeoman and have it installed)

Head over to the [Yeoman site](http://yeoman.io) for in-depth info. Basically it is a code generator based on templates called "generators". You can install Yeoman using npm (i.e. you also need Node js installed first!) as follows:

```bash
npm install -g yo
```

### Installing the DNN SPA module generator

To install this generator, run:

```bash
npm install -g generator-dnn-spa-gulp-react
```

### Use

Now, head over to the DesktopModules folder of your project, start a shell and type:

```bash
yo dnn-spa-gulp-react
```

This will prompt you with a few questions, some of which are mandatory to answer as they are used to configure the module:

1. Project name. Use a short name without special characters or spaces as it is used for namespaces and folders. (e.g. "Map")
2. Primary object name. This is the name of the primary object of your module that you're managing. You can leave this as-is. Again a short name without special characters or spaces (e.g. MapPoints).
3. Organization name. Used to create the subfolder and namespaces as well. So like the above without special characters or spaces, please (e.g. DNNConnect).
4. Url. Url of your organization or any other url you'd like to use for this project. It's used in the DNN manifest.
5. Your name. Used in the manifest.
6. Email. Used in the manifest.

Finally the generator will ask if you wish to overwrite the old settings in the template so that any subsequent module generations will prompt the same name, email, etc. 

## License

MIT
