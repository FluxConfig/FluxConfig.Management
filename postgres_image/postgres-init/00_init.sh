#!/bin/bash
set -e

SQL_CONTENT=$(sed \
  -e "s/__POSTGRES_DB__/$POSTGRES_DB/g" \
  -e "s/__PG_MIGRATION_USER__/$PG_MIGRATION_USER/g" \
  -e "s/__PG_MIGRATION_PSWD__/$PG_MIGRATION_PSWD/g" \
  -e "s/__PG_FCM_APP_USER__/$PG_FCM_APP_USER/g" \
  -e "s/__PG_FCM_APP_PSWD__/$PG_FCM_APP_PSWD/g" \
  /docker-entrypoint-initdb.d/init.sql.template)

# Execute the generated SQL directly
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<EOF
$SQL_CONTENT
EOF

  