/// <binding BeforeBuild='move:bootrap' />
"use strict";
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/",
    npm: "./node_modules/"
};

paths.bootstrapJsSource = paths.npm + "bootstrap/dist/**/*.min.js";
paths.bootstrapCssSource = paths.npm + "bootstrap/dist/**/*.min.css";
paths.bootstrapDest = paths.webroot + "lib/bootstrap";

//paths.js = paths.webroot + "js/**/*.js";
//paths.minJs = paths.webroot + "js/**/*.min.js";
//paths.css = paths.webroot + "css/**/*.css";
//paths.minCss = paths.webroot + "css/**/*.min.css";
//paths.concatJsDest = paths.webroot + "js/site.min.js";
//paths.concatCssDest = paths.webroot + "css/site.min.css";

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

// rimraf verilen path'i temizliyor
gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

// rimraf verilen path'i temizliyor
gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

//gulp.task("move:bootstrap", function () {
//    return gulp.src([paths.bootstrapCssSource, paths.bootstrapJsSource]).pipe(gulp.dest(paths.bootstrapDest));
//});


// moves everything 
//gulp.task("copy:bootstrap", function () {
//    return gulp.src(paths.npm + "bootstrap/**/*.*").pipe(gulp.dest(paths.webroot + "lib/bootstrap"));
//});

//gulp.task("copy:icheck", function () {
//    return gulp.src(paths.npm + "/icheck/**/*.*").pipe(gulp.dest(paths.webroot + "lib/icheck"));
//});

//gulp.task("copy:jquery", function () {
//    return gulp.src(paths.npm + "/jquery/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery"));
//});

//gulp.task("copy:jquery-ui", function () {
//    return gulp.src(paths.npm + "/jquery-ui/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery-ui"));
//});

//gulp.task("copy:jquery-ui-dist", function () {
//    return gulp.src(paths.npm + "/jquery-ui-dist/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery-ui-dist"));
//});

//gulp.task("copy:jquery-validation", function () {
//    return gulp.src(paths.npm + "jquery-validation/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery-validation"));
//});

//gulp.task("copy:jquery-validation-unobtrusive", function () {
//    return gulp.src(paths.npm + "jquery-validation-unobtrusive/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery-validation-unobtrusive"));
//});

//gulp.task("copy:select2", function () {
//    return gulp.src(paths.npm + "select2/**/*.*").pipe(gulp.dest(paths.webroot + "lib/select2"));
//});

//gulp.task("copy:toastr", function () {
//    return gulp.src(paths.npm + "/toastr/**/*.*").pipe(gulp.dest(paths.webroot + "lib/toastr"));
//});

//gulp.task("copy:twbs-pagination", function () {
//    return gulp.src(paths.npm + "/twbs-pagination/**/*.*").pipe(gulp.dest(paths.webroot + "lib/twbs-pagination"));
//});

//gulp.task("copy:fine-uploader", function () {
//    return gulp.src(paths.npm + "/fine-uploader/**/*.*").pipe(gulp.dest(paths.webroot + "lib/fine-uploader"));
//});

//gulp.task("copy:magnific-popup", function () {
//    return gulp.src(paths.npm + "/magnific-popup/**/*.*").pipe(gulp.dest(paths.webroot + "lib/magnific-popup"));
//});

//gulp.task("copy:ionicons", function () {
//    return gulp.src(paths.npm + "/ionicons/**/*.*").pipe(gulp.dest(paths.webroot + "lib/ionicons"));
//});

//gulp.task("copy:font-awesome", function () {
//    return gulp.src(paths.npm + "/font-awesome/**/*.*").pipe(gulp.dest(paths.webroot + "lib/font-awesome"));
//});

//gulp.task("copy:jquery-tageditor", function () {
//    return gulp.src(paths.npm + "/jquery-tageditor/**/*.*").pipe(gulp.dest(paths.webroot + "lib/jquery-tageditor"));
//});

//gulp.task("copy:ckeditor", function () {
//    return gulp.src(paths.npm + "/ckeditor/**/*.*").pipe(gulp.dest(paths.webroot + "lib/ckeditor"));
//});

//gulp.task("copy:highlightjs", function () {
//    return gulp.src(paths.npm + "highlightjs/**/*.*").pipe(gulp.dest(paths.webroot + "lib/highlightjs"));
//});


gulp.task("min", ["min:js", "min:css"]);

//gulp.task("copy", ["copy:bootstrap", "copy:icheck", "copy:jquery", "copy:jquery-ui", "copy:jquery-ui-dist", "copy:jquery-validation", "copy:jquery-validation-unobtrusive", "copy:select2", "copy:toastr", "copy:twbs-pagination", "copy:fine-uploader", "copy:magnific-popup", "copy:ionicons", "copy:font-awesome", "copy:jquery-tageditor", "copy:ckeditor", "copy:highlightjs"])