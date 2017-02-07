module.exports = function(grunt) {

	var path = require('path');

	// Load the package JSON file
	var pkg = grunt.file.readJSON('package.json');

	// get the root path of the project
	var projectRoot = 'src/' + pkg.name + '/';

	// Load information about the assembly
	var assembly = grunt.file.readJSON(projectRoot + 'Properties/AssemblyInfo.json');

	// Get the version of the package
	var version = assembly.informationalVersion ? assembly.informationalVersion : assembly.version;

	grunt.initConfig({
		pkg: pkg,
		clean: {
			files: [
				'releases/temp/'
			]
		},
		copy: {
			bacon: {
				files: [
					{
						expand: true,
						cwd: projectRoot + 'bin/Release/',
						src: [
							pkg.name + '.dll',
							pkg.name + '.xml',
							'Skybrud.Essentials.dll',
							'Skybrud.Essentials.xml',
							'Newtonsoft.Json.dll',
						],
						dest: 'releases/temp/bin/'
					}
				]
			}
		},
		zip: {
			release: {
				cwd: 'releases/temp/',
				src: [
					'releases/temp/**/*.*'
				],
				dest: 'releases/github/' + pkg.name + '.v' + version + '.zip'
			}
		},
		nugetpack: {
			dist: {
				src: 'src/' + pkg.name + '/' + pkg.name + '.csproj',
				dest: 'releases/nuget/'
			}
		}
	});

	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-nuget');
	grunt.loadNpmTasks('grunt-zip');

	grunt.registerTask('dev', ['clean', 'copy', 'zip', 'nugetpack', 'clean']);

	grunt.registerTask('default', ['dev']);

};