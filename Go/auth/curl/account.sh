rm -rf tmp
mkdir tmp
curl -isX GET 'http://127.0.0.1:8080/account' > tmp/account.get.json
curl -isX POST 'http://127.0.0.1:8080/account' --data '' > tmp/account.post.json
curl -isX PUT 'http://127.0.0.1:8080/account' --data '' > tmp/account.put.json
curl -isX DELETE 'http://127.0.0.1:8080/account' --data '' > tmp/account.delete.json

