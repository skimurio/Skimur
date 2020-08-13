#!/bin/sh

# build the docker image
echo "Building Docker Image..."
docker build -t skimur .

# tag the docker image
echo "Tagging the docker image..."
docker tag skimur registry.heroku.com/skimur/web

# login to heroku
echo "Logging into Heroku..."
heroku container:login

# push to heroku
echo "Pushing to Heroku..."
heroku container:push web -a skimur

# Release to web
echo "Releasing to heroku..."
heroku container:release web -a skimur
