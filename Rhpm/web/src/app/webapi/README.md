#Set up environment for generating the swagger client

- run npm install ng-swagger-gen --save-dev for swagger client installation
it will install the swagger packages and package detail in package.json file

# for generating content
- run node_modules/.bin/ng-swagger-gen -i <path_to_swagger_json> [-o output_dir]

- here path_to_swagger_json is either a relative path to the Swagger JSON file or an URL.

- output_dir is the directory where the generated code will be outputted. It is recommended that this directory is ignored on GIT (or whatever source control software you are using), for example, by adding its name to .gitignore. The default output directory if nothing is specified is src/app/api.

- testing purpose for current project 
- node_modules/.bin/ng-swagger-gen -i http://localhost:50035/swagger/v1.0/swagger.json -o ./src/app/webapi

# reference link
- https://www.npmjs.com/package/ng-swagger-gen