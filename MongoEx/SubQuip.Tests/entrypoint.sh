#!/bin/bash
echo "gem: --no-ri --no-rdoc" > ~/.gemrc
bundle install
bundle exec cucumber
