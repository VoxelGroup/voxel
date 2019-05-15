set -e

docker-compose down
docker-compose up -d --build 

sleep 2

docker-compose run --rm integrationtest
# docker-compose push
kubectl set image deploy/votingapp votingapp=votingapp:${TAG} --record