const { watch } = require('gulp');
var gulp = require('gulp'),
    cssmin = require("gulp-cssmin")
    rename = require("gulp-rename");
const sass = require('gulp-sass')(require('sass'));

gulp.task('default', function () {
    return gulp.src('Colors/scss/site.scss')
        .pipe(sass().on('error', sass.logError))
        // .pipe(cssmin())
        .pipe(rename({
            // suffix: ".min"
        }))
        .pipe(gulp.dest('wwwroot/css/'));
    });

    gulp.task('watch', function(){
        gulp.watch('Colors/scss/*.scss', gulp.series('default'));
    });