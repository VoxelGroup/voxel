curl http://localhost:5000/api/voting \
--request 'POST' \
--data '["c#","java"]' \
--header 'Content-Type: application/json'

curl http://localhost:5000/api/voting \
--request 'PUT' \
--data '"c#"' \
--header 'Content-Type: application/json'