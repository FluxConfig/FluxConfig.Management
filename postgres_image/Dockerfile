FROM postgres:17.4

ENV POSTGRES_DB=fcm_db

COPY ["./postgres-init/", "/docker-entrypoint-initdb.d/"]

RUN chmod +x /docker-entrypoint-initdb.d/*.sh && \
    chown -R postgres:postgres /docker-entrypoint-initdb.d