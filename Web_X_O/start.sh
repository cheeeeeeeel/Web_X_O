#!/usr/bin/env bash
set -euo pipefail

if [[ -z "${PORT:-}" ]]; then
  export PORT=8080
fi

dotnet run --project "Web_X_O.csproj" --urls "http://0.0.0.0:${PORT}"
