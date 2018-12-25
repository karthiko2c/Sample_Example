FROM ruby:2.4.0
WORKDIR /app
ADD entrypoint.sh /entrypoint.sh
ADD Gemfile .
ADD Gemfile.lock .
ADD features ./features

ENV TEST_ENV=CI

RUN chmod +x /entrypoint.sh;

ENTRYPOINT ["/entrypoint.sh"]
