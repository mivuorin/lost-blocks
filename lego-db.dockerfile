# syntax=docker/dockerfile:1
FROM postgres:17.4-alpine

# Initialize database 
COPY lego.sql /docker-entrypoint-initdb.d/0-lego.sql
COPY lego-sequence-fix.sql /docker-entrypoint-initdb.d/1-lego-sequence-fix.sql
