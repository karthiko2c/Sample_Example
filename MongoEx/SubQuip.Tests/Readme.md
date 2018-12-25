SubQuip backend testing
========================

This testing project is based on [Cucumer]. This allows us to test the API at a functional level and serves as automated accetance test.

Running tests
--------------

There are two ways to run the acceptance tests. Either locally or uising [Docker].

### Locally

#### Prerequisites

The following list contains software required to run the cucumber tests

- [Ruby]
- [Bundler]
- [Docker]

##### Installing Ruby

Ruby can be installd directly using most package managers such as `brew` and `apt-get`.

For windows the recomanded way to install Ruby is to use either [Chocolatey] or the [RubyInstaller] directly.

##### Installing Bundler
Bundler is intalled using `gem` which is a package manager for Ruby. Gem should already be installed along with ruby. To install Bundler run the following command.

`gem install bundler`

#### Running the cucmber tests

>All commands in this section assumes that the current working directory is the SubQuip.Tests directory.

The first time you run cucumber, in order to install cucumber and it dependencies, run `bundle install`.

Then, and for all subsequent run simply run: `cucumber`

The tests should now run.

### With Docker

In order to facilitate tesing with CI, a docker-based setup has been created.

#### Prerequisites

- [Docker]
- Bash

#### Running the cucmber tests

The easiest to run the tests using docker is to use the runCI.sh script. This script takses the image name of the SubQuip backend as an argument. Thus in order to run ew first build the backend and copy its image name. Then run the following command: 

`./runCI.sh <image_name>`

Writing new tests
----------------

Tests are created by adding new files in the features directory with the file-extention `.feature`.

Each feature file contains a single feature section and several scenarios.

The Feature section follows the following pattern:

<pre>Feature: title
    Description
</pre>

The Secnarios follw the following patten:

<pre>
Scenario: title
  Given Precondition
  And Other preconditon
  When Action 1
  And Action 2
  Then Assertion 1
  And Assertion 2
</pre>

Each Given, When, Then and And line are interpreted by cucumber and matched to a step definition using regular expressions. The step definitions are defined in `.rb` files in the directory `features/step_definitions`. In addition, a number of steps are availble from the library [cucumber-api].


[Cucumer]: http://cucumber.io
[cucumber-api]:https://github.com/hidroh/cucumber-api
[Docker]: https://www.docker.com
[Ruby]: https://www.ruby-lang.org
[Bundler]: https://bundler.io/
[RubyInstaller]: https://rubyinstaller.org/
[Chocolatey]: https://chocolatey.org/