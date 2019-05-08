FROM ubuntu:18.04

RUN apt-get update && apt-get install curl jq -y

WORKDIR /tests
COPY test-api.sh .

ENTRYPOINT [ "bash", "-c", "./test-api.sh" ]
