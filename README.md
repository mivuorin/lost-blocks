# demo-db-integration-test

Demo for database integration test practices.

## Quickstart

1. Start Postgres with docker-compose.

It should build custom postgres dockerfile and initialize sample database from lego.sql

```shell
docker-compose up
```

## Characteristics of good tests

F.I.R.S.T rule of tests.

Fast - Test runtime matters and when it reaches certain limit it will slow down development.

Tests will be run all the time during development. Aim in situation where you can keep IDE's test auto run enabled.

Independent - Tests need to be run in any order and tests can't rely on existing test data.

Every test needs to set up their own data. Dependencies in tests increase complexity which makes
it harder to maintain tests and reason why tests fail and in the end lowering trust for tests.

Repeatable - Test run should always produce same result when production code has not changed.

Random failures reduce trust on the tests and can waste a lot of time hard to find bugs in test code.
Common causes are not having control over datetimes, guids or other generated values, which cause brittle tests.

Self-Validating - Test should always produce binary result: test either pass or fail. Developers should not need to look
on logs or other sources to see if test failed.
Tests should also fail for single reason, and it should be obvious why test failed.

Timely - Tests should be easy to write and should support development. Writing tests should not be an
endeavor which is left for later because of time constraints.

Make sure tests are refactored and developers have access to good utilities when creating tests.
This includes test data generators and easy way to set up different states for tests.

## How database complicates testing?

Database causes all tests to share state which can easily break Independent rule of the tests.

eg. one test modifies rows in database which causes other test to fail.

Database also adds overhead to tests and can sometimes significantly slow down test execution.

eg. queries over physical layer to real database are way slower than using in-memory stub for tests.

Databases also have their own sequence and datetime generators which can cause brittle tests.

Usually database is replaced with in-memory stub in most of the unit tests to avoid these problems
and then real database is used only for subset of integration tests.

## Why it's beneficial to use real database in tests?

It's very common for applications, especially web apps, to store data in database. Most of the simple apps
code is categorized under CRUD (Create - Retrieve - Update - Delete), so most of the code depends on database.

Using real database in all tests helps in improving test coverage and in finding those pesky bugs which leak through
tests
by mismatch between in-memory stub and real database used in production.

There will be no need for additional integration tests which usually overlap code covered by unit tests.

### ORM (Object - Relational - Mapper) as database abstraction layer

Using ORM to hide database implementation and enable unit testing can save a lot of time, but
ORM code still needs to be tested against real database to avoid running in problems where code behaves differently when
running against real database.

ORM can generate invalid SQL query, and sometimes with very subtle differences, when run against database.
ORMs need to be configured to correctly map against database schema, which can be different in real database.

## Patterns to avoid problems caused by shared database

### 1. Transaction Rollback on Teardown pattern

Single test is run inside of database transaction which will be rolled back when test is finished.
This makes sure that tests there is no persistent data

### Shared database connection in tests

All tests share single instance of Entity Framework Context which manages connections to database. This reduces overhead for single test to open database connection.

## Scaffolding database

Following script can be used to generate Entity Framework Core entities from existing database schema.

```shell
dotnet ef dbcontext scaffold name=lego --context-dir Data --output-dir Models Npgsql.EntityFrameworkCore.PostgreSQL -- --environment Development
```

## Docker database container

Project uses custom Postgres docker image [lego-db.dockerfile](lego-db.dockerfile) which is seeded from Lego sample
database [lego.sql](lego.sql)
and run with docker-compose [compose.yaml](compose.yaml)

## Lego sample database license (lego.sql)

Lego test database (lego.sql) is from
here: https://github.com/neondatabase-labs/postgres-sample-dbs?tab=readme-ov-file#lego-database
Which original source is here: https://www.kaggle.com/datasets/rtatman/lego-database
and original license https://creativecommons.org/publicdomain/zero/1.0/

