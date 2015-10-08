var gulp = require('gulp'),
  msbuild = require('gulp-msbuild'),
  browserify = require('gulp-browserify'),
  minifyCss = require('gulp-minify-css'),
  uglify = require('gulp-uglify'),
  assemblyInfo = require('gulp-dotnet-assembly-info'),
  plumber = require('gulp-plumber'),
  config = require('./package.json'),
  zip = require('gulp-zip'),
  filter = require('gulp-filter'),
  merge = require('merge2'),
  gutil = require('gulp-util'),
  markdown = require('gulp-markdown'),
  rename = require('gulp-rename'),
  manifest = require('gulp-dnn-manifest');

gulp.task('browserify', function() {
  gulp.src('js/src/<%= props.projectName.toLowerCase() %>.js')
    .pipe(plumber())
    .pipe(browserify({
      transform: 'reactify',
      ignore: 'react'
    }))
    .pipe(gulp.dest('js/'));
});

gulp.task('watch', function() {
  gulp.watch('js/src/**/*.js', ['browserify']);
});

gulp.task('assemblyInfo', function() {
  return gulp
    .src('**/AssemblyInfo.cs')
    .pipe(assemblyInfo({
      title: config.dnnModule.friendlyName,
      description: config.description,
      version: config.version,
      fileVersion: config.version,
      company: config.dnnModule.owner.organization,
      copyright: function(value) {
        return 'Copyright ' + new Date().getFullYear() + ' by ' + config.dnnModule.owner.organization;
      }
    }))
    .pipe(gulp.dest('.'));
});

gulp.task('build', ['assemblyInfo'], function() {
  return gulp.src('./<%= props.projectName %>.csproj')
    .pipe(msbuild({
      toolsVersion: 12.0,
      targets: ['Clean', 'Build'],
      errorOnFail: true,
      stdout: true,
      properties: {
        Configuration: 'Release',
        OutputPath: config.dnnModule.pathToAssemblies
      }
    }));
});

gulp.task('packageInstall', ['browserify', 'build'], function() {
  var packageName = config.dnnModule.fullName + '_' + config.version;
  var dirFilter = filter(fileTest);
  return merge(
      merge(
        gulp.src([
          '**/*.html',
          '**/*.resx'
        ], {
          base: '.'
        })
        .pipe(dirFilter),
        gulp.src(['**/*.css'], {
          base: '.'
        })
        .pipe(minifyCss())
        .pipe(dirFilter),
        gulp.src(['js/*.js', '!js/*.min.js'], {
          base: '.'
        })
        .pipe(uglify().on('error', gutil.log)),
        gulp.src(['js/*.min.js'], {
          base: '.'
        })
      )
      .pipe(zip('Resources.zip')),
      gulp.src(config.dnnModule.pathToSupplementaryFiles + '/*.dnn')
      .pipe(manifest(config)),
      gulp.src([config.dnnModule.pathToAssemblies + '/*.dll',
        config.dnnModule.pathToScripts + '/*.SqlDataProvider',
        config.dnnModule.pathToSupplementaryFiles + '/License.txt',
        config.dnnModule.pathToSupplementaryFiles + '/ReleaseNotes.txt'
      ]),
      gulp.src(config.dnnModule.pathToSupplementaryFiles + '/ReleaseNotes.md')
      .pipe(markdown())
      .pipe(rename('ReleaseNotes.txt'))
    )
    .pipe(zip(packageName + '_Install.zip'))
    .pipe(gulp.dest(config.dnnModule.packagesPath));
});

gulp.task('packageSource', ['browserify', 'build'], function() {
  var packageName = config.dnnModule.fullName + '_' + config.version;
  var dirFilter = filter(fileTest);
  return merge(
      gulp.src(['**/*.html',
        '**/*.css',
        'js/**/*.js',
        '**/*.??proj',
        '**/*.sln',
        '**/*.json',
        '**/*.cs',
        '**/*.vb',
        '**/*.resx',
        config.dnnModule.pathToSupplementaryFiles + '**/*.*'
      ], {
        base: '.'
      })
      .pipe(dirFilter)
      .pipe(zip('Resources.zip')),
      gulp.src(config.dnnModule.pathToSupplementaryFiles + '/*.dnn')
      .pipe(manifest(config)),
      gulp.src([config.dnnModule.pathToAssemblies + '/*.dll',
        config.dnnModule.pathToScripts + '/*.SqlDataProvider',
        config.dnnModule.pathToSupplementaryFiles + '/License.txt',
        config.dnnModule.pathToSupplementaryFiles + '/ReleaseNotes.txt'
      ])
    )
    .pipe(zip(packageName + '_Source.zip'))
    .pipe(gulp.dest(config.dnnModule.packagesPath));
})

gulp.task('package', ['packageInstall', 'packageSource'], function() {
  return null;
})

gulp.task('default', ['browserify']);

function fileTest(file) {
  var res = false;
  for (var i = config.dnnModule.excludeFilter.length - 1; i >= 0; i--) {
    res = res | file.relative.startsWith(config.dnnModule.excludeFilter[i]);
  };
  return !res;
}

function startsWith(str, strToSearch) {
  return str.indexOf(strToSearch) === 0;
}