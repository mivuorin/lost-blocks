# syntax=docker/dockerfile:1
FROM postgres:17.4-alpine

# Initialize database 
COPY lego.sql /docker-entrypoint-initdb.d/lego.sql
