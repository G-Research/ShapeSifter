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
curl_body='{"tag_name":"'"$TAG"'","target_commitish":"","name":"'"$TAG"'","draft":false,"prerelease":false,"generate_release_notes":false}'

# process_curl_error expects no arguments, but reads the output of `curl` from the file `curl_output.json`.
script_exit_code=-1
process_curl_error() {
    jq_query='if .errors | length == 1 then .errors[0].code else null end'
    if ! jq -r --exit-status "$jq_query" curl_output.json; then
        echo "Unexpectedly got number of errors not equal to 1"
        cat curl_output.json
        script_exit_code=3
    else
        github_error="$(jq -r "$jq_query" curl_output.json)"
        echo "Error reported by GitHub: $github_error"

        if [ "$github_error" != "already_exists" ]; then
            echo "Unrecognised error message"
            script_exit_code=4
        else
            # Expected: we tried to make a release that already exists, which can happen when path filters prevent a new tag
            script_exit_code=0
        fi
    fi
}

test_process_curl_error() {
    echo "Running tests."

    failed_output=$(cat <<'EOF'
{
  "message": "Validation Failed",
  "errors": [
    {
      "resource": "Release",
      "code": "already_exists",
      "field": "tag_name"
    }
  ],
  "documentation_url": "https://docs.github.com/rest/releases/releases#create-a-release"
}
EOF
)
    echo "$failed_output" > curl_output.json
    process_curl_error
    if [ $script_exit_code != 0 ] ; then
        echo "Test failure: got exit code $script_exit_code when we expected 0"
        exit $script_exit_code
    fi

    echo "Tests completed."
}

if [[ "$DRY_RUN" = "1" ]]; then
    test_process_curl_error
else
    if curl --fail-with-body -L -X POST -H "Accept: application/vnd.github+json" -H "Authorization: Bearer $GITHUB_TOKEN" -H "X-GitHub-Api-Version: 2022-11-28" https://api.github.com/repos/G-Research/ShapeSifter/releases -d "$curl_body" > curl_output.json ; then
        cat curl_output.json
        echo "cURL succeeded."
    else
        process_curl_error
        exit $script_exit_code
    fi
fi
