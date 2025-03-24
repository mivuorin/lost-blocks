# demo-db-integration-test
Demo for database integration test practices

## Quickstart

1. Start Postgres with docker-compose.

It should build custom postgres dockerfile and initialize sample database from lego.sql

```shell
docker-compose up
```

## Scaffolding database

Following script can be used to generate Entity Framework Core entities from existing database schema.

```shell
dotnet ef dbcontext scaffold name=lego --context-dir Data --output-dir Models Npgsql.EntityFrameworkCore.PostgreSQL -- --environment Development
```

## Lego sample database license (lego.sql)

Lego test database (lego.sql) is from here: https://github.com/neondatabase-labs/postgres-sample-dbs?tab=readme-ov-file#lego-database
Which original source is here: https://www.kaggle.com/datasets/rtatman/lego-database
and original license https://creativecommons.org/publicdomain/zero/1.0/

