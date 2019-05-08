#!/bin/bash

url="http://"${VOTING_URL-:"localhost:5000"}"/api/voting"
echo "Executing in .."$url
curl $url  \
--request 'POST' \
--data '["c#","java"]' \
--header 'Content-Type: application/json'

echo '\n'

curl $url  \
--request 'PUT' \
--data '"c#"' \
--header 'Content-Type: application/json' 

echo '\n'

winner=$(curl $url \
--request 'DELETE' \
--silent \
--header 'Content-Type: application/json' | jq -r '.winner')

echo "The winner is "$winner
if [ "$winner" == "c#" ]; then
    echo "PASSED!"
    exit 0 
else
    echo "FAILED!"
    exit 1
fi

echo "The winner is" $winner