#!/bin/sh
# Lance TOUTES les suites de tests SimAddon AVANT toute build ou distribution.
# Appele par le hook git pre-push (githooks/pre-push) et par la CI
# (.github/workflows/main.yml et build_version.yml, job "test" en amont du build).
#
# Exit 0 = tout est vert, 1 = au moins un echec -> bloque le push / la build.
# Les suites UDP tournent SEQUENTIELLEMENT (ports differents de toute instance
# SimAddon/X-Plane en cours : 49195 par defaut, surchargeable via SIMBRIDGE_TEST_PORT).
set -e
cd "$(dirname "$0")/.."

echo "== 0. Build de la solution (Release) =="
dotnet build SimAddon.sln -c Release

echo "== 1. Suites de non-regression SimDataManager.Tests =="
dotnet run -c Release --project tests/SimDataManager.Tests

echo "== TOUTES LES SUITES SONT VERTES =="
