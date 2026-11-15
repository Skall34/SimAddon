# Hooks git versionnes (SimAddon)

Installation une fois par clone :

```
git config core.hooksPath githooks
```

## pre-push

Avant chaque push (branche **ou tag**), lance `tests/run_all_tests.sh`
(build Release de la solution + suites de non-regression). Si un test
échoue, le push est annulé — aucune build CI ne partira donc avec des
tests rouges. Bypass exceptionnel : `git push --no-verify`.

La même suite tourne en CI dans `.github/workflows/main.yml` et
`build_version.yml` (job `test`, le job `build` a `needs: test`) :
un tag ne produit une release que si les tests sont verts.
