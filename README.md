# asp.net core dynamodb project
more aws, this time with more c# and nosql

## local dev installs
* [aws-cli](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html)
* [awscli-local](https://github.com/localstack/awscli-local)
* docker
* [localstack](https://github.com/localstack/localstack)
* node
* python

## dynamodb setup
`awslocal dynamodb create-table --table-name "Planets" --attribute-definitions '[{"AttributeName":"Universe","AttributeType":"S"},{"AttributeName":"Name","AttributeType":"S"}]' --key-schema '[{"AttributeName":"Universe","KeyType":"HASH"},{"AttributeName":"Name","KeyType":"RANGE"}]' --provisioned-throughput "ReadCapacityUnits=9,WriteCapacityUnits=3"`
