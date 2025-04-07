#!/bin/bash
set -e

sed \
  -e "s/__POSTGRES_DB__/$POSTGRES_DB/g" \
  -e "s/__PG_MIGRATION_USER__/$PG_MIGRATION_USER/g" \
  -e "s/__PG_MIGRATION_PSWD__/$PG_MIGRATION_PSWD/g" \
  -e "s/__PG_FCM_APP_USER__/$PG_FCM_APP_USER/g" \
  -e "s/__PG_FCM_APP_PSWD__/$PG_FCM_APP_PSWD/g" \
  /docker-entrypoint-initdb.d/init.sql.template > /docker-entrypoint-initdb.d/01_init.sql