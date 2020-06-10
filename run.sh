#!\bin\bash

docker build -t skimurweb .
docker run -p 5000:80 skimurweb