# ts handlers
lambda functions in typescript workflow

## local dev installs
* [aws-cli](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html)
* [awscli-local](https://github.com/localstack/awscli-local)
* docker
* [localstack](https://github.com/localstack/localstack)
* node
* python

## dynamodb setup
`awslocal dynamodb create-table --table-name "Planets" --attribute-definitions '[{"AttributeName":"Id","AttributeType":"N"}]' --key-schema "AttributeName=Id,KeyType=HASH" --provisioned-throughput "ReadCapacityUnits=9,WriteCapacityUnits=3"`

