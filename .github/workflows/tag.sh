#!/bin/bash

run_or_echo() {
    if [[ "$DRY_RUN" = "1" ]]; then
        echo "$@"
    else
        "$@"
    fi
}

export -f run_or_echo

find . -maxdepth 2 -type f -name '*.nupkg' -exec bash -c 'tag=$(basename "$1" .nupkg); git tag "$tag"; run_or_echo git push origin "$tag"' shell {} \;

export TAG
TAG=$(find . -maxdepth 2 -type f -name 'ShapeSifter.*.nupkg' -exec sh -c 'basename "$1" .nupkg' shell {} \;)

case "$TAG" in
  *"
"*)
    echo "Error: TAG contains a newline; multiple packages found."
    exit 1
    ;;
  "")
      echo "Error: no TAG found; aborting."
      exit 2
      ;;
esac

# the empty target_commitish indicates the repo default branch
run_or_echo curl -L -X POST -H "Accept: application/vnd.github+json" -H "Authorization: Bearer $GITHUB_TOKEN" -H "X-GitHub-Api-Version: 2022-11-28" https://api.github.com/repos/G-Research/ShapeSifter/releases -d '{"tag_name":"'"$TAG"'","target_commitish":"","name":"'"$TAG"'","draft":false,"prerelease":false,"generate_release_notes":false}'
