#!/bin/bash
POSTFIX=${1/:/-}
POSTFIX=${POSTFIX/\//_}

docker run --name mongodb$POSTFIX -p 27017:27017 -d mongo
docker run --name sqb$POSTFIX --link mongodb$POSTFIX:localhost -p 3000:80 -d $1
sleep 5
docker build -t test$1 .
docker run --name test$POSTFIX --env URL_PREFIX="sqb" --link sqb$POSTFIX:sqb test$1

function finish {
  docker stop mongodb$POSTFIX || true
  docker rm mongodb$POSTFIX || true
  docker stop sqb$POSTFIX || true
  docker rm sqb$POSTFIX || true
  docker stop test$POSTFIX || true
  docker rm test$POSTFIX || true
}
trap finish EXIT

