#!/usr/bin/env bash
set -euo pipefail

ENGINE="${DB_ENGINE:-sqlserver}"
BACKUP_DIR="${BACKUP_DIR:-./backups}"
TIMESTAMP="$(date -u +"%Y%m%dT%H%M%SZ")"

mkdir -p "$BACKUP_DIR"

if [[ "$ENGINE" == "sqlite" ]]; then
  SQLITE_PATH="${SQLITE_PATH:-./IgrejaSocial.db}"
  if [[ ! -f "$SQLITE_PATH" ]]; then
    echo "Arquivo SQLite não encontrado em: $SQLITE_PATH" >&2
    exit 1
  fi
  DEST="$BACKUP_DIR/IgrejaSocial-$TIMESTAMP.db"
  cp "$SQLITE_PATH" "$DEST"
  echo "Backup SQLite criado em: $DEST"
  exit 0
fi

if [[ "$ENGINE" == "sqlserver" ]]; then
  SQLSERVER_SERVER="${SQLSERVER_SERVER:-}"
  SQLSERVER_DATABASE="${SQLSERVER_DATABASE:-}"
  SQLSERVER_USER="${SQLSERVER_USER:-}"
  SQLSERVER_PASSWORD="${SQLSERVER_PASSWORD:-}"

  if [[ -z "$SQLSERVER_SERVER" || -z "$SQLSERVER_DATABASE" || -z "$SQLSERVER_USER" || -z "$SQLSERVER_PASSWORD" ]]; then
    echo "Defina SQLSERVER_SERVER, SQLSERVER_DATABASE, SQLSERVER_USER e SQLSERVER_PASSWORD." >&2
    exit 1
  fi

  if ! command -v sqlcmd >/dev/null 2>&1; then
    echo "sqlcmd não encontrado. Instale o SQL Server Command Line Tools." >&2
    exit 1
  fi

  DEST="$BACKUP_DIR/${SQLSERVER_DATABASE}-${TIMESTAMP}.bak"
  sqlcmd -S "$SQLSERVER_SERVER" -U "$SQLSERVER_USER" -P "$SQLSERVER_PASSWORD" \
    -Q "BACKUP DATABASE [$SQLSERVER_DATABASE] TO DISK = N'$DEST' WITH INIT, COMPRESSION;"
  echo "Backup SQL Server criado em: $DEST"
  exit 0
fi

echo "DB_ENGINE inválido. Use 'sqlite' ou 'sqlserver'." >&2
exit 1
