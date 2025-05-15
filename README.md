# LostBlocks

Demo for database integration test practices. LostBlocks is api for managing Lego sample database similar
to https://rebrickable.com/.

Project simulates generic api project with database and how it can be tested with real database server, instead of using
mock or stub replacement.

## Quickstart

1. Start Postgres with docker-compose.

It should build custom postgres dockerfile and initialize sample database from lego.sql

```shell
docker-compose up
```

2. Run tests

```shell
dotnet test
```

3. Run Api project

```shell
dotnet run --project LostBlocks.Api
```

## Why?

Containerization and computation resources have improved in such way that abstracting database dependency from tests is
not needed anymore.

In past installing database servers was cumbersome and could not be done on the fly, which made setting up development
environments and CI pipelines hard. This problem is now solved with docker containers.

Abstracting database dependency takes significant extra effort from architecture design and almost always ends up
causing leaky abstraction problems.

This demo project is a study on what kind of effects using real database introduce to normal development process with
unit testing.

## Characteristics of good tests

Writing and running tests should be as frictionless as possible, otherwise developer start skipping writing tests which
eventually leads to poor test coverage.

Supporting and maintaining test automation with poor coverage has very low return of investment value.

There's F.I.R.S.T rule of tests which help to reduce friction caused by tests:

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
tests by mismatch between in-memory stub and real database used in production.

There will be no need for additional integration tests which usually overlap code covered by unit tests.

### ORM (Object - Relational - Mapper) as database abstraction layer

Using ORM to hide database implementation and enable unit testing can save a lot of time, but
ORM code still needs to be tested against real database to avoid running in problems where code behaves differently when
running against real database.

ORM can generate invalid SQL query, and sometimes with very subtle differences, when run against database.
ORMs need to be configured to correctly map against database schema, which can be different in real database.

## Patterns to avoid problems caused by shared database

### Transaction Rollback on Teardown -pattern

Single test is run inside of database transaction which will be rolled back when test is finished.
This makes sure that tests do not have persistent data and database state keeps unmodified after tests.

### Shared database connection in tests

Creating database connections can cause overhead when run once per tests, so use either shared connection between
tests or database connection pooling.

Entity Framework already uses connection pooling on lower level, so it does not require extra configuration.

### Do not share instance of DbContext between tests (TODO)

(TODO write with general orm concept and use EF as specific.)

Entity Framework DbContext has internal state (change tracker, identity map...) which should not be shared between
tests.

For example adding single entity to database which violates any constraint will break all tests if context is shared
between tests.

Tests uses PooledDbContextFactory to instantiate DbContext, which automatically reuses mapping configuration in setup
and resets context state
when context is disposed. This makes context creation faster.

### Tests arrange their own data (TODO)

Tests should always have full control in their own test data.

* use generators
* avoid sharing test cases
* avoid relying on shared state in database

Avoid shared state at all costs!

https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=without-di%2Cexpression-api-with-constant#dbcontext-pooling

## Technical details

### Automatic request validation

Api uses [FluentValidation](https://docs.fluentvalidation.net/)
and [SharpGrid.FluentValidation.AutoValidation](https://github.com/SharpGrip/FluentValidation.AutoValidation) for
validating request DTO's automatically on Asp.Net modeling binding.

### Scaffolding database

Following script can be used to generate Entity Framework Core entities from existing database schema.

```shell
dotnet ef dbcontext scaffold name=lego --context-dir Data --output-dir Models Npgsql.EntityFrameworkCore.PostgreSQL -- --environment Development
```

### Docker database container

Project uses custom Postgres docker image [lego-db.dockerfile](lego-db.dockerfile) which is seeded from Lego sample
database [lego.sql](lego.sql)
and run with docker-compose [compose.yaml](compose.yaml)

### Entity framework and nullable reference types

Entity framework uses virtual proxies to populate entities when executing queries, which does not always work well with
.Net's nullable reference type -feature.

Related entities can either be `null` or populated on demand with `Include` method or by lazy loading.

There's a trade off in declaring entity relations as nullable or not:

#### Nullable - requires extra null checks in code

Every query needs to have extra null check when traversing relations.

```csharp
var part = Context.Parts
    .Include(p => p.Category)
    .Where(p => p.Category != null && p.Category.Name == category.Name);
```

Benefit is that compiler gives warnings on missing null checks and runtime null exceptions should not happen.

#### Not null - possible runtime null exceptions

All relations need to be initialized to null or empty collections to suppress uninitialized nullable property -warning.

```csharp
public LegoPartCategory Category { get; set; } = null!;
```

Benefit is that now there is no need for extra null checks in queries, but runtime null exceptions might happen if query
does not populate relationship.

```csharp
var part = Context.Parts
    .Include(p => p.Category)
    .Where(p => p.Category.Name == category.Name);
```

Projects uses latter approach to reduce extra code and relies on unit tests
and [IDE code inspection](https://www.jetbrains.com/help/rider/EntityFramework.NPlusOne.IncompleteDataQuery.html) for
catching runtime exceptions.

## Problems in sample database

### Sequences are not set to correct values after seeding data

Some tables use sequences for primary keys which are not set to correct starting values after seeding data.
This causes sequences to generate conflicting primary key values on inserts.

This is fixed in [lego-sequence-fix.sql](lego-sequence-fix.sql).

### Foreign key constraints are missing

Most of the tables are missing foreign key constraints.

### Delete cascades are also missing because of missing foreign keys

Not having proper delete rules configured for relationships makes it hard to do proper deletes
and can lead to accidental data loss when deleting graphs.

Developers should take be extra careful when working with Entity Framework because, Entity Framework uses cascading
delete behaviour by default.

## Licenses

### Lego sample database license (lego.sql)

Lego set can consist of one or more Inventories and is identified by set number (set_num eg. 8088-1)

There might be multiple Inventories if the Parts in the Set were changed during it's production period, or perhaps to
cater for different geographic regions.

Each Inventory can contain Parts and/or Sets. Most Inventories contain just Parts, but some consist solely of Sets (e.g.
5004559-1) or a combination of Parts and Sets (e.g. 9509-1).

Lego test database (lego.sql) is from
here: https://github.com/neondatabase-labs/postgres-sample-dbs?tab=readme-ov-file#lego-database
Which original source is here: https://www.kaggle.com/datasets/rtatman/lego-database
and original license https://creativecommons.org/publicdomain/zero/1.0/

Original source of the data is from Rebrickable CSV datasets which can be found from here
https://rebrickable.com/downloads/

Rebrickable does only provide CSV data sets and not database schema. Updated database schema diagram can be found here.
https://rebrickable.com/help/lego-database/


